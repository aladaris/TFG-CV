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
        private Point[] _poly;
        private PointF _center;
        // Drawing stuff
        private Color _pointColor = Color.FromArgb(200, 255, 107, 107);
        private Brush _pointBrush;
        private Color _polygonColor = Color.FromArgb(150, 78, 205, 196);
        private Brush _polygonBrush;
        private Color _centerColor = Color.FromArgb(220, 85, 98, 112);
        private Brush _centerBrush;
        private int _pointRadius = 7;
        private int _centerRadius = 4;
        private bool _showCenter = true;

        #region Properties
        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        public int PointRadius {
            get { return _pointRadius;  }
            set {
                if (value >= 0) {
                    _pointRadius = value;
                } else
                    throw new ArgumentOutOfRangeException("PointRadius", "Radius must be greater than zero (0).");
            }
        }
        public int CenterRadius {
            get { return _centerRadius; }
            set {
                if (value >= 0) {
                    _centerRadius = value;
                } else
                    throw new ArgumentOutOfRangeException("CenterRadius", "Radius must be greater than zero (0).");
            }
        }
        public bool ShowCenter {
            get { return _showCenter; }
            set { _showCenter = value; }
        }
        #endregion

        #region Public Methods
        public Step(List<Point> i_poly, int i_id = -1) {
            _id = i_id;
            _poly = i_poly.ToArray();
            _pointBrush = new SolidBrush(_pointColor);
            _polygonBrush = new SolidBrush(_polygonColor);
            _centerBrush = new SolidBrush(_centerColor);
            _center = PolygonHandling.GetMassCenter(i_poly);
        }

        public void Draw(PaintEventArgs e) {
            // Draw Polygon
            if (_poly.Length > 1) {
                e.Graphics.FillPolygon(_polygonBrush, _poly);
            }
            // Draw Points
            for (int i = 0; i < _poly.Length; i++) {
                Point p = _poly[i];
                e.Graphics.FillEllipse(_pointBrush, p.X - _pointRadius / 2, p.Y - _pointRadius / 2, _pointRadius, _pointRadius);
            }
            // Draw Center
            if (_showCenter)
                e.Graphics.FillEllipse(_centerBrush, _center.X - _centerRadius / 2, _center.Y - _centerRadius / 2, _centerRadius, _centerRadius);
        }
        #endregion
    }
}
