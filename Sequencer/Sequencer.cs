using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Sequencer {

    class Sequencer : IDisposable{
        private Board _board;
        private Capture _camera;
        private bool _capturing = false;
        private bool _correctingPerspect = false;
        private ImageBox _mainDisplay;  // Main display, on the form, for showing the frames

        #region Events
        /// <summary>
        /// Event generated on every perspective correction to a frame.
        /// </summary>
        public event GeneratedImage<Bgr, Byte> PerspectiveCorrectedFrame;
        #endregion

        public Sequencer(ImageBox i_disp) {
            if (i_disp != null) {
                _mainDisplay = i_disp;
                _camera = new Capture();  // TODO: Selección de cámara
                _board = new Board(_camera, _mainDisplay);
            } else {
                throw new NullReferenceException("An ImageBox is needed.");
            }
        }

        #region Properties
        public Capture Camera {
            get { return _camera; }
        }

        public bool Capturing {
            get { return _capturing; }
        }

        public bool PerspectiveCalibrated {
            get { return _board.PerspectiveCalibrated; }
        }
        #endregion

        #region Public methods
        public bool StartCapture() {
            if (_camera != null) {
                _camera.Start();
                _capturing = true;
                return true;
            }
            return false;
        }

        public bool StopCapture() {
            if (_camera != null) {
                _camera.Stop();
                _capturing = false;
                return true;
            }
            return false;
        }

        public void SetPerspectiveCalibration(List<Point> i_corners) {
            if (i_corners.Count == 4)
                _board.SetPerspectiveCalibration(i_corners, _mainDisplay.Size);

            if (_board.PerspectiveCalibrated)
                _camera.ImageGrabbed += OnImageToFilter;
            else
                _camera.ImageGrabbed -= OnImageToFilter;
        }

        public void ResetPerspectiveCalibration() {
            _board.ResetPerspectiveCalibration();
            _camera.ImageGrabbed -= OnImageToFilter;
        }

        public void Dispose() {
            _camera.Stop();
            _capturing = false;
            _camera.Dispose();
        }
        #endregion

        #region Handlers
        private void OnImageToFilter(object sender, EventArgs e) {
            Capture cam = (Capture)sender;
            if ((_board.PerspectiveCalibrated) && (!_correctingPerspect) && (PerspectiveCorrectedFrame != null)) {
                _correctingPerspect = true;
                Image<Bgr, Byte> nframe = cam.RetrieveBgrFrame().Clone().Resize(_mainDisplay.Size.Width, _mainDisplay.Size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4);
                _board.Frame = nframe.WarpPerspective(_board.CalibrationMatrix, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4, Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new Bgr(Color.Black));
                PerspectiveCorrectedFrame(_board.Frame, e);  // Generamos el evento con el frame corregido
                _correctingPerspect = false;
            }
        }
        #endregion
    }


}
