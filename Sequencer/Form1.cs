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
using Emgu.CV.UI;

using Stateless;

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
    public enum State { Init, Configuration, PersCalib, Idle, BoardSetup, AddSteps };
    public enum Trigger { Initialize, StartPersCalib, StopPersCalib, NewSteps, StopCVEdition };


    public partial class Form1 : Form {
        #region Atributes
        private PolygonDrawingTool _polyDrawTool;
        private Sequencer _sequencer;
        private StateMachine<State, Trigger> _stmachine = new StateMachine<State, Trigger>(State.Init);
        #endregion

        #region Form Manage
        public Form1() {
            InitializeComponent();
            _sequencer = new Sequencer(imageBox_mainDisplay);
            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
        }

        private void Form1_Load(object sender, EventArgs e) {
            InitStateMachine();
            _stmachine.Fire(Trigger.Initialize);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            _sequencer.Camera.ImageGrabbed -= OnImageGrabbed;
            _sequencer.Dispose();
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// This method renders the "RAW" image retrieved from the capture object.
        /// </summary>
        /// <param name="sender">This our Capture object</param>
        /// <param name="e"></param>
        public void OnImageGrabbed(object sender, EventArgs e) {
            if ((_stmachine.IsInState(State.Configuration)) || (_stmachine.IsInState(State.Init)) || (_stmachine.IsInState(State.Idle))) {
                Size s = imageBox_mainDisplay.Size;
                imageBox_mainDisplay.Image = ((Capture)sender).RetrieveBgrFrame().Resize(s.Width, s.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            } else
                _sequencer.Camera.ImageGrabbed -= OnImageGrabbed; /* Sino estamos en el estado correcto, nos damos de baja del evento de la cámara;
                                                                   * así nos aseguramos de dar de baja el manejador */
        }

        /// <summary>
        /// Triggered when the user creates a polygon during the perspective correction calibration
        /// </summary>
        /// <param name="i_corners">The four corners of the polygon returned by a PolygonDrawingTool</param>
        private void OnPerspetiveCalibrationPolygon(List<Point> i_corners) {
            if (_stmachine.IsInState(State.PersCalib)) {
                if (i_corners.Count == 4) {
                    _sequencer.SetPerspectiveCalibration(i_corners);
                    _stmachine.Fire(Trigger.StopPersCalib);
                } else {
                    MessageBox.Show("We need exactly four (4) Points for the calibration", "INFO");
                }
            }
        }

        private void OnStepPolygon(List<Point> i_vertices) {
            if (_stmachine.IsInState(State.AddSteps)) {
                if (i_vertices.Count > 0) {
                    _sequencer.AddStep(i_vertices);
                }
            }
        }
        #endregion

        #region GUI Handlers
        /// <summary>
        /// This method handles the startup and stopping of the capture thread.
        /// </summary>
        private void button_startCamera_Click(object sender, EventArgs e) {
            if (!_sequencer.Capturing) {
                _sequencer.StartCapture();
                button_startCamera.Text = "Stop Camera";
                checkBox_perspectRectangle.Enabled = true;
            } else {
                _sequencer.StopCapture();
                button_startCamera.Text = "Start Camera";
                checkBox_perspectRectangle.Enabled = false;
            }
        }

        /// <summary>
        /// Enable/Disable the perspective polygon drawing.
        /// </summary>
        private void checkBox_perspectRectangle_CheckedChanged(object sender, EventArgs e) {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked) {
                _stmachine.Fire(Trigger.StartPersCalib);
            } else {
                _stmachine.Fire(Trigger.StopPersCalib);
            }
        }
        /// <summary>
        /// Enable/Disable Adding Steps to the sequencer
        /// </summary>
        private void checkBox_addSteps_CheckedChanged(object sender, EventArgs e) {
            CheckBox cb = (CheckBox)sender;
            if (cb.Checked)
                _stmachine.Fire(Trigger.NewSteps);
            else
                _stmachine.Fire(Trigger.StopCVEdition);
        }
        #endregion

        #region StateMachine Logic
        /// <summary>
        /// Defines the states and transitions of the state machine.
        /// </summary>
        private void InitStateMachine() {
            // INIT
            _stmachine.Configure(State.Init)
                .PermitReentry(Trigger.Initialize)
                .OnEntry(() => Initialize())
                .PermitIf(Trigger.StartPersCalib, State.PersCalib, () => IsInitialized());
            // PERSPECTIVE CALIBRATION
            _stmachine.Configure(State.PersCalib)
                .SubstateOf(State.Configuration)
                .OnEntry(() => ResetPerspectiveCalibration())
                .OnExit(() => EndPerspectiveCalibration())
                .Permit(Trigger.StopPersCalib, State.Idle)
                .PermitIf(Trigger.NewSteps, State.BoardSetup, () => _sequencer.PerspectiveCalibrated);
            // IDLE
            _stmachine.Configure(State.Idle)
                .OnEntry(() => EnterIdle())
                .Permit(Trigger.StartPersCalib, State.PersCalib)
                .PermitIf(Trigger.NewSteps, State.AddSteps, () => _sequencer.PerspectiveCalibrated);
            // BOARD SETUP
            _stmachine.Configure(State.BoardSetup)
                .SubstateOf(State.Configuration)
                .Permit(Trigger.StartPersCalib, State.PersCalib);
            // ADD STEPS
            _stmachine.Configure(State.AddSteps)
                .SubstateOf(State.BoardSetup)
                .OnEntry(() => StartAddSteps())
                .OnExit(() => StopAddSteps())
                .Permit(Trigger.StopCVEdition, State.Idle);
        }

            #region Init State
        private void Initialize() {
            if (_sequencer != null)
                _sequencer.Camera.ImageGrabbed += OnImageGrabbed;
            this.Text = "Sequencer - INIT";  // DEBUG ?
        }

        private bool IsInitialized() {
            if (_sequencer != null) {
                if (_sequencer.Capturing)
                    return true;
            }
            return false;
        }
            #endregion

            #region PersCalib State
        private void ResetPerspectiveCalibration() {
            /* Aquí se desea volver a realizar el ajuste de perspectiva.
            * por lo que dejamos el sistema en el mismo estado que antes
            * del calibrado (y no sólo la variable _currentstate, sino
            * todo lo que ellos conlleva).
            * */
            _sequencer.ResetPerspectiveCalibration();
            _sequencer.Camera.ImageGrabbed += OnImageGrabbed;
            // Polygon handling
            _polyDrawTool.Enabled = true;
            _polyDrawTool.ReturnPolygon += OnPerspetiveCalibrationPolygon;
            // Board Handling
            _sequencer.DrawSteps = false;
            // GUI
            this.Text = "Sequencer - PERS_CALIB";  // DEBUG?
        }

        private void EndPerspectiveCalibration() {
            // GUI
            _polyDrawTool.ReturnPolygon -= OnPerspetiveCalibrationPolygon;
            _polyDrawTool.Enabled = false;
            checkBox_perspectRectangle.Checked = false;
        }
            #endregion

            #region Idle State
        private void EnterIdle() {
            if (_sequencer.PerspectiveCalibrated) {
                _sequencer.Camera.ImageGrabbed -= OnImageGrabbed;
                // GUI
                checkBox_perspectRectangle.Text = "Reset perspective correction polygon";
                checkBox_addSteps.Enabled = true;
            } else {
                _sequencer.Camera.ImageGrabbed += OnImageGrabbed;
                _sequencer.DrawSteps = false;
                // GUI
                checkBox_perspectRectangle.Text = "Set perspective correction polygon";
                checkBox_addSteps.Enabled = false;
            }
            checkBox_perspectRectangle.Enabled = true;
            this.Text = "Sequencer - IDLE";  // DEBUG ?

        }
            #endregion

            #region AddSteps State
        private void StartAddSteps() {
            // Polygon handling
            _polyDrawTool.Enabled = true;
            _polyDrawTool.ReturnPolygon += OnStepPolygon;
            // Board Handling
            _sequencer.DrawSteps = true;
            // GUI
            checkBox_perspectRectangle.Enabled = false;
            this.Text = "Sequencer - ADD_STEPS";  // DEBUG ?
        }

        private void StopAddSteps() {
            // Polygon handling
            _polyDrawTool.Enabled = false;
            _polyDrawTool.ReturnPolygon -= OnStepPolygon;
        }
            #endregion


        #endregion


    }
}
