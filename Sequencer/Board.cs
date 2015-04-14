using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Sequencer {

    class Board {

        #region Atributes
        private List<Step> _steps;
        #endregion

        #region Properties
        public int StepsCount {
            get { return _steps.Count; }
        }
        public IReadOnlyList<Step> Steps {
            get { return _steps.AsReadOnly(); }
        }
        #endregion

        #region Public Methods

        public Board(Capture i_cam) {
            _steps = new List<Step>();
        }

        public Step GetStep(int i_sId) {
            try {
                return _steps.Single(step => step.Id == i_sId);
            } catch {
                return null;
            }
        }

        public void DrawSteps(PaintEventArgs e) {
            foreach (Step s in _steps){
                s.Draw(e);
            }
        }

        /// <summary>
        /// Add a step to the sequencer board.
        /// </summary>
        /// <param name="i_step">Step to be added</param>
        public void AddStep(Step i_step){
            if (i_step.Id < 0) {
                i_step.Id = _steps.Count > 0 ? _steps[_steps.Count - 1].Id + 1 : 0;
            }
            _steps.Add(i_step);
        }
        #endregion
    }
}
