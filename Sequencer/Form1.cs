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
    public enum State {
        Init,
        Configuration,
            PersCalib,
            BoardSetup,
                AddSteps,
            CVSetup,
                CVColorSampleSelection,
        Idle,
            IdleInit,
            IdleCalibrated,
                IdleWithBoard,
                IdleColorSampled,
    };
    public enum Trigger {
        Initialize, Initialized,
        StartPersCalib, CancelPersCalib, FinishPersCalib,
        NewSteps,
        CancelCVEdition, FinishCVEdition,
        StartCVColorSampling,
        FinishSequencerLoading
    };


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

        private void OnStepPolygon(List<Point> i_vertices) {
            if (_stmachine.IsInState(State.AddSteps)) {
                if (i_vertices.Count > 0) {
                    _sequencer.AddStep(i_vertices);
                }
            }
        }

        private void OnSample(Image<Bgr, Byte> i_sample) {
            // TODO: Que hacer con el Sample
            if (_stmachine.State == State.CVColorSampleSelection)
                _stmachine.Fire(Trigger.FinishCVEdition);
           // DEBUGGGGG
            CvInvoke.cvShowImage("Sample", i_sample.Ptr);  // DEBUG
            CvInvoke.cvWaitKey(0);  // DEBUG
            CvInvoke.cvDestroyWindow("Sample");  // DEBUG
        }

        private void OnSampleList(IEnumerable<Image<Bgr, Byte>> i_samples) {
            // TODO: Que hacer con la list de samples
            if (_stmachine.State == State.CVColorSampleSelection)
                _stmachine.Fire(Trigger.FinishCVEdition);
            
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
                }
        }

        private void button_setColor_Click(object sender, EventArgs e) {
            if (_stmachine.IsInState(State.IdleCalibrated))
                _stmachine.Fire(Trigger.StartCVColorSampling);
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
                .Permit(Trigger.Initialized, State.IdleInit);
            // PERSPECTIVE CALIBRATION
            _stmachine.Configure(State.PersCalib)
                .SubstateOf(State.Configuration)
                .OnEntry(() => ResetPerspectiveCalibration())
                .OnExit(() => EndPerspectiveCalibration())
                .Permit(Trigger.CancelPersCalib, State.IdleInit)
                .PermitIf(Trigger.FinishPersCalib, State.IdleCalibrated, () => _sequencer.StepCount == 0)
                .PermitIf(Trigger.FinishPersCalib, State.IdleWithBoard, () => (_sequencer.StepCount > 0 && !_colorSampled))
                .PermitIf(Trigger.FinishPersCalib, State.IdleColorSampled, () => (_sequencer.StepCount > 0 && _colorSampled));
            // BOARD SETUP
            _stmachine.Configure(State.BoardSetup)
                .SubstateOf(State.Configuration)
                .Permit(Trigger.StartPersCalib, State.PersCalib);
            // ADD STEPS
            _stmachine.Configure(State.AddSteps)
                .SubstateOf(State.BoardSetup)
                .OnEntry(() => StartAddSteps())
                .OnExit(() => StopAddSteps())
                .Permit(Trigger.CancelCVEdition, State.IdleCalibrated)
                .PermitIf(Trigger.FinishCVEdition, State.IdleCalibrated, () => _sequencer.StepCount == 0)
                .PermitIf(Trigger.FinishCVEdition, State.IdleWithBoard, () => (_sequencer.StepCount > 0 && !_colorSampled))
                .PermitIf(Trigger.FinishCVEdition, State.IdleColorSampled, () => (_sequencer.StepCount > 0 && _colorSampled));
            // CV COLOR SAMPLE SELECTION
            _stmachine.Configure(State.CVColorSampleSelection)
                .SubstateOf(State.CVSetup)
                .OnEntry(() => StartColorSampling())
                .OnExit(() => StopColorSampling())
                .Permit(Trigger.FinishCVEdition, State.IdleColorSampled);
            // IDLE
            _stmachine.Configure(State.Idle)
                .PermitIf(Trigger.StartPersCalib, State.PersCalib, () => IsInitialized())
                .PermitIf(Trigger.NewSteps, State.AddSteps, () => _sequencer.PerspectiveCalibrated);
            // IDLE INIT
            _stmachine.Configure(State.IdleInit)
                .SubstateOf(State.Idle)
                .OnEntry(() => EnterIdleInit());
            // IDLE CALIBRATED
            _stmachine.Configure(State.IdleCalibrated)
                .SubstateOf(State.Idle)
                .OnEntry(() => EnterIdleCalibrated())
                .OnExit(() => ExitIdleCalibrated())
                .Permit(Trigger.FinishSequencerLoading, State.IdleWithBoard)
                .Permit(Trigger.StartCVColorSampling, State.CVColorSampleSelection);
            // IDLE WITH BOARD
            _stmachine.Configure(State.IdleWithBoard)
                .SubstateOf(State.IdleCalibrated)  // Aquí ya estamos mostrando la imagen con la perspectiva corregida
                .OnEntry(() => EnterIdleWithBoard());
            // IDLE COLOR SAMPLED
            _stmachine.Configure(State.IdleColorSampled)
                .SubstateOf(State.IdleCalibrated)
                .OnEntry(() => EnterIdleColorSampled());
        }

            #region Init State
        private void Initialize() {
            if (_sequencer != null) {
                _sequencer.Camera.ImageGrabbed += OnImageGrabbed;
                _stmachine.Fire(Trigger.Initialized);
            }
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
            * del calibrado (y no sólo el estado de la máquina, sino
            * todo lo que ello conlleva).
            * */
            _sequencer.ResetPerspectiveCalibration();  // Dentro se hace: Camera.ImageGrabbed -= CalibrateIncomingFrame
            _sequencer.Camera.ImageGrabbed += OnImageGrabbed;
            // Polygon handling
            _polyDrawTool.Enabled = true;
            _polyDrawTool.ReturnPolygon += OnPerspetiveCalibrationPolygon;
            // Board Handling
            _sequencer.DrawSteps = false;
            // GUI
            button_setPersCalib.Text = "Draw 4 points";
            button_setPersCalib.Enabled = false;
            button_addSteps.Enabled = false;
            button_loadBoard.Enabled = false;
            button_saveBoard.Enabled = false;
            toolStripStatusLabel_state.Text = "State: Perspective calibration";  // DEBUG ?
        }

        private void EndPerspectiveCalibration() {
            if (_sequencer.PerspectiveCalibrated)
                _sequencer.Camera.ImageGrabbed -= OnImageGrabbed;
            // GUI
            _polyDrawTool.ReturnPolygon -= OnPerspetiveCalibrationPolygon;
            _polyDrawTool.Enabled = false;
            button_setPersCalib.Text = "Set perspective correction rectangle";
            button_setPersCalib.Enabled = true;
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
            button_addSteps.Text = "Save steps";
            button_addSteps.Enabled = true;  // El estado padre 'IdleCalibrated', lo desactiva al entrar
            toolStripStatusLabel_state.Text = "State: Add Steps";  // DEBUG ?
        }

        private void StopAddSteps() {
            // Polygon handling
            _polyDrawTool.Enabled = false;
            _polyDrawTool.ReturnPolygon -= OnStepPolygon;
            // GUI
            button_addSteps.Text = "Add steps";
        }
            #endregion

            #region Color Sample Selection State
        private void StartColorSampling() {
            _colorSampled = false;
            _sequencer.DrawSteps = false;
            _selectionRect.Enabled = true;
            _selectionRect.AcquiredSample += OnSample;
            _selectionRect.AcquiredSampleList += OnSampleList;
            // GUI
            button_setColor.Text = "Sampling";
            toolStripStatusLabel_state.Text = "State: Color Sample Selection";  // DEBUG ?
        }
        private void StopColorSampling() {
            _colorSampled = true;
            _selectionRect.Enabled = false;
            _selectionRect.AcquiredSample -= OnSample;
            _selectionRect.AcquiredSampleList -= OnSampleList;
            // GUI
            button_setColor.Text = "Set color";
        }
            #endregion

        #region Idle States

        private void EnterIdleInit() {
            _sequencer.Camera.ImageGrabbed += OnImageGrabbed;
            _sequencer.DrawSteps = false;
            // GUI
            button_setPersCalib.Enabled = true;
            button_loadBoard.Enabled = false;
            button_saveBoard.Enabled = false;
            toolStripStatusLabel_state.Text = "State: Idle Init";  // DEBUG ?
        }

        private void EnterIdleCalibrated() {
            // GUI
            button_setPersCalib.Enabled = true;
            button_addSteps.Enabled = true;
            button_loadBoard.Enabled = true;
            if (_sequencer.StepCount > 0)
                button_saveBoard.Enabled = true;
            button_setColor.Enabled = true;
            toolStripStatusLabel_state.Text = "State: Idle Calibrated";  // DEBUG ?
        }

        private void ExitIdleCalibrated() {
            // GUI
            button_setPersCalib.Enabled = false;
            button_addSteps.Enabled = false;
            button_loadBoard.Enabled = false;
            button_saveBoard.Enabled = false;
            button_setColor.Enabled = false;
        }

        private void EnterIdleWithBoard() {
            _sequencer.DrawSteps = true;
            // GUI
            toolStripStatusLabel_state.Text = "State: Idle with Board";  // DEBUG ?
        }

        private void EnterIdleColorSampled() {
            _sequencer.DrawSteps = true;
            // GUI
            toolStripStatusLabel_state.Text = "State: Idle Color Sampled";  // DEBUG ?
        }
            #endregion


        #endregion


    }
}
