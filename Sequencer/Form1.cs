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
        private bool _gettingFigureBlobs = false;  // TODO: Mover a donde se mueva lka lógica de la detección de blobs
        #endregion

        #region Form Manage
        public Form1() {
            InitializeComponent();
            _sequencer = new Sequencer(imageBox_mainDisplay);
            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
            _selectionRect = new SelectionRectangle(imageBox_mainDisplay);
            _sequencer.ColorFilteredFrame += OnColorFilteredFrame;  // TODO: Mover de aquí una vez se definan los estados de la STMachine
            numericUpDown_fpsIn.Value = _sequencer.FpsIn;
        }

        private void Form1_Load(object sender, EventArgs e) {
            toolStripStatusLabel_state.Text = "State: Init";  // DEBUG ?

            InitStateMachine();
            LoadFigureAreaValues();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            _sequencer.RawFrame -= OnImageGrabbed;
            _sequencer.Dispose();
        }

        private void LoadFigureAreaValues() {
            Figures.Corchea.MaxArea = (int)numericUpDown_corcheaMax.Value;
            Figures.Corchea.MinArea = (int)numericUpDown_corcheaMin.Value;
            Figures.Negra.MaxArea = (int)numericUpDown_negraMax.Value;
            Figures.Negra.MinArea = (int)numericUpDown_negraMin.Value;
            Figures.Blanca.MaxArea = (int)numericUpDown_blancaMax.Value;
            Figures.Blanca.MinArea = (int)numericUpDown_blancaMin.Value;
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// This method renders the "RAW" image retrieved from the sequencer.
        /// </summary>
        /// <param name="i_frame">The RAW frame retrieved from the capture device</param>
        /// <param name="e"></param>
        public void OnImageGrabbed(Image<Bgr, Byte> i_frame, EventArgs e) {
            if ((_stmachine.IsInState(State.Configuration)) || (_stmachine.IsInState(State.Init)) || (_stmachine.State == State.IdleInit)) {
                Size s = imageBox_mainDisplay.Size;
                imageBox_mainDisplay.Image = i_frame.Resize(s.Width, s.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            } else
                _sequencer.RawFrame -= OnImageGrabbed; /* Sino estamos en el estado correcto, nos damos de baja del evento de la cámara;
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
            if (!_gettingFigureBlobs) {
                _gettingFigureBlobs = true;
                //CvInvoke.cvSmooth(i_img.Ptr, i_img.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, 13, 13, 1.5, 1);
                Image<Gray, Byte> gray = i_img.Convert<Gray, Byte>();//.PyrDown().PyrUp();
                CvInvoke.cvSmooth(gray.Ptr, gray.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, 13, 13, 1.5, 1);
                Gray grayLow = new Gray(1);
                Gray grayHigh = new Gray(255);
                Image<Gray, Byte> thresholded = gray.ThresholdBinary(grayLow, grayHigh);
                List<CVFigureBlob> blobs = new List<CVFigureBlob>();
                using (MemStorage storage = new MemStorage()) {
                    for (Contour<Point> contours = thresholded.Erode(1).FindContours(); contours != null; contours = contours.HNext) {
                        Contour<Point> currContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);
                        Figure fig = Figures.GetFigure((int)currContour.Area);
                        if (fig != null) {
                            blobs.Add(new CVFigureBlob(currContour.BoundingRectangle, fig, imageBox_mainDisplay));
                            blobs[blobs.Count - 1].Paint();
                        }
                        //CVFigureBlob blob = new CVFigureBlob(currContour.BoundingRectangle, currContour.Area, imageBox_mainDisplay);
                        //thresholded.Draw(currContour, grayLow, 1);
                    }
                }

                // GUI
                imageBox_preview.Image = thresholded;//.Resize(imageBox_preview.Size.Width, imageBox_preview.Size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN);
                _gettingFigureBlobs = false;
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

        private void numericUpDown_fpsIn_ValueChanged(object sender, EventArgs e) {
            _sequencer.FpsIn = (int)numericUpDown_fpsIn.Value;
        }
        #endregion


        // TODO: Eliminar todo este código duplicado del infierno
        private void numericUpDown_corcheaMin_ValueChanged(object sender, EventArgs e) {
            Figures.Corchea.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = Figures.Corchea.MinArea;
        }

        private void numericUpDown_corcheaMax_ValueChanged(object sender, EventArgs e) {
            Figures.Corchea.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = Figures.Corchea.MaxArea;
        }

        private void numericUpDown_negraMin_ValueChanged(object sender, EventArgs e) {
            Figures.Negra.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = Figures.Negra.MinArea;
        }

        private void numericUpDown_negraMax_ValueChanged(object sender, EventArgs e) {
            Figures.Negra.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = Figures.Negra.MaxArea;
        }

        private void numericUpDown_blancaMin_ValueChanged(object sender, EventArgs e) {
            Figures.Blanca.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = Figures.Blanca.MinArea;
        }

        private void numericUpDown_blancaMax_ValueChanged(object sender, EventArgs e) {
            Figures.Blanca.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = Figures.Blanca.MaxArea;
        }


    }
}
