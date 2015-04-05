using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

namespace Sequencer {

    class Board {

        private SortedList<ushort, Step> _steps;
        private HomographyMatrix _calibMatrix;

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

        public Board() {
            _steps = new SortedList<ushort, Step>();
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
        /// <param name="i_destSize">Size of the output image.</param>
        public void SetPerspectiveCalibration(List<Point> i_poly, Size i_destSize) {
            PointF[] dest_corners = new PointF[4];
            dest_corners[0] = new PointF(0f, 0f);
            dest_corners[1] = new PointF(i_destSize.Width, 0f);
            dest_corners[2] = new PointF(i_destSize.Width, i_destSize.Height);
            dest_corners[3] = new PointF(0f, i_destSize.Height);

            PointF[] sorted_corners = PolygonHandling.SortCorners(i_poly);

            _calibMatrix = CameraCalibration.GetPerspectiveTransform(sorted_corners, dest_corners);
        }

        #endregion
    }
}
