using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Sequencer {

    class Board {

        #region Atributes
        private SortedList<ushort, Step> _steps;
        private HomographyMatrix _calibMatrix;
        private bool _validCalibMatrix = false;
        private bool _correctingPerspect = false;
        private Image<Bgr, Byte> _frame;  // This frame will be allways "perspective corrected".
        private ImageBox _display;  // Reference to the display imageBox
        #endregion
        #region Events
        /// <summary>
        /// Event generated on every perspective correctec to a frame.
        /// </summary>
        public event GeneratedImage<Bgr, Byte> PerspectiveCorrectedFrame;
        #endregion

        #region Properties

        /// <summary>
        /// Matrix used on the image perspective correction
        /// done on each frame from the camera.
        /// </summary>
        public HomographyMatrix CalibrationMatrix {
            get { return _calibMatrix; }
        }

        #endregion

        #region Public Methods

        public Board(Capture i_cam, ImageBox i_display) {
            _steps = new SortedList<ushort, Step>();
            _display = i_display;
            i_cam.ImageGrabbed += OnImageGrabbed;
        }

        /// <summary>
        /// Add a step to the sequencer board.
        /// </summary>
        /// <param name="i_step">Step to be added</param>
        /// <returns>"true" if the step was added; "false" if not.</returns>
        public bool AddStep(Step i_step){
            try {
                _steps.Add(i_step.Id, i_step);
            } catch (ArgumentException) {
                DataLogging.ErrorLog("ArgumentException", "Board._steps", "Se ha intentado añadir un paso con ID ya existente.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates the perspective transformation matrix,
        /// used on the perspective correction of each frame.
        /// </summary>
        /// <param name="i_poly">List of points. Expected from a "PolygonDrawingTool".</param>
        public void SetPerspectiveCalibration(List<Point> i_poly) {
            Size destSize = _display.Size;
            PointF[] dest_corners = new PointF[4];
            dest_corners[0] = new PointF(0f, 0f);
            dest_corners[1] = new PointF(destSize.Width, 0f);
            dest_corners[2] = new PointF(destSize.Width, destSize.Height);
            dest_corners[3] = new PointF(0f, destSize.Height);

            PointF[] sorted_corners = PolygonHandling.SortCorners(i_poly);

            _calibMatrix = CameraCalibration.GetPerspectiveTransform(sorted_corners, dest_corners);
            _validCalibMatrix = true;
        }

        public void ResetPerspectiveCalibration() {
            _validCalibMatrix = false;
        }

        #endregion

        #region Private Methods
        #region Handlers
        private void OnImageGrabbed(object sender, EventArgs e) {
            Capture cam = (Capture)sender;
            if ((_validCalibMatrix) && (!_correctingPerspect)) {
                Image<Bgr, Byte> nframe = cam.RetrieveBgrFrame().Clone().Resize(_display.Size.Width, _display.Size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4);
                _correctingPerspect = true;
                _frame = nframe.WarpPerspective(_calibMatrix, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4, Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new Bgr(Color.Black));
                PerspectiveCorrectedFrame(_frame, e);  // Generamos el evento con el frame corregido
                _correctingPerspect = false;
            }
        }
        #endregion
        #endregion
    }
}
