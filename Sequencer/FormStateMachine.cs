using System.Windows.Forms;

namespace Sequencer {

    public enum State {
        Init,
        Configuration,
            PersCalib,
            BoardSetup,
                AddSteps,
                ClearSteps,
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
        NewSteps, ClearSteps,
        CancelCVEdition, FinishCVEdition,
        StartCVColorSampling,
        FinishSequencerLoading
    };


    public partial class Form1 : Form {
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
            // CLEAR STEPS
            _stmachine.Configure(State.ClearSteps)
                .SubstateOf(State.BoardSetup)
                .OnEntry(() => EnterClearSteps())
                .PermitIf(Trigger.ClearSteps, State.IdleCalibrated, () => !_colorSampled)
                .PermitIf(Trigger.ClearSteps, State.IdleColorSampled, () => _colorSampled);
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
                .Permit(Trigger.StartCVColorSampling, State.CVColorSampleSelection)
                .Permit(Trigger.ClearSteps, State.ClearSteps);
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
            _sequencer.PerspectiveCorrectedFrame -= OnPerspectiveCorrectedFrame;
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
            button_clearSteps.Enabled = false;
            button_loadBoard.Enabled = false;
            button_saveBoard.Enabled = false;
            toolStripStatusLabel_state.Text = "State: Perspective calibration";  // DEBUG ?
        }

        private void EndPerspectiveCalibration() {
            if (_sequencer.PerspectiveCalibrated) {
                _sequencer.Camera.ImageGrabbed -= OnImageGrabbed;
                _sequencer.PerspectiveCorrectedFrame += OnPerspectiveCorrectedFrame;
            }
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

        #region ClearSteps  State
        private void EnterClearSteps() {
            _sequencer.ClearSteps();
            _stmachine.Fire(Trigger.ClearSteps);
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
            _sequencer.PerspectiveCorrectedFrame -= OnPerspectiveCorrectedFrame;
            _sequencer.DrawSteps = false;
            // GUI
            button_setPersCalib.Enabled = true;
            button_loadBoard.Enabled = false;
            button_saveBoard.Enabled = false;
            button_clearSteps.Enabled = false;
            toolStripStatusLabel_state.Text = "State: Idle Init";  // DEBUG ?
        }

        private void EnterIdleCalibrated() {
            // GUI
            button_setPersCalib.Enabled = true;
            button_addSteps.Enabled = true;
            button_loadBoard.Enabled = true;
            if (_sequencer.StepCount > 0) {
                button_saveBoard.Enabled = true;
                button_clearSteps.Enabled = true;
            }
            button_setColor.Enabled = true;
            toolStripStatusLabel_state.Text = "State: Idle Calibrated";  // DEBUG ?
        }

        private void ExitIdleCalibrated() {
            // GUI
            button_setPersCalib.Enabled = false;
            button_addSteps.Enabled = false;
            button_clearSteps.Enabled = false;
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
    }
}