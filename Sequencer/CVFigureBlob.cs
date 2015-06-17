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
    public enum FIGNAME { CORCHEA = 1, NEGRA = 2, BLANCA = 4 };

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
        private bool _valid = false;
        private ImageBox _imgBox;

        public Figure Figura { get; private set; }
        public double Area { get; private set; }

        public bool IsValid {
            get { return _valid; }
        }

        public CVFigureBlob(Rectangle boundingBox, double area, Figure fig, ImageBox imgbox) {
            boundingBox.Width *= 3;
            boundingBox.Height *= 3;
            boundingBox.X *= 3;
            boundingBox.Y *= 3;
            Area = area;
            _blobCenter = new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            Figura = fig;
            if (Figura != null)
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
                switch (Figura.FigureName) {
                    case FIGNAME.CORCHEA: figureCharacter = "♪"; break;
                    case FIGNAME.NEGRA: figureCharacter = "♩"; break;
                    case FIGNAME.BLANCA: figureCharacter = "♭"; break;
                }
                if (_imgBox != null)
                    _imgBox.CreateGraphics().DrawString(figureCharacter, new Font("Arial", 14), Brushes.Yellow, _blobCenter);
            }
        }

        /// <summary>
        /// Obtiene una lista con los blobs detectados en la imagen (ya filtrada) proporcionada.
        /// La lista es de objetos CVFigureBlob, así que también se le asigna una figura a cada blob.
        /// </summary>
        /// <param name="i_img"></param>
        /// <param name="display"></param>
        /// <returns></returns>
        public static List<CVFigureBlob> GetBlobs(Image<Gray, Double> i_img, ImageBox display) {
            //CvInvoke.cvSmooth(i_img.Ptr, i_img.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_GAUSSIAN, 13, 13, 1.5, 1);
            var gray = i_img.Convert<Gray, Byte>();//.PyrDown().PyrUp();
            CvInvoke.cvSmooth(gray.Ptr, gray.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_BLUR, 13, 13, 1.5, 1);
            Image<Gray, Byte> thresholded = gray.ThresholdBinary(new Gray(1), new Gray(255));
            List<CVFigureBlob> blobs = new List<CVFigureBlob>();
            using (MemStorage storage = new MemStorage()) {
                for (Contour<Point> contours = thresholded.Erode(1).FindContours(); contours != null; contours = contours.HNext) {
                    Contour<Point> currContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);
                    Figure fig = Figures.GetFigure((int)currContour.Area);
                    if (fig != null) {
                        blobs.Add(new CVFigureBlob(currContour.BoundingRectangle, currContour.Area, fig, display));
                        //blobs[blobs.Count - 1].Paint();  // DEBUG // VERBOSE
                    }
                }
            }
            return blobs;
        }

        public static CVFigureBlob GetBiggestBlob(Image<Gray, Double> i_img, ImageBox display) {
            var blobs = GetBlobs(i_img, display);
            double biggest = 0d;
            CVFigureBlob result = null;
            foreach (var blob in blobs) {
                if (biggest < blob.Area) {
                    result = blob;
                    biggest = blob.Area;
                }
            }
            return result;
        }

        private void OnPaint(object sender, PaintEventArgs e) {
            if (_valid) {
                string figureCharacter = "";
                switch (Figura.FigureName) {
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
