using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using Emgu.CV.UI;

namespace Sequencer {
    class Step {
        private int _id;
        private List<Point> _poly;
        // Drawing stuff
        private Color _brushColor = Color.FromArgb(53, 35, 93, 220);
        private Brush _brush;
        private Point _pointDrawSize = new Point(7, 7);

        #region Properties
        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Public Methods
        public Step(List<Point> i_poly, int i_id = -1) {
            _id = i_id;
            _poly = i_poly.ToList();
            _brush = new SolidBrush(_brushColor);
        }

        public void Draw(PaintEventArgs e) {
            // Draw Points
            foreach (Point p in _poly)
                e.Graphics.FillEllipse(_brush, p.X - (_pointDrawSize.X / 2), p.Y - (_pointDrawSize.Y / 2), _pointDrawSize.X, _pointDrawSize.Y);
            // Draw Lines
            if (_poly.Count > 1) {
//                Pen pen = new Pen(_brush);
//                pen.Width = 2;
                e.Graphics.FillPolygon(_brush, _poly.ToArray());
            }
        }
        #endregion
    }
}
