using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sequencer {

    public class NoteComboBox : IDisposable {
        private readonly string[] notes_list;

        public ComboBox ComboBox { get; set; }
        
        public NoteComboBox() {
            notes_list = CSNoteHandler.ListOfNotes;
            ComboBox = new ComboBox();
            ComboBox.Size = new Size(45, ComboBox.Size.Height);
            ComboBox.Items.AddRange(notes_list);
            ComboBox.AutoCompleteMode = AutoCompleteMode.Append;
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
        }

        public void Dispose() {
            if (ComboBox != null) {
                ComboBox.Dispose();
            }
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
            if ((_sequencer.PerspectiveCalibrated) && (_sequencer.StepCount > 0) && (!_sequencer.CSoundRunning))
                _sequencer.StartCSound();
        }

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
                            while (cbs.Count > track.Length) {
                                var noteCb = cbs.Pop();
                                noteCb.Dispose();
                            }
                            while (cbs.Count < track.Length) {
                                var noteCb = new NoteComboBox();
                                noteCb.ComboBox.SelectedText = CSNoteHandler.GetNoteValue(track.Notes[cbs.Count]);
                                nud.Parent.Controls.Add(noteCb.ComboBox);
                                cbs.Push(noteCb);
                            }
                        }
                    }

                }
            }
        }
        #endregion
    }
}
