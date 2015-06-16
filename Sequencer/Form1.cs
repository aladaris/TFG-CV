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
        private int _samplingTrackID = -1;  // The track ID of the track wich is sampling a color.
        #endregion

        #region Form Manage
        public Form1() {
            InitializeComponent();
            _sequencer = new Sequencer(imageBox_mainDisplay, 1);
            _sequencer.Tracks[0] = new Track(1, _sequencer.CSound, 10, 11, 12);
            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
            _selectionRect = new SelectionRectangle(imageBox_mainDisplay);
            numericUpDown_fpsIn.Value = _sequencer.FpsIn;
        }

        private void Form1_Load(object sender, EventArgs e) {
            toolStripStatusLabel_state.Text = "State: Init";  // DEBUG ?
            if (_sequencer != null) {
                cb_FlipH.Checked = _sequencer.FlipH;
                cb_FlipV.Checked = _sequencer.FlipV;
            }

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
            if (i_frame != null) {
                if (!_sequencer.PerspectiveCalibrated) {
                    Size s = imageBox_mainDisplay.Size;
                    imageBox_mainDisplay.Image = i_frame.Resize(s.Width, s.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                } else {
                    _sequencer.RawFrame -= OnImageGrabbed;
                    _sequencer.PerspectiveCorrectedFrame += OnPerspectiveCorrectedFrame;
                }
            }
        }

        private void OnPerspectiveCorrectedFrame(Image<Bgr, Byte> i_img, EventArgs e) {
            if (i_img != null) {
                if (_sequencer.PerspectiveCalibrated) {
                    imageBox_mainDisplay.Image = i_img;
                } else {
                    _sequencer.PerspectiveCorrectedFrame -= OnPerspectiveCorrectedFrame;
                }
            }
        }

        /// <summary>
        /// Triggered when the user creates a polygon during the perspective correction calibration
        /// </summary>
        /// <param name="i_corners">The four corners of the polygon returned by a PolygonDrawingTool</param>
        private void OnPerspetiveCalibrationPolygon(List<Point> i_corners) {
            if (i_corners.Count == 4) {
                _sequencer.SetPerspectiveCalibration(i_corners);
                if (_sequencer.PerspectiveCalibrated) {
                    _polyDrawTool.Enabled = false;
                    _polyDrawTool.ReturnPolygon -= OnPerspetiveCalibrationPolygon;
                    _sequencer.RawFrame -= OnImageGrabbed;
                    _sequencer.PerspectiveCorrectedFrame += OnPerspectiveCorrectedFrame;
                    // GUI
                    button_setPersCalib.Text = "Set perspective correction rectangle";
                }
            } else {
                MessageBox.Show("We need exactly four (4) Points for the calibration", "INFO");
            }
        }

        private void OnStepPolygon(List<Point> i_vertices) {
            if (i_vertices.Count > 0) {
                _sequencer.AddStep(i_vertices);
            }
        }

        private void OnSample(Image<Bgr, Byte> i_sample) {
            if (_samplingTrackID >= 0) {
                _sequencer.SetFilterColor(i_sample, _samplingTrackID);
            }
            StopColorSampling();
        }

        private void OnSampleList(IEnumerable<Image<Bgr, Byte>> i_samples) {
            if (_samplingTrackID >= 0) {
                _sequencer.SetFilterColor(i_samples, _samplingTrackID);
            }
            StopColorSampling();
            
        }
        #endregion


        private void StopColorSampling() {
            _selectionRect.Enabled = false;
            _selectionRect.AcquiredSample -= OnSample;
            _selectionRect.AcquiredSampleList -= OnSampleList;
            Button b = null;
            Panel p = null;
            // TODO: Este switch no es nada flexible
            switch (_samplingTrackID) {
                case 1:
                    b = button_setColor_Track1;
                    p = p_colorPreview_Track1;
                    break;
                case 2:
                    b = button_setColor_Track2;
                    p = p_colorPreview_Track2;
                    break;
                case 3:
                    b = button_setColor_Track3;
                    p = p_colorPreview_Track3;
                    break;
            }
            if (b != null)
                b.Text = "Set color";
            if (p != null)
                p.BackColor = _sequencer.GetTrack(_samplingTrackID).ColorFilter.SampleMeanColor;
            _samplingTrackID = -1;
            if (_sequencer.StepCount > 0) {
                _sequencer.DrawSteps = true;
            }
        }

        #region GUI Handlers
        /// <summary>
        /// This method handles the startup and stopping of the capture thread.
        /// </summary>
        private void button_startCamera_Click(object sender, EventArgs e) {
            if (!_sequencer.Capturing) {
                _sequencer.StartCapture();
                if (_sequencer.PerspectiveCalibrated) {
                    _sequencer.PerspectiveCorrectedFrame += OnPerspectiveCorrectedFrame;
                } else {
                    _sequencer.RawFrame += OnImageGrabbed;
                }
                // GUI
                button_startCamera.Text = "Stop Camera";
            } else {
                _sequencer.StopCapture();
                _sequencer.RawFrame -= OnImageGrabbed;
                if (_sequencer.PerspectiveCalibrated) {
                    _sequencer.PerspectiveCorrectedFrame -= OnPerspectiveCorrectedFrame;
                }
                // GUI
                button_startCamera.Text = "Start Camera";
            }
        }

        /// <summary>
        /// Starts the perspective correction calibration.
        /// </summary>
        private void button_setPersCalib_Click(object sender, EventArgs e) {
            if (_sequencer.PerspectiveCalibrated){
                _sequencer.ResetPerspectiveCalibration();
                _sequencer.RawFrame += OnImageGrabbed;
            }
            _sequencer.PerspectiveCorrectedFrame -= OnPerspectiveCorrectedFrame;
            // Polygon handling
            _polyDrawTool.Enabled = true;
            _polyDrawTool.ReturnPolygon += OnPerspetiveCalibrationPolygon;
            // Board Handling
            _sequencer.DrawSteps = false;
            // GUI
            button_setPersCalib.Text = "Draw 4 points";
        }

        private void button_addSteps_Click(object sender, EventArgs e) {
            if (!_polyDrawTool.Enabled) {
                // Polygon handling
                _polyDrawTool.Enabled = true;
                _polyDrawTool.ReturnPolygon += OnStepPolygon;
                // Board Handling
                _sequencer.DrawSteps = true;
                // GUI
                button_addSteps.Text = "Stop Adding";
            } else {
                // Polygon handling
                _polyDrawTool.Enabled = false;
                _polyDrawTool.ReturnPolygon -= OnStepPolygon;
                // Board Handling
                if (_sequencer.StepCount <= 0)
                    _sequencer.DrawSteps = false;
                // GUI
                button_addSteps.Text = "Add steps";
            }
        }

        private void button_clearSteps_Click(object sender, EventArgs e) {
            if (_sequencer != null) {
                _sequencer.ClearSteps();
                _sequencer.DrawSteps = false;
            }
        }

        private void button_saveSteps_Click(object sender, EventArgs e) {
            if (_sequencer != null)
                _sequencer.Save();
        }

        private void button_loadSteps_Click(object sender, EventArgs e) {
            if (_sequencer != null) {
                _sequencer.Load();
                _sequencer.DrawSteps = true;
            }
        }

        private void button_setColor_Click(object sender, EventArgs e) {
            var b = sender as Button;
            if (b != null) {
                int trackId = 0;
                bool parsed = Int32.TryParse(b.Name[b.Name.Length - 1].ToString(), out trackId);
                if (parsed) {
                    _samplingTrackID = trackId;
                    _selectionRect.Enabled = true;
                    _selectionRect.AcquiredSample += OnSample;
                    _selectionRect.AcquiredSampleList += OnSampleList;
                    _sequencer.DrawSteps = false;
                    b.Text = "Sampling";
                }
            }
        }

        private void numericUpDown_fpsIn_ValueChanged(object sender, EventArgs e) {
            _sequencer.FpsIn = (int)numericUpDown_fpsIn.Value;
        }

        private void button_StartInstrument_Click(object sender, EventArgs e) {
            if ((_sequencer.PerspectiveCalibrated)&&(_sequencer.StepCount > 0)&&(!_sequencer.CSoundRunning))
                _sequencer.StartCSound();
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

        private void cb_FlipH_CheckedChanged(object sender, EventArgs e) {
            if (_sequencer != null)
                _sequencer.FlipH = cb_FlipH.Checked;
        }

        private void cb_FlipV_CheckedChanged(object sender, EventArgs e) {
            if (_sequencer != null)
                _sequencer.FlipV = cb_FlipV.Checked;
        }

    }
}
