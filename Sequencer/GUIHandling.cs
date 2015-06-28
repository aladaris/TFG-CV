using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sequencer {

    public delegate void NoteComboBoxValueChangeHandler(int tId, int sId, string value);

    public class NoteComboBox : IDisposable {
        private readonly string[] notes_list;

        public ComboBox ComboBox { get; set; }
        public int TrackId { get; private set; }
        public int StepId { get; private set; }
        public NoteComboBoxValueChangeHandler NoteValueChange;
        
        public NoteComboBox(int tId, int sId) {
            notes_list = CSNoteHandler.ListOfNotes;
            TrackId = tId;
            StepId = sId;
            ComboBox = new ComboBox();
            ComboBox.Size = new Size(45, ComboBox.Size.Height);
            ComboBox.Items.AddRange(notes_list);
            ComboBox.AutoCompleteMode = AutoCompleteMode.Append;
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.SelectedValueChanged += OnComboBoxValueChange;
        }

        public void Dispose() {
            if (ComboBox != null) {
                ComboBox.Dispose();
            }
        }

        private void OnComboBoxValueChange(object sender, EventArgs e) {
            if (NoteValueChange != null)
                NoteValueChange(TrackId, StepId, ComboBox.Text);
        }
    }

    public partial class Form1 : Form {
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
            if (_sequencer.PerspectiveCalibrated) {
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
            //if ((_sequencer.PerspectiveCalibrated) && (_sequencer.StepCount > 0) && (!_sequencer.CSoundRunning))
            //    _sequencer.StartCSound();
        }

        // TODO: Eliminar todo este código duplicado del infierno
        private void numericUpDown_corcheaMin_ValueChanged(object sender, EventArgs e) {
            MelodicFigures.Corchea.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = MelodicFigures.Corchea.MinArea;
        }

        private void numericUpDown_corcheaMax_ValueChanged(object sender, EventArgs e) {
            MelodicFigures.Corchea.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = MelodicFigures.Corchea.MaxArea;
        }

        private void numericUpDown_negraMin_ValueChanged(object sender, EventArgs e) {
            MelodicFigures.Negra.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = MelodicFigures.Negra.MinArea;
        }

        private void numericUpDown_negraMax_ValueChanged(object sender, EventArgs e) {
            MelodicFigures.Negra.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = MelodicFigures.Negra.MaxArea;
        }

        private void numericUpDown_blancaMin_ValueChanged(object sender, EventArgs e) {
            MelodicFigures.Blanca.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = MelodicFigures.Blanca.MinArea;
        }

        private void numericUpDown_blancaMax_ValueChanged(object sender, EventArgs e) {
            MelodicFigures.Blanca.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = MelodicFigures.Blanca.MaxArea;
        }

        private void numericUpDown_kickMin_ValueChanged(object sender, EventArgs e) {
            RitmicFigures.Kick.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = RitmicFigures.Kick.MinArea;
        }

        private void numericUpDown_KickMax_ValueChanged(object sender, EventArgs e) {
            RitmicFigures.Kick.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = RitmicFigures.Kick.MaxArea;
        }

        private void numericUpDown_SnareMin_ValueChanged(object sender, EventArgs e) {
            RitmicFigures.Snare.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = RitmicFigures.Snare.MinArea;
        }

        private void numericUpDown_SnareMax_ValueChanged(object sender, EventArgs e) {
            RitmicFigures.Snare.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = RitmicFigures.Snare.MaxArea;
        }

        private void numericUpDown_HihatMin_ValueChanged(object sender, EventArgs e) {
            RitmicFigures.Hihat.MinArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = RitmicFigures.Hihat.MinArea;
        }

        private void numericUpDown_HihatMax_ValueChanged(object sender, EventArgs e) {
            RitmicFigures.Hihat.MaxArea = (int)(((NumericUpDown)sender).Value);
            ((NumericUpDown)sender).Value = RitmicFigures.Hihat.MaxArea;
        }

        private void cb_FlipH_CheckedChanged(object sender, EventArgs e) {
            if (_sequencer != null)
                _sequencer.FlipH = cb_FlipH.Checked;
        }

        private void cb_FlipV_CheckedChanged(object sender, EventArgs e) {
            if (_sequencer != null)
                _sequencer.FlipV = cb_FlipV.Checked;
        }

        private void numericUpDown_bpm_ValueChanged(object sender, EventArgs e) {
            if (_sequencer != null) {
                _sequencer.Bpm = (int)numericUpDown_bpm.Value;
            }
        }

        #endregion

        private void tabControl_bottom_SelectedIndexChanged(object sender, EventArgs e) {
            var tb = sender as TabControl;
            if (tb != null) {
                tabControl_Modes.SelectedIndex = tb.SelectedIndex;
            }
        }
        private void tabControl_Modes_SelectedIndexChanged(object sender, EventArgs e) {
            var tb = sender as TabControl;
            if (tb != null) {
                tabControl_bottom.SelectedIndex = tb.SelectedIndex;
            }
        }
        private void checkBox_playPause_CheckedChanged(object sender, EventArgs e) {
            if (_sequencer != null) {
                _sequencer.IsPlaying = checkBox_playPause.Checked;
                checkBox_playPause.Text = _sequencer.IsPlaying ? "Pause" : "Play";
            }
        }
        private void comboBox_stepLengthTrack3_SelectedIndexChanged(object sender, EventArgs e) {
            if (_sequencer != null) {
                var cb = sender as ComboBox;
                if (cb != null){
                    var rt = _sequencer.GetTrack(3) as RitmicTrack;
                    switch (cb.SelectedIndex) {
                        case 0:  // Corchea
                            if (rt != null) {
                                rt.StepDuration = 1;
                            }
                            break;
                        case 1:  // Negra
                            if (rt != null) {
                                rt.StepDuration = 2;
                            }
                            break;
                        case 2:  // Blanca
                            if (rt != null) {
                                rt.StepDuration = 4;
                            }
                            break;
                    }
                }
            }
        }

        private void comboBox_seqMode_SelectedIndexChanged(object sender, EventArgs e) {
            if (_sequencer != null) {
                var cb = sender as ComboBox;
                if (cb != null) {
                    switch (cb.SelectedIndex) {
                        case 0: _sequencer.SequenceMode = SEQUENCE_MODE.UP; break;
                        case 1: _sequencer.SequenceMode = SEQUENCE_MODE.DOWN; break;
                    }
                }
            }
        }


        #region Sequencer parameters
        private void TrackLength_ValueChanged(object sender, EventArgs e) {
            NumericUpDown nud = sender as NumericUpDown;
            if (nud != null) {
                int trackId = 0;
                Int32.TryParse(nud.Name[nud.Name.Length - 1].ToString(), out trackId);
                if (trackId > 0) {
                    var track = _sequencer.GetTrack(trackId);
                    if (track != null) {
                        track.Length = (int)nud.Value;

                        // Update del numero de NoteCombobox en la pista
                        Stack<NoteComboBox> cbs = null;
                        switch (trackId) {
                            case 1: cbs = _notesComboBoxesTrack1; break;
                            case 2: cbs = _notesComboBoxesTrack2; break;
                            case 3: cbs = _notesComboBoxesTrack3; break;
                        }
                        if (cbs != null) {
                            MelodicTrack mt = track as MelodicTrack;
                            if (mt != null) {  // Track Melódica
                                while (cbs.Count > track.Length) {
                                    var noteCb = cbs.Pop();
                                    noteCb.Dispose();
                                }

                                while (cbs.Count < track.Length) {
                                    var noteCb = new NoteComboBox(trackId, cbs.Count);
                                    noteCb.ComboBox.SelectedText = CSNoteHandler.GetNoteValue(mt.Notes[cbs.Count]);
                                    noteCb.NoteValueChange += OnNoteComboBoxValueChange;
                                    nud.Parent.Controls.Add(noteCb.ComboBox);
                                    cbs.Push(noteCb);
                                }
                            } else {
                                RitmicTrack rt = track as RitmicTrack;
                                if (rt != null) {  // Track Rítmica
                                    // TODO: Nada?
                                }
                            }
                        }
                    }

                }
            }
        }

        private void OnNoteComboBoxValueChange(int tId, int sId, string value) {
            if ((tId > 0) && (tId <= _sequencer.Tracks.Length)&&(value != null)&&(!value.Equals(String.Empty))) {
                var track = _sequencer.GetTrack(tId) as MelodicTrack;  // Sólo las tracks melódicas tienen estos controles
                if (track != null) {
                    if ((sId >= 0) && (sId < track.Length)) {
                        track.Notes[sId] = CSNoteHandler.GetPchValue(value);
                    }
                }
            }
        }

        private void TrackVolumenChange(object sender, EventArgs e) {
            TrackBar tb = sender as TrackBar;
            if ((tb != null)&&(_sequencer != null)) {
                int trackId = 0;
                Int32.TryParse(tb.Name[tb.Name.Length - 1].ToString(), out trackId);
                if (trackId > 0) {
                    var track = _sequencer.GetTrack(trackId);
                    if (track != null)
                        track.Volumen = tb.Value / 100d;
                }
            }
        }

        private void trackBar_MainVol_Scroll(object sender, EventArgs e) {
            if (_sequencer != null)
                _sequencer.MainVolumen = trackBar_MainVol.Value / 100d;
        }
        #endregion
    }
}
