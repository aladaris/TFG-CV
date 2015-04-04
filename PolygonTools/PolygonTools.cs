using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualTools {

    public delegate void GetPolygon(List<Point> poly);

    public class PolygonTools {
        #region Atributes
        private List<Point> _points = new List<Point>();
        private PictureBox _pictBox;
        private Color _brushColor = Color.FromArgb(128, 72, 145, 220);
        private Brush _brush;
        private Point _pointDrawSize = new Point(5, 5);
        private bool _enabled = false;
        #endregion

        #region Properties
        public List<Point> Points {
            get {
                return _points;
            }
        }

        public PictureBox PictureBox {
            set {
                _pictBox = value;
            }
        }

        public Color LineColor {
            get {
                return _brushColor;
            }
            set {
                _brushColor = value;
            }
        }

        public Point PointSize {
            get {
                return _pointDrawSize;
            }
            set {
                _pointDrawSize = value;
            }
        }

        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
                if (_enabled)
                    AttachEvents();
                else
                    DeattachEvents();
            }
        }
        #endregion

        #region Public Methods
        public PolygonTools(PictureBox i_picBox) {
            _brush = new SolidBrush(_brushColor);
            _pictBox = i_picBox;
        }
        #endregion

        #region Events
        /// <summary>
        /// Evento generado cuando se desea obtener el polígono.
        /// Al hacer "Shift + Click izquierdo" se lanza este evento que
        /// pasará como parámetro la lista de puntos.
        /// </summary>
        public event GetPolygon ReturnPolygon;

        /// <summary>
        /// Click Izquierdo: Añade un punto.
        /// Click Derecho: Elimina el último punto añadido.
        /// </summary>
        private void OnClick(object sender, EventArgs e) {
            MouseEventArgs eM = (MouseEventArgs)e;
            if (eM.Button == MouseButtons.Left)
                if (Control.ModifierKeys != Keys.Shift)
                    _points.Add(eM.Location);
                else {
                    // Al devolver el polígono, borramos la lista de puntos
                    // pues esto implica qeu se desea comenzar con uno nuevo.
                    if (ReturnPolygon != null) {
                        ReturnPolygon(_points);
                        _points.Clear();
                    }
                }
            else if ((eM.Button == MouseButtons.Right) && (_points.Count >= 1))
                _points.RemoveAt(_points.Count - 1);
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            // Draw Points
            foreach (Point p in _points)
                e.Graphics.FillEllipse(_brush, p.X - (_pointDrawSize.X / 2), p.Y - (_pointDrawSize.Y / 2), _pointDrawSize.X, _pointDrawSize.Y);
            // Draw Lines
            if (_points.Count > 1) {
                Pen pen = new Pen(_brush);
                pen.Width = 2;
                e.Graphics.DrawPolygon(pen, _points.ToArray());
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Establece "listeneres" para los eventos, del PictureBoxm, necesarios.
        /// </summary>
        private void AttachEvents() {
            if (_pictBox != null) {
                _pictBox.Click += OnClick;
                _pictBox.Paint += OnPaint;
            }
        }

        /// <summary>
        /// Elimina los "listeners" asociados a los eventos del PictureBox,
        /// dejando de este modo, esta herramienta, deshabilitada
        /// </summary>
        private void DeattachEvents() {
            if (_pictBox != null) {
                _pictBox.Click -= OnClick;
                _pictBox.Paint -= OnPaint;
            }
        }
        #endregion
    }
}
