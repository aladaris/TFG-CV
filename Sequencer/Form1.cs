using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

using VisualTools;

namespace Sequencer {

    /// <summary>
    /// Estados en los que se puede encontrar el sistema (aplicación).
    ///     INIT: Estado de inicialización del sistema. Por defecto el
    ///           sistema comienza en este estado
    ///     PERS_CALIB: La cámara (objeto Capture) ha sido inicializada.
    ///                 Se espera que el usuario introduzca un rectángulo
    ///                 de referencia para realizar la correción de perspectiva.
    ///                 En este estado se muestra, en el display principal, la
    ///                 imagen RAW obtenida con la cámara.
    ///     BOARD_SETUP: La cámara (objeto Capture) ha sido inicializada.
    ///                  Se están recibiendo frames con la perspectiva corregida.
    ///                  Se espera que el usuario establezca los pasos del secuenciador
    /// </summary>
    public enum SYSTEM_STATE { INIT, PERS_CALIB, BOARD_SETUP };

    public partial class Form1 : Form {

        private Board _board;
        private PolygonDrawingTool _polyDrawTool;
        private Capture _camera;
        private bool _capturing = false;
        private SYSTEM_STATE _currentState = SYSTEM_STATE.INIT;

        #region Form Manage
        public Form1() {
            InitializeComponent();
            try {
                _camera = new Capture();  // TODO: Selección de cámara
            } catch (NullReferenceException ex) {
                MessageBox.Show(ex.Message);
            }
            _board = new Board(_camera, imageBox_mainDisplay);
            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
        }

        private void Form1_Load(object sender, EventArgs e) {
            _camera.ImageGrabbed += OnImageGrabbed;
            _board.PerspectiveCorrectedFrame += OnPerspectiveCorrectedFrame;
            this.Text = "Sequencer - INIT";  // DEBUG
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            _camera.Stop();
            _camera.Dispose();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// This method renders the "RAW" image retrieved from the capture object.
        /// </summary>
        /// <param name="sender">This our Capture object</param>
        /// <param name="e"></param>
        private void OnImageGrabbed(object sender, EventArgs e) {
            if ((_currentState == SYSTEM_STATE.PERS_CALIB) || (_currentState == SYSTEM_STATE.INIT)) {
                Size s = imageBox_mainDisplay.Size;
                imageBox_mainDisplay.Image = ((Capture)sender).RetrieveBgrFrame().Resize(s.Width, s.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            } else
                _camera.ImageGrabbed -= OnImageGrabbed; // Sino estamos en el estado correcto, nos damos de baja del evento de la cámara
        }

        private void OnPerspectiveCorrectedFrame(Image<Bgr, Byte> i_img, EventArgs e) {
            if (_currentState == SYSTEM_STATE.BOARD_SETUP) {
                Size s = imageBox_mainDisplay.Size;
                imageBox_mainDisplay.Image = i_img;
            }
        }

        private void OnReturnedPerspetiveCalibrationPolygon(List<Point> i_corners) {
            if (i_corners.Count == 4) {
                _board.SetPerspectiveCalibration(i_corners);
                _currentState = SYSTEM_STATE.BOARD_SETUP;
                checkBox_perspectRectangle.Checked = false;
                checkBox_perspectRectangle.Text = "Reset perspective correction polygon";
                this.Text = "Sequencer - BOARD_SETUP";  // DEBUG
            } else {
                MessageBox.Show("We need exactly four (4) Points for the calibration", "INFO");
            }
        }
        #endregion

        #region GUI Handlers
        /// <summary>
        /// This method handles the startup and stopping of the capture thread.
        /// </summary>
        private void button_startCamera_Click(object sender, EventArgs e) {
            if (!_capturing) {
                _camera.Start();
                button_startCamera.Text = "Stop Camera";
                checkBox_perspectRectangle.Enabled = true;
            } else {
                _camera.Stop();
                button_startCamera.Text = "Start Camera";
            }
            _capturing = !_capturing;
        }

        /// <summary>
        /// Enable/Disable the perspective polygon drawing.
        /// </summary>
        private void checkBox_perspectRectangle_CheckedChanged(object sender, EventArgs e) {

            switch (_currentState) {
                case SYSTEM_STATE.INIT:
                    _currentState = SYSTEM_STATE.PERS_CALIB;
                    this.Text = "Sequencer - PERS_CALIB";  // DEBUG
                    goto case SYSTEM_STATE.PERS_CALIB;  // C# no permite falldown en los case (i.e: no permite un case sin break)
                case SYSTEM_STATE.PERS_CALIB:
                    // Queremos obtener el poligono de correcion de perspectiva, para ello
                    // enlazamos con el manejador para dicho evento (i.e: OnReturnedPerspetiveCalibrationPolygon).
                    if (checkBox_perspectRectangle.Checked) {
                        _polyDrawTool.ReturnPolygon += OnReturnedPerspetiveCalibrationPolygon;
                    } else {
                        _polyDrawTool.ReturnPolygon -= OnReturnedPerspetiveCalibrationPolygon;
                    }
                    break;
                case SYSTEM_STATE.BOARD_SETUP:
                    if (checkBox_perspectRectangle.Checked) {
                        /* Aquí se desea volver a realizar el ajuste de perspectiva.
                         * por lo que dejamos el sistema en el mismo estado que antes
                         * del calibrado (y no sólo la variable _currentstate, sino
                         * todo lo que ellos conlleva).
                         * */
                        _board.ResetPerspectiveCalibration();
                        _currentState = SYSTEM_STATE.PERS_CALIB;
                        _camera.ImageGrabbed += OnImageGrabbed;
                        goto case SYSTEM_STATE.PERS_CALIB;
                    }
                    break;
            }


            _polyDrawTool.Enabled = checkBox_perspectRectangle.Checked;
        }
        #endregion


    }
}
