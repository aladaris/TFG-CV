using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Sequencer {

    class Polygon {
        #region Atributes
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
        #endregion

        #region Properties
        public int VerticesCount {
            get { return _poly.Length; }
        }
        public int PointRadius {
            get { return _pointRadius; }
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

        /*
        public Polygon() {
            _poly = new Point[8];  // TODO: Magic Number
            _pointBrush = new SolidBrush(_pointColor);
            _polygonBrush = new SolidBrush(_polygonColor);
            _centerBrush = new SolidBrush(_centerColor);
            _center = new PointF(0f, 0f);
        }
        */
        public Polygon(List<Point> i_poly) {
            _poly = i_poly.ToArray();
            _pointBrush = new SolidBrush(_pointColor);
            _polygonBrush = new SolidBrush(_polygonColor);
            _centerBrush = new SolidBrush(_centerColor);
            _center = GetMassCenter(i_poly);
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

        #region Class Methods
        /// <summary>
        /// Gets the, aproximate, mass center of a give Polygon.
        /// </summary>
        /// <param name="i_polygon">List of points, wich represents the polygon.</param>
        /// <returns>A point with floating point precision.</returns>
        public static PointF GetMassCenter(List<Point> i_polygon) {
            PointF center = new PointF(0f, 0f);
            foreach (Point p in i_polygon) {
                center.X += p.X;
                center.Y += p.Y;
            }
            center.X *= (1f / i_polygon.Count);
            center.Y *= (1f / i_polygon.Count);
            return center;
        }

        /// <summary>
        /// Arranges the given rectangle vertices to the formart
        /// used by OpenCV.
        /// </summary>
        /// <param name="i_corners">List with the four vertices</param>
        /// <returns></returns>
        public static PointF[] SortRectangleCorners(List<Point> i_corners) {
            PointF massCenter = GetMassCenter(i_corners);
            Point[] top = new Point[2];
            Point[] bott = new Point[2];
            int top_pos = 0;
            int bott_pos = 0;

            if (i_corners.Count != 4)
                throw new ArgumentException("Four corners expected", "i_corners");

            foreach (Point c in i_corners) {
                if (c.Y < massCenter.Y)
                    top[top_pos++] = c;
                else
                    bott[bott_pos++] = c;
            }

            PointF[] result = {
                        top[0].X > top[1].X ? top[1] : top[0],      // Top-Left
                        top[0].X > top[1].X ? top[0] : top[1],      // Top-Right
                        bott[0].X > bott[1].X ? bott[0] : bott[1],  // Bottom-Right
                        bott[0].X > bott[1].X ? bott[1] : bott[0],  // Bottom-Left
                     };

            return result;
        }
        #endregion
    }
}
