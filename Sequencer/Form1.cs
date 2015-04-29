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
using Aladaris;
using VisualTools;

namespace Sequencer {

    public partial class Form1 : Form {
        #region Atributes
        private PolygonDrawingTool _polyDrawTool;
        private SelectionRectangle _selectionRect;
        private Sequencer _sequencer;
        private StateMachine<State, Trigger> _stmachine = new StateMachine<State, Trigger>(State.Init);
        private bool _colorSampled = false;  // Indica si se ha seleccionado la muestra necesaria para el filtrado de color
        #endregion

        #region Form Manage
        public Form1() {
            InitializeComponent();
            _sequencer = new Sequencer(imageBox_mainDisplay);
            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
            _selectionRect = new SelectionRectangle(imageBox_mainDisplay);
            _sequencer.ColorFilteredFrame += OnColorFilteredFrame;  // TODO: Mover de aquí una vez se definan los estados de la STMachine
        }

        private void Form1_Load(object sender, EventArgs e) {
            toolStripStatusLabel_state.Text = "State: Init";  // DEBUG ?

            InitStateMachine();
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
            if ((_stmachine.IsInState(State.Configuration)) || (_stmachine.IsInState(State.Init)) || (_stmachine.State == State.IdleInit)) {
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
                    _stmachine.Fire(Trigger.FinishPersCalib);
                } else {
                    MessageBox.Show("We need exactly four (4) Points for the calibration", "INFO");
                }
            }
        }

        private void OnPerspectiveCorrectedFrame(Image<Bgr, Byte> i_img, EventArgs e) {
            if (i_img != null)
                imageBox_mainDisplay.Image = i_img;
        }

        private void OnStepPolygon(List<Point> i_vertices) {
            if (_stmachine.IsInState(State.AddSteps)) {
                if (i_vertices.Count > 0) {
                    _sequencer.AddStep(i_vertices);
                }
            }
        }

        private void OnSample(Image<Bgr, Byte> i_sample) {
            if (_stmachine.State == State.CVColorSampleSelection)
                _stmachine.Fire(Trigger.FinishCVEdition);
            _sequencer.SetFilterColor(i_sample);
           // DEBUGGGGG
            //CvInvoke.cvShowImage("Sample", i_sample.Ptr);  // DEBUG
            //CvInvoke.cvWaitKey(0);  // DEBUG
            //CvInvoke.cvDestroyWindow("Sample");  // DEBUG
        }

        private void OnSampleList(IEnumerable<Image<Bgr, Byte>> i_samples) {
            if (_stmachine.State == State.CVColorSampleSelection)
                _stmachine.Fire(Trigger.FinishCVEdition);
            _sequencer.SetFilterColor(i_samples);
            
        }

        private void OnColorFilteredFrame(Image<Gray, Double> i_img, EventArgs e) {
            // TODO: Todo lo de los estados
            imageBox_preview.Image = i_img.Resize(imageBox_preview.Size.Width, imageBox_preview.Size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN);
        }
        #endregion

        #region GUI Handlers
        /// <summary>
        /// This method handles the startup and stopping of the capture thread.
        /// </summary>
        private void button_startCamera_Click(object sender, EventArgs e) {
            if (!_sequencer.Capturing) {
                _sequencer.StartCapture();
                if (_stmachine.IsInState(State.Init))
                    _stmachine.Fire(Trigger.Initialize);
                // GUI
                button_startCamera.Text = "Stop Camera";
            } else {
                _sequencer.StopCapture();
                button_startCamera.Text = "Start Camera";
            }
        }

        /// <summary>
        /// Starts the perspective correction calibration.
        /// </summary>
        private void button_setPersCalib_Click(object sender, EventArgs e) {
            if (_stmachine.IsInState(State.Idle)) {
                // Pasar de un Idle a PersCalib
                _stmachine.Fire(Trigger.StartPersCalib);
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
                _stmachine.Fire(Trigger.FinishCVEdition);
        }

        private void button_addSteps_Click(object sender, EventArgs e) {
            if (_stmachine.IsInState(State.IdleCalibrated)) {
                _stmachine.Fire(Trigger.NewSteps);
            } else if (_stmachine.State == State.AddSteps) {
                _stmachine.Fire(Trigger.FinishCVEdition);
            }
        }

        private void button_clearSteps_Click(object sender, EventArgs e) {
            if (_sequencer != null) {
                if (_stmachine.IsInState(State.IdleCalibrated)) {
                    _stmachine.Fire(Trigger.ClearSteps);
                }
            }
        }

        private void button_saveSteps_Click(object sender, EventArgs e) {
            if (_sequencer != null)
                if (_stmachine.IsInState(State.IdleWithBoard))
                    _sequencer.Save();
        }

        private void button_loadSteps_Click(object sender, EventArgs e) {
            if (_sequencer != null)
                if (_stmachine.IsInState(State.IdleCalibrated)) {
                    _sequencer.Load();
                    _stmachine.Fire(Trigger.FinishSequencerLoading);
                    button_clearSteps.Enabled = true;
                }
        }

        private void button_setColor_Click(object sender, EventArgs e) {
            if (_stmachine.IsInState(State.IdleCalibrated))
                _stmachine.Fire(Trigger.StartCVColorSampling);
        }
        #endregion

    }
}
