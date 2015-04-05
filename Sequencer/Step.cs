using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Sequencer {
    class Step {
        private ushort _id;
        private List<Point> _poly;

        #region Properties
        public ushort Id {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Public Methods
        public Step(List<Point> i_poly, ushort i_id = 0) {
            _id = i_id;
            _poly = i_poly;
        }
        #endregion
    }
}
