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

    class CVFigureBlob {
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

        public void Paint() {
            if (_valid) {
                string figureCharacter = "";
                MelodicFigure mf = Figura as MelodicFigure;

                if (mf != null) {  // Figura melódica
                    switch (mf.FigureName) {
                        case FIGNAME.CORCHEA: figureCharacter = "♪"; break;
                        case FIGNAME.NEGRA: figureCharacter = "♩"; break;
                        case FIGNAME.BLANCA: figureCharacter = "♭"; break;
                    }
                } else {
                    RitmicFigure rf = Figura as RitmicFigure;
                    if (rf != null) {  // Figura Rítmica
                        switch (rf.RitmicPart){
                            case RHYTHMPART.KICK: figureCharacter = "K"; break;
                            case RHYTHMPART.SNARE: figureCharacter = "S"; break;
                            case RHYTHMPART.HIHAT: figureCharacter = "H"; break;
                        }
                    }
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
        public static List<CVFigureBlob> GetBlobs(Image<Gray, Double> i_img, ImageBox display, CV_BLOB_TYPE blob_type) {
            if (i_img != null) {
                var gray = i_img.Convert<Gray, Byte>();
                CvInvoke.cvSmooth(gray.Ptr, gray.Ptr, Emgu.CV.CvEnum.SMOOTH_TYPE.CV_BLUR, 13, 13, 1.5, 1);
                Image<Gray, Byte> thresholded = gray.ThresholdBinary(new Gray(1), new Gray(255));
                List<CVFigureBlob> blobs = new List<CVFigureBlob>();
                using (MemStorage storage = new MemStorage()) {
                    for (Contour<Point> contours = thresholded.Erode(1).FindContours(); contours != null; contours = contours.HNext) {
                        Contour<Point> currContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);
                        int area = currContour.BoundingRectangle.Width * currContour.BoundingRectangle.Height;
                        switch (blob_type) {
                            case CV_BLOB_TYPE.MELODIC:
                                MelodicFigure mfig = MelodicFigures.GetFigure(area);
                                if (mfig != null)
                                    blobs.Add(new CVFigureBlob(currContour.BoundingRectangle, currContour.Area, mfig, display));
                                break;
                            case CV_BLOB_TYPE.RITMIC:
                                RitmicFigure rfig = RitmicFigures.GetFigure(area);
                                if (rfig != null)
                                    blobs.Add(new CVFigureBlob(currContour.BoundingRectangle, currContour.Area, rfig, display));
                                break;
                        }
                    }
                }
                return blobs;
            }
            return null;
        }

        public static CVFigureBlob GetBiggestBlob(Image<Gray, Double> i_img, ImageBox display, CV_BLOB_TYPE blob_type) {
            var blobs = GetBlobs(i_img, display, blob_type);
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
                MelodicFigure mf = Figura as MelodicFigure;
                if (mf != null) {  // Figura melódica
                    switch (mf.FigureName) {
                        case FIGNAME.CORCHEA: figureCharacter = "♪"; break;
                        case FIGNAME.NEGRA: figureCharacter = "♩"; break;
                        case FIGNAME.BLANCA: figureCharacter = "♭"; break;
                    }
                } else {
                    RitmicFigure rf = Figura as RitmicFigure;
                    if (rf != null) {  // Figura Rítmica
                        switch (rf.RitmicPart) {
                            case RHYTHMPART.KICK: figureCharacter = "K"; break;
                            case RHYTHMPART.SNARE: figureCharacter = "S"; break;
                            case RHYTHMPART.HIHAT: figureCharacter = "H"; break;
                        }
                    }
                }
                ImageBox imgbox = (ImageBox)sender;
                e.Graphics.DrawString(figureCharacter, new Font("Arial", 14), Brushes.Yellow, _blobCenter);
            }
        }

    }
}
