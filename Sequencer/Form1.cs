using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

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
        private int _samplingTrackID = -1;  // The track ID of the track wich is sampling a color.
        private Stack<NoteComboBox> _notesComboBoxesTrack1 = new Stack<NoteComboBox>();
        private Stack<NoteComboBox> _notesComboBoxesTrack2 = new Stack<NoteComboBox>();
        //private Stack<NoteComboBox> _notesComboBoxesTrack3 = new Stack<NoteComboBox>();
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
        }

        private void Form1_Load(object sender, EventArgs e) {
            PopulateDevicesCombobox();
            LoadConfigurationFile();
            LoadFigureAreaValues();
            if (_sequencer != null) {
                _sequencer.FlipH = cb_FlipH.Checked;
                _sequencer.FlipV = cb_FlipV.Checked;
                _sequencer.Bpm = (int)numericUpDown_bpm.Value;
                _sequencer.FpsIn = (int)numericUpDown_fpsIn.Value;
                _sequencer.MainVolumen = trackBar_MainVol.Value / 100d;
                _sequencer.GetTrack(1).Volumen = trackBar_volTrack1.Value / 100d;
                _sequencer.GetTrack(2).Volumen = trackBar_volTrack2.Value / 100d;
                _sequencer.GetTrack(3).Volumen = trackBar_volTrack3.Value / 100d;
                _sequencer.StartCSound();
                numericUpDown_LengthTrack1.Maximum = _sequencer.GetTrack(1).MaxSteps;
                numericUpDown_LengthTrack2.Maximum = _sequencer.GetTrack(2).MaxSteps;
                numericUpDown_LengthTrack3.Maximum = _sequencer.GetTrack(3).MaxSteps;
            }
        }

        private void LoadConfigurationFile() {
            comboBox_cameras.SelectedIndex = Int32.Parse(ConfigurationManager.AppSettings["webcamIndex"]);
            numericUpDown_fpsIn.Value = Int32.Parse(ConfigurationManager.AppSettings["webcamFps"]);
            cb_FlipH.Checked = Boolean.Parse(ConfigurationManager.AppSettings["webcamFlipH"]);
            cb_FlipV.Checked = Boolean.Parse(ConfigurationManager.AppSettings["webcamFlipV"]);

            numericUpDown_corcheaMax.Value = Int32.Parse(ConfigurationManager.AppSettings["figureCorcheaMax"]);
            numericUpDown_corcheaMin.Value = Int32.Parse(ConfigurationManager.AppSettings["figureCorcheaMin"]);
            numericUpDown_negraMax.Value = Int32.Parse(ConfigurationManager.AppSettings["figureNegraMax"]);
            numericUpDown_negraMin.Value = Int32.Parse(ConfigurationManager.AppSettings["figureNegraMin"]);
            numericUpDown_blancaMax.Value = Int32.Parse(ConfigurationManager.AppSettings["figureBlancaMax"]);
            numericUpDown_blancaMin.Value = Int32.Parse(ConfigurationManager.AppSettings["figureBlancaMin"]);

            numericUpDown_KickMax.Value = Int32.Parse(ConfigurationManager.AppSettings["figureKickMax"]);
            numericUpDown_kickMin.Value = Int32.Parse(ConfigurationManager.AppSettings["figureKickMin"]);
            numericUpDown_SnareMax.Value = Int32.Parse(ConfigurationManager.AppSettings["figureSnareMax"]);
            numericUpDown_SnareMin.Value = Int32.Parse(ConfigurationManager.AppSettings["figureSnareMin"]);
            numericUpDown_HihatMax.Value = Int32.Parse(ConfigurationManager.AppSettings["figureHihatMax"]);
            numericUpDown_HihatMin.Value = Int32.Parse(ConfigurationManager.AppSettings["figureHihatMin"]);

            numericUpDown_bpm.Value = Int32.Parse(ConfigurationManager.AppSettings["seqBpm"]);
            comboBox_seqMode.SelectedIndex = Int32.Parse(ConfigurationManager.AppSettings["seqModeIndex"]);
            trackBar_MainVol.Value = Int32.Parse(ConfigurationManager.AppSettings["seqMainVol"]);
            textBox_boardFile.Text = ConfigurationManager.AppSettings["seqLastBoardFile"];
            comboBox_stepLengthTrack3.SelectedIndex = Int32.Parse(ConfigurationManager.AppSettings["seqTrack3StepLengthIndex"]);
            trackBar_volTrack1.Value = Int32.Parse(ConfigurationManager.AppSettings["seqTrack1Vol"]);
            trackBar_volTrack2.Value = Int32.Parse(ConfigurationManager.AppSettings["seqTrack2Vol"]);
            trackBar_volTrack3.Value = Int32.Parse(ConfigurationManager.AppSettings["seqTrack3Vol"]);

            if (_sequencer != null) {
                var notes = ConfigurationManager.AppSettings["seqTrack1Notes"].Split(',');
                var track = _sequencer.GetTrack(1) as MelodicTrack;
                if (notes.Length == track.Notes.Length) {
                    for (int i = 0; i < notes.Length; i++) {
                        track.Notes[i] = CSNoteHandler.GetPchValue(notes[i]);
                    }
                }
                notes = ConfigurationManager.AppSettings["seqTrack2Notes"].Split(',');
                track = _sequencer.GetTrack(2) as MelodicTrack;
                if (notes.Length == track.Notes.Length) {
                    for (int i = 0; i < notes.Length; i++) {
                        track.Notes[i] = CSNoteHandler.GetPchValue(notes[i]);
                    }
                }
            }

        }

        private void SaveConfigurationFileValues() {

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


            config.AppSettings.Settings["webcamIndex"].Value = comboBox_cameras.SelectedIndex.ToString();
            config.AppSettings.Settings["webcamFps"].Value = numericUpDown_fpsIn.Value.ToString();
            config.AppSettings.Settings["webcamFlipH"].Value = cb_FlipH.Checked.ToString();
            config.AppSettings.Settings["webcamFlipV"].Value = cb_FlipV.Checked.ToString();

            config.AppSettings.Settings["figureCorcheaMin"].Value = numericUpDown_corcheaMin.Value.ToString();
            config.AppSettings.Settings["figureCorcheaMax"].Value = numericUpDown_corcheaMax.Value.ToString();
            config.AppSettings.Settings["figureNegraMin"].Value = numericUpDown_negraMin.Value.ToString();
            config.AppSettings.Settings["figureNegraMax"].Value = numericUpDown_negraMax.Value.ToString();
            config.AppSettings.Settings["figureBlancaMin"].Value = numericUpDown_blancaMin.Value.ToString();
            config.AppSettings.Settings["figureBlancaMax"].Value = numericUpDown_blancaMax.Value.ToString();

            config.AppSettings.Settings["figureKickMin"].Value = numericUpDown_kickMin.Value.ToString();
            config.AppSettings.Settings["figureKickMax"].Value = numericUpDown_KickMax.Value.ToString();
            config.AppSettings.Settings["figureSnareMin"].Value = numericUpDown_SnareMin.Value.ToString();
            config.AppSettings.Settings["figureSnareMax"].Value = numericUpDown_SnareMax.Value.ToString();
            config.AppSettings.Settings["figureHihatMin"].Value = numericUpDown_HihatMin.Value.ToString();
            config.AppSettings.Settings["figureHihatMax"].Value = numericUpDown_HihatMax.Value.ToString();

            config.AppSettings.Settings["seqBpm"].Value = numericUpDown_bpm.Value.ToString();
            config.AppSettings.Settings["seqModeIndex"].Value = comboBox_seqMode.SelectedIndex.ToString();
            config.AppSettings.Settings["seqMainVol"].Value = trackBar_MainVol.Value.ToString();
            config.AppSettings.Settings["seqLastBoardFile"].Value = textBox_boardFile.Text;
            config.AppSettings.Settings["seqTrack3StepLengthIndex"].Value = comboBox_stepLengthTrack3.SelectedIndex.ToString();
            config.AppSettings.Settings["seqTrack1Vol"].Value = trackBar_volTrack1.Value.ToString();
            config.AppSettings.Settings["seqTrack2Vol"].Value = trackBar_volTrack2.Value.ToString();
            config.AppSettings.Settings["seqTrack3Vol"].Value = trackBar_volTrack3.Value.ToString();

            if (_sequencer != null) {
                var track = _sequencer.GetTrack(1) as MelodicTrack;
                string notesStr = "";
                for (int i = 0; i < track.Notes.Length; i++) {
                    notesStr += CSNoteHandler.GetNoteValue(track.Notes[i]);
                    if (i < track.Notes.Length - 1)
                        notesStr += ",";
                }
                config.AppSettings.Settings["seqTrack1Notes"].Value = notesStr;
                track = _sequencer.GetTrack(2) as MelodicTrack;
                notesStr = "";
                for (int i = 0; i < track.Notes.Length; i++) {
                    notesStr += CSNoteHandler.GetNoteValue(track.Notes[i]);
                    if (i < track.Notes.Length - 1)
                        notesStr += ",";
                }
                config.AppSettings.Settings["seqTrack2Notes"].Value = notesStr;
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            _sequencer.RawFrame -= OnImageGrabbed;
            _sequencer.Dispose();
            SaveConfigurationFileValues();
        }

        /// <summary>
        /// Loads into the static Figures clases the initial values on the GUI
        /// </summary>
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
