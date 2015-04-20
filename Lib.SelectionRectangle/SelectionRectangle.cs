/*
   Copyright © 2014 Fernando González López - Peñalver <aladaris@gmail.com>
   This file is part of EmguCV-Projects.

    EmguCV-Projects is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License.

    EmguCV-Projects is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with EmguCV-Projects.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Aladaris {

    public delegate void GeneratedSample<C, D>(Image<C, D> i_sample)
        where C : struct, IColor
        where D : new();

    public delegate void GeneratedSampleList<C, D>(IEnumerable<Image<C, D>> i_samples)
        where C : struct, IColor
        where D : new();

    public class SelectionRectangle {
        #region Atributes
        private Point _startPoint;
        private Rectangle _rect = new Rectangle();
        private static Color _brushColor = Color.FromArgb(128, 72, 145, 220);
        private Brush _selBrush = new SolidBrush(_brushColor);
        private ImageBox _control;
        private bool _enabled;
        #region MultiSelection
        private List<Rectangle> _rectangles = new List<Rectangle>();
        private static Color _brushColorMulti = Color.FromArgb(128, 220, 72, 105);
        private Brush _selBrushMulti = new SolidBrush(_brushColorMulti);
        #endregion
        #region Events
        /// <summary>
        /// Evento generado cada vez que se obtiene una muestra.
        /// </summary>
        public event GeneratedSample<Bgr, Byte> AcquiredSample;
        /// <summary>
        /// Evento generado cada vez que se genera un listado de muestras.
        /// </summary>
        public event GeneratedSampleList<Bgr, Byte> AcquiredSampleList;
        #endregion
        #endregion

        #region Properties
        public ImageBox ControlImageBox {
            get {
                return _control;
            }
            set {
                _control = value;
            }
        }

        public bool Enabled {
            get { return _enabled; }
            set {
                _enabled = value;
                if (value)
                    AttachHandlers();
                else
                    DeattachHandlers();
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i_imBox">The ImageBox to work with</param>
        public SelectionRectangle(ImageBox i_imBox, bool i_enabled = false) {
            _control = i_imBox;
            Enabled = i_enabled;
        }

        #region Private Methods
        /// <summary>
        /// Devuelve una nueva imagen compuesta por lo contenido en el rectangulo '_rect'
        /// </summary>
        /// <typeparam name="C">Espacio de Color</typeparam>
        /// <typeparam name="D">Profundidad</typeparam>
        /// <param name="i_img">Imagen de la que extraer la muestra</param>
        /// <returns>Muestra obtenida</returns>
        private Image<C, D> GetSelectedImage<C, D>(Image<C, D> i_img)
            where C : struct, IColor
            where D : new() {
            try {
                return i_img.Copy(_rect);
            } catch {
                System.Windows.Forms.MessageBox.Show("[ERROR] Area seleccionada insuficiente.", "ERROR de selección");
                return null;
            }
        }

        /// <summary>
        /// Devuelve un listado de imágenes, cada una generada por cada uno de los rectangulos en '_rectangles'
        /// </summary>
        /// <typeparam name="C">Espacio de Color</typeparam>
        /// <typeparam name="D">Profundidad</typeparam>
        /// <param name="i_img">Imagen de la que extraer la muestra</param>
        /// <returns>Listado de muestras obtenidas</returns>
        private List<Image<C, D>> GetSelectedImagesMulti<C, D>(Image<C, D> i_img)
            where C : struct, IColor
            where D : new() {
            if (_rectangles.Capacity <= 0)
                return new List<Image<C, D>>();

            var result = new List<Image<C, D>>();
            Image<C, D> sample = null;
            foreach (Rectangle r in _rectangles) {
                try {
                    sample = i_img.Copy(r);
                } catch {
                    System.Windows.Forms.MessageBox.Show("[ERROR] Una de las áreas seleccionadas es insuficiente.", "ERROR de selección");
                }
                if (sample != null)
                    result.Add(sample);
            }
            return result;
        }

        #region Support Methods
        /// <summary>
        /// Attach this object to the ImageBox control
        /// </summary>
        private void AttachHandlers() {
            _control.MouseDown += this.MouseDown;
            _control.MouseUp += this.MouseUp;
            _control.MouseMove += this.MouseMove;
            _control.Paint += this.Paint;
        }

        /// <summary>
        /// Deattach this object from the ImageBox control
        /// </summary>
        private void DeattachHandlers() {
            _control.MouseDown -= this.MouseDown;
            _control.MouseUp -= this.MouseUp;
            _control.MouseMove -= this.MouseMove;
            _control.Paint -= this.Paint;
        }
        #endregion

        #endregion

        #region EventHandlers
        private void MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left){
                _startPoint = e.Location;
                _control.Invalidate();
                
            }
        }
        private void MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left){
                Point tmpEndPoint = e.Location;
                _rect.Location = new Point(Math.Min(_startPoint.X, tmpEndPoint.X), Math.Min(_startPoint.Y, tmpEndPoint.Y));
                _rect.Size = new Size(Math.Abs(_startPoint.X - tmpEndPoint.X), Math.Abs(_startPoint.Y - tmpEndPoint.Y));
                _control.Invalidate();
            }
        }
        private void Paint(object sender, PaintEventArgs e) {
            if (_control.Image != null) {
                if (Control.ModifierKeys != Keys.Shift) {
                    if (_rect != null && _rect.Width > 0 && _rect.Height > 0)
                        e.Graphics.FillRectangle(_selBrush, _rect);
                } else {
                    if (_rect != null)
                        e.Graphics.FillRectangle(_selBrush, _rect);

                    foreach (Rectangle r in _rectangles)
                        e.Graphics.FillRectangle(_selBrushMulti, r);
                }
            }
        }
        /// <summary>
        /// Selección simple: Click izquierdo: se obtiene el sample (genera evento AcquiredSample).
        /// Selección múltple: Click izquierdo + Shift: crear u nuevo rectángulo de selección múltiple.
        ///                    Click derecho   + Shift: Obtiene el listado de samples (genera evento AcquiredSampleList).
        /// </summary>
        private void MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                // LEFT BUTTON
                if (Control.ModifierKeys == Keys.None) {
                    // Selección sencilla
                    Image<Bgr, Byte> sample = GetSelectedImage<Bgr, Byte>(new Image<Bgr, byte>(_control.Image.Bitmap));
                    // Send event!
                    if (AcquiredSample != null)
                        AcquiredSample(sample);
                } else if (Control.ModifierKeys == Keys.Shift) {
                    // Selección múltiple
                    _rectangles.Add(_rect);
                }
                _rect = new Rectangle();
                // SHIFT + RIGHT BUTTON (Get multiselection sample)
            } else if (e.Button == MouseButtons.Right && Control.ModifierKeys == Keys.Shift) {
                List<Image<Bgr, Byte>> samples;
                samples = GetSelectedImagesMulti<Bgr, Byte>(new Image<Bgr, byte>(_control.Image.Bitmap));
                _rectangles = new List<Rectangle>();
                // Send event!
                if (AcquiredSampleList != null)
                    AcquiredSampleList(samples);
            }
        }
        #endregion
    }
}