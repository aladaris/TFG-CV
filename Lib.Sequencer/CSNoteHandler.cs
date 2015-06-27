using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sequencer {
    public static class CSNoteHandler {
        private static double MIDDLE_C_SHIFT = 4.00d;  // Desplazamiento respecto a la notación clásica (D4 = 0.02 + (4 + MIDDLE_C_SHIFT) = 8.02)
        private static int SCALES_COUNT = 8;
        private static string[] notes = new string[]{ 
                    "C", "C#",
                    "D", "D#",
                    "E",
                    "F", "F#",
                    "G", "G#",
                    "A", "A#",
                    "B"
                };
        private static Dictionary<string, double> notes_values = new Dictionary<string,double>{
                    {"C", 0.00d}, {"C#", 0.01d},
                    {"D", 0.02d}, {"D#", 0.03d},
                    {"E", 0.04d},
                    {"F", 0.05d}, {"F#", 0.06d},
                    {"G", 0.07d}, {"G#", 0.08d},
                    {"A", 0.09d}, {"A#", 0.10d},
                    {"B", 0.11d}
                };

        /// <summary>
        /// Generates a list of notes representing 8 full chromatic scales.
        /// </summary>
        public static string[] ListOfNotes {
            get {
                var result = new string[notes.Length * SCALES_COUNT];
                int curr_scale = -1;
                for (int i = 0; i < result.Length; i++) {
                    if (i % notes.Length == 0) {
                        curr_scale++;
                    }
                    result[i] = String.Format("{0}{1}", notes[i % notes.Length], curr_scale);
                }
                return result;
            }
        }

        /// <summary>
        /// Returns the CSound PCH value associated with a note.
        /// The expected notes are on the C1, D#4, ... format
        /// </summary>
        /// <param name="note"></param>
        /// <returns>Double representation of the pch value</returns>
        public static double GetPchValue(string note) {
            var regexp = new Regex(@"(?<note>[CDEFGAB]{1}[#]?)(?<scale>[\d]{1,2})");
            note = note.ToUpper();
            if (regexp.IsMatch(note)) {
                var match = regexp.Match(note);
                double nvalue = 0d;
                notes_values.TryGetValue(match.Groups["note"].Value, out nvalue);
                double scale = 0d;
                Double.TryParse(match.Groups["scale"].Value, out scale);
                return ((scale + MIDDLE_C_SHIFT) + nvalue);
            }
            return 0d;
        }

        public static string GetNoteValue(double pch) {
            decimal scale = (decimal)Math.Truncate(pch);
            decimal note_value = (decimal)pch - scale;  // Parte decimal
            scale -= (decimal)MIDDLE_C_SHIFT;// Parte entera - MIDDLE_C_SHIFT
            string result = "";
            foreach (var nv in notes_values) {
                if ((decimal)nv.Value == note_value) {
                    result = string.Format("{0}{1}", nv.Key, scale);
                    break;
                }
            }
            return result;
        }

    }
}
