using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;  // PaintEventArgs

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

        public Board() {
            _steps = new List<Step>();
        }

        public Board(IEnumerable<Step> i_steps) {
            _steps = i_steps.ToList();
        }

        public Step GetStep(int i_sId) {
            try {
                return _steps.Single(step => step.Id == i_sId);
            } catch {
                return null;
            }
        }

        public void DrawSteps(PaintEventArgs e) {
            foreach (Step s in _steps)
                s.Draw(e);
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

        #region Class Methods
        /// <summary>
        /// Serialize a Board as an XElement.
        /// </summary>
        /// <param name="i_board">Board to be serialized.</param>
        /// <returns>XElemnt of the board with Step childs.</returns>
        public static XElement SerializeAsXElement(Board i_board) {
            if (i_board != null) {
                XElement boardXml = new XElement("board", new XAttribute("size", i_board._steps.Count));
                foreach(Step s in i_board._steps) {
                    boardXml.Add(Step.SerializeAsXElement(s));
                }
                return boardXml;
            }
            return null;
        }

        /// <summary>
        /// Deserialize an XElement into a Board object.
        /// </summary>
        /// <param name="i_xboard">XElemnt with the Board data.</param>
        /// <returns>A new Board with the deserialized data.</returns>
        public static Board DeserializeFromXElement(XElement i_xboard) {
            IEnumerable<Step> steps = i_xboard.Descendants("step").Select(
                    x => Step.DeserializeFromXElement(x)
                );
            Board b = new Board(steps);
            return b;
        }
        #endregion
    }
}
