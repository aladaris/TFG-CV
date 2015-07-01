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

using Aladaris;
using VisualTools;

namespace Sequencer {

    public partial class Form1 : Form {
        #region Atributes
        private PolygonDrawingTool _polyDrawTool;
        private SelectionRectangle _selectionRect;
        private Sequencer _sequencer;
        //private StateMachine<State, Trigger> _stmachine = new StateMachine<State, Trigger>(State.Init);
        private bool _colorSampled = false;  // Indica si se ha seleccionado la muestra necesaria para el filtrado de color
        //private bool _gettingFigureBlobs = false;  // TODO: Mover a donde se mueva lka lógica de la detección de blobs
        private int _samplingTrackID = -1;  // The track ID of the track wich is sampling a color.
        private Stack<NoteComboBox> _notesComboBoxesTrack1 = new Stack<NoteComboBox>();
        private Stack<NoteComboBox> _notesComboBoxesTrack2 = new Stack<NoteComboBox>();
        private Stack<NoteComboBox> _notesComboBoxesTrack3 = new Stack<NoteComboBox>();
        #endregion

        #region Form Manage
        public Form1() {
            InitializeComponent();
            _sequencer = new Sequencer(imageBox_mainDisplay, 3);
            _sequencer.Tracks[0] = new MelodicTrack(1, _sequencer.CSound, 10, 11, 12);
            _sequencer.Tracks[1] = new MelodicTrack(2, _sequencer.CSound, 20, 21, 22);
            _sequencer.Tracks[2] = new RitmicTrack (3, _sequencer.CSound, 30, 31, 32);
            //_sequencer.TrackStepChange += OnTrackStepChange;

            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
            _selectionRect = new SelectionRectangle(imageBox_mainDisplay);
            numericUpDown_fpsIn.Value = _sequencer.FpsIn;
        }

        private void Form1_Load(object sender, EventArgs e) {
            PopulateDevicesCombobox();
            comboBox_seqMode.SelectedIndex = 0;
            comboBox_stepLengthTrack3.SelectedIndex = 1;

            if (_sequencer != null) {
                cb_FlipH.Checked = _sequencer.FlipH;
                cb_FlipV.Checked = _sequencer.FlipV;
                numericUpDown_LengthTrack1.Maximum = _sequencer.GetTrack(1).MaxSteps;
                numericUpDown_LengthTrack2.Maximum = _sequencer.GetTrack(2).MaxSteps;
                numericUpDown_LengthTrack3.Maximum = _sequencer.GetTrack(3).MaxSteps;

                _sequencer.Bpm = (int)numericUpDown_bpm.Value;
                /*
                numericUpDown_LengthTrack1.Value = 8;
                numericUpDown_LengthTrack2.Value = 8;
                numericUpDown_LengthTrack3.Value = 8;
                _sequencer.GetTrack(1).Length = (int)numericUpDown_LengthTrack1.Value;
                _sequencer.GetTrack(2).Length = (int)numericUpDown_LengthTrack2.Value;
                _sequencer.GetTrack(3).Length = (int)numericUpDown_LengthTrack3.Value;
                */
                _sequencer.MainVolumen = trackBar_MainVol.Value / 100d;
                _sequencer.GetTrack(1).Volumen = trackBar_volTrack1.Value / 100d;
                _sequencer.GetTrack(2).Volumen = trackBar_volTrack2.Value / 100d;
                _sequencer.GetTrack(3).Volumen = trackBar_volTrack3.Value / 100d;
                _sequencer.StartCSound();
            }

            //InitStateMachine();
            LoadFigureAreaValues();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            _sequencer.RawFrame -= OnImageGrabbed;
            _sequencer.Dispose();
        }

        private void LoadFigureAreaValues() {
            MelodicFigures.Corchea.MaxArea = (int)numericUpDown_corcheaMax.Value;
            MelodicFigures.Corchea.MinArea = (int)numericUpDown_corcheaMin.Value;
            MelodicFigures.Negra.MaxArea = (int)numericUpDown_negraMax.Value;
            MelodicFigures.Negra.MinArea = (int)numericUpDown_negraMin.Value;
            MelodicFigures.Blanca.MaxArea = (int)numericUpDown_blancaMax.Value;
            MelodicFigures.Blanca.MinArea = (int)numericUpDown_blancaMin.Value;

            RitmicFigures.Kick.MaxArea = (int)numericUpDown_KickMax.Value;
            RitmicFigures.Kick.MinArea = (int)numericUpDown_kickMin.Value;
            RitmicFigures.Snare.MaxArea = (int)numericUpDown_SnareMax.Value;
            RitmicFigures.Snare.MinArea = (int)numericUpDown_SnareMin.Value;
            RitmicFigures.Hihat.MaxArea = (int)numericUpDown_HihatMax.Value;
            RitmicFigures.Hihat.MinArea = (int)numericUpDown_HihatMin.Value;

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
    }
}
