using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV.UI;

namespace Sequencer {
    class Step : Polygon {
        private int _id;
        #region Properties
        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Public Methods
        public Step(List<Point> i_poly, int i_id = -1) : base(i_poly) {
            _id = i_id;
        }
        #endregion
    }
}
