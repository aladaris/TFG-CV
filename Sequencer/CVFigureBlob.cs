using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Sequencer {

    /// <summary>
    /// Musical figures names
    /// </summary>
    public enum FIGNAME { CORCHEA, NEGRA, BLANCA };

    /// <summary>
    /// Defines the blob area range asociated with a musical figure.
    /// Area range overlaps, between figures, should be avoided.
    /// </summary>
    public class Figure {
        private int[] _areaRange = new int[2];
        private FIGNAME _figure;

        public Figure(FIGNAME i_fig) {
            _figure = i_fig;
        }
        /// <summary>
        /// Minimum area size asociated with this duration.
        /// On set, if the value if greater than MaxArea, it its
        /// set to MaxArea - 1 (minimum value of 0)
        /// </summary>
        public int MinArea {
            get { return _areaRange[0]; }
            set {
                if (value < _areaRange[1])
                    _areaRange[0] = value;
                else
                    _areaRange[0] = _areaRange[1] - 1 > 0 ? _areaRange[1] - 1 : 0;
            }
        }
        /// <summary>
        /// Maximum area size asociated with this duration.
        /// On set, if the value if lower than MinArea, it its
        /// set to MinArea + 1
        /// </summary>
        public int MaxArea {
            get { return _areaRange[1]; }
            set {
                if (value > _areaRange[0])
                    _areaRange[1] = value;
                else
                    _areaRange[1] = _areaRange[0] + 1;
            }
        }
        /// <summary>
        /// The figure associated with this area sizes.
        /// </summary>
        public FIGNAME FigureName {
            get { return _figure; }
            set { _figure = value; }
        }
    }

    class CVFigureBlob /*: IDisposable*/ {
        //private Rectangle _blob;
        private Point _blobCenter;
        private Figure _figure;
        private bool _valid = false;
        private ImageBox _imgBox;

        public bool IsValid {
            get { return _valid; }
        }
/*
        public CVFigureBlob(Rectangle boundingBox, double area, ImageBox imgbox) {
            _blob = boundingBox;
            _figure = Figures.GetFigure((int)area);
            if (_figure != null)
                _valid = true;
            imgbox.Paint += OnPaint;
        }
*/
        public CVFigureBlob(Rectangle boundingBox, Figure fig, ImageBox imgbox) {
            boundingBox.Width *= 3;
            boundingBox.Height *= 3;
            boundingBox.X *= 3;
            boundingBox.Y *= 3;
            _blobCenter = new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            _figure = fig;
            if (_figure != null)
                _valid = true;
            _imgBox = imgbox;
            //_imgBox.Paint += OnPaint;
        }

        ~CVFigureBlob() {
            //_imgBox.Paint -= OnPaint;
        }
        /*
        public void Dispose() {
            _imgBox.Paint -= OnPaint;
        }
        */

        public void Paint() {
            if (_valid) {
                string figureCharacter = "";
                switch (_figure.FigureName) {
                    case FIGNAME.CORCHEA: figureCharacter = "♪"; break;
                    case FIGNAME.NEGRA: figureCharacter = "♩"; break;
                    case FIGNAME.BLANCA: figureCharacter = "♭"; break;
                }
                _imgBox.CreateGraphics().DrawString(figureCharacter, new Font("Arial", 14), Brushes.Yellow, _blobCenter);
            }
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            if (_valid) {
                string figureCharacter = "";
                switch (_figure.FigureName) {
                    case FIGNAME.CORCHEA: figureCharacter = "♪"; break;
                    case FIGNAME.NEGRA: figureCharacter = "♩"; break;
                    case FIGNAME.BLANCA: figureCharacter = "♭"; break;
                }
                ImageBox imgbox = (ImageBox)sender;
                e.Graphics.DrawString(figureCharacter, new Font("Arial", 14), Brushes.Yellow, _blobCenter);
                //e.Graphics.FillEllipse(Brushes.Yellow, _blobCenter.X, _blobCenter.Y, 4, 4);
            }
        }

    }
}
