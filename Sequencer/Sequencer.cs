using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using Aladaris;

namespace Sequencer {

    class Sequencer : IDisposable{
        private Board _board;
        // Capture and display
        private Capture _camera;
        private bool _capturing = false;
        private Image<Bgr, Byte> _frame;  // This frame will be allways "perspective corrected".
        private ImageBox _mainDisplay;  // Main display, on the form, for showing the frames
        // Calibration
        private HomographyMatrix _calibMatrix;
        private bool _validCalibMatrix = false;
        private bool _correctingPerspect = false;
        // FPS handling
        private System.Timers.Timer _fpsTimer;
        private int _fps = 6;
        // Computer Vision
        private ProbabilisticImageFiltering _colorFilter;
        // Options
        private bool _drawSteps = false;
        // XML
        private string _configFilePath = "sequencer.config.xml";
        // Events
        public event GeneratedImage<Bgr, Byte> RawFrame;
        public event GeneratedImage<Bgr, Byte> PerspectiveCorrectedFrame;
        public event GeneratedImage<Gray, Double> ColorFilteredFrame;

        public Sequencer(ImageBox i_disp) {
            if (i_disp != null) {
                _mainDisplay = i_disp;
                _camera = new Capture();  // TODO: Selección de cámara
                //_camera.ImageGrabbed += OnRawFrame;
                _fpsTimer = new System.Timers.Timer(1000 / _fps);
                _fpsTimer.Elapsed += GetNewFrame;
                _board = new Board();
                _colorFilter = new ProbabilisticImageFiltering(3);
            } else {
                throw new NullReferenceException("An ImageBox is required.");
            }
        }

        #region Properties
        /*
        public Capture Camera {
            get { return _camera; }
        }
        */
        public bool Capturing {
            get { return _capturing; }
        }

        /// <summary>
        /// Matrix used on the image perspective correction
        /// done on each frame from the camera.
        /// </summary>
        public HomographyMatrix CalibrationMatrix {
            get { return _calibMatrix; }
        }

        public bool DrawSteps {
            get { return _drawSteps; }
            set {
                if (value == true) {
                    _mainDisplay.Paint -= PaintBoard;  // Eliminamos cualquier posible subscripción anterior
                    _mainDisplay.Paint += PaintBoard;
                } else
                    _mainDisplay.Paint -= PaintBoard;
                _drawSteps = value;
            }
        }

        public int Fps {
            get { return _fps; }
            set {
                _fps = value >= 1 ? value : 1;
                _fpsTimer.Interval = 1000 / _fps;
            }
        }

        public Image<Bgr, Byte> Frame {
            get { return _frame; }
            set { _frame = value; }
        }

        public bool PerspectiveCalibrated {
            get { return _validCalibMatrix; }
        }


        public int StepCount {
            get { return _board.StepsCount; }
        }

        #endregion

        #region Public methods

            #region CV
        public bool StartCapture() {
            if (_camera != null) {
                _camera.Start();
                _capturing = true;
                _fpsTimer.Enabled = true;
                return true;
            }
            return false;
        }

        public bool StopCapture() {
            if (_camera != null) {
                _camera.Stop();
                _capturing = false;
                _fpsTimer.Enabled = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates the perspective transformation matrix,
        /// used on the perspective correction of each frame.
        /// </summary>
        /// <param name="i_poly">List of points. Expected from a "PolygonDrawingTool".</param>
        public void SetPerspectiveCalibration(List<Point> i_poly) {
            if (i_poly.Count == 4) {
                //Size destSize = _display.Size;
                PointF[] dest_corners = new PointF[4];
                dest_corners[0] = new PointF(0f, 0f);
                dest_corners[1] = new PointF(_mainDisplay.Size.Width, 0f);
                dest_corners[2] = new PointF(_mainDisplay.Size.Width, _mainDisplay.Size.Height);
                dest_corners[3] = new PointF(0f, _mainDisplay.Size.Height);

                PointF[] sorted_corners = Polygon.SortRectangleCorners(i_poly);

                _calibMatrix = CameraCalibration.GetPerspectiveTransform(sorted_corners, dest_corners);
                _validCalibMatrix = true;
            }

            if (PerspectiveCalibrated)
                this.RawFrame += CalibrateIncomingFrame;
                //_camera.ImageGrabbed += CalibrateIncomingFrame;
            else
                this.RawFrame -= CalibrateIncomingFrame;
                //_camera.ImageGrabbed -= CalibrateIncomingFrame;
        }

        public void ResetPerspectiveCalibration() {
            _validCalibMatrix = false;
            this.RawFrame -= CalibrateIncomingFrame;
            //_camera.ImageGrabbed -= CalibrateIncomingFrame;
        }

        public void SetFilterColor(Object i_sample) {
            if (i_sample != null) {
                _colorFilter.SetDistributionValues<Bgr>(i_sample);
                // Si ya está corregida la perspectiva, comienzamos con el filtrado de color
                if (PerspectiveCalibrated) {
                    this.PerspectiveCorrectedFrame += FilterImage;
                    _colorFilter.ImageFiltered += OnFilteredImage;
                }
            } else
                throw new NullReferenceException("A sample (or list of samples) is nedded to set the filter color");
        }

            #endregion

            #region Board
        public void AddStep(Step i_step) {
            _board.AddStep(i_step);
        }

        public void AddStep(List<Point> i_points) {
            Step s = new Step(i_points);
            _board.AddStep(s);
        }

        public void ClearSteps() {
            _board.ClearSteps();
        }
            #endregion

        /// <summary>
        /// Saves (serializes) the sequencer state, and all its meaningfull atributes,
        /// to the xml configuration file.
        /// </summary>
        public void Save() {
            XDocument configFile;
            // Intentamos abrir el fichero, sino existe creamos uno con la estructura xml básica.
            try {
                configFile = XDocument.Load(_configFilePath);
            } catch (IOException) {
                configFile = new XDocument(
                    new XDeclaration("1.0", Encoding.UTF8.HeaderName, "yes"),
                    new XComment("Sequencer configuration file"),
                    new XElement("sequencer")
                    );
            }
            XElement sequencerXml = configFile.Descendants("sequencer").First();
            sequencerXml.RemoveNodes();
            sequencerXml.Add(Board.SerializeAsXElement(_board));
            configFile.Save(_configFilePath);
        }

        public void Load() {
            XDocument configFile;
            // Intentamos abrir el fichero, sino existe creamos uno con la estructura xml básica.
            try {
                configFile = XDocument.Load(_configFilePath);
            } catch (IOException) {
                MessageBox.Show("Can't open the sequencer definition file", "IO error");
                return;
            }
            XElement boardXml = configFile.Element("sequencer").Element("board");
            _board = Board.DeserializeFromXElement(boardXml);
        }

        public void Dispose() {
            _camera.Stop();
            _capturing = false;
            _camera.Dispose();
        }
        #endregion

        #region Handlers
        /// <summary>
        /// This event is launched whenever a frame is aviable at the capture (i.e: sender).
        /// The frame is proccessed to be calibrated and saved into the object atribute '_frame'.
        /// </summary>
        /// <param name="sender">A Capture object with an aviable frame to retrieve.</param>
        private void CalibrateIncomingFrame(Image<Bgr, Byte> i_frame, EventArgs e) {
            if ((PerspectiveCalibrated) && (!_correctingPerspect)) {
                _correctingPerspect = true;
                Image<Bgr, Byte> nframe = i_frame.Resize(_mainDisplay.Size.Width, _mainDisplay.Size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4);
                _frame = nframe.WarpPerspective(_calibMatrix, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4, Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new Bgr(Color.Black));
                if (PerspectiveCorrectedFrame != null)
                    PerspectiveCorrectedFrame(_frame, e);
                _correctingPerspect = false;
            }
        }

        /// <summary>
        /// Draws the board
        /// </summary>
        private void PaintBoard(object sender, PaintEventArgs e) {
            if (DrawSteps)
                _board.DrawSteps(e);
        }

        /// <summary>
        /// Triggered on every frame retrieved by the capture device.
        /// Triggers the "RawFrame" event.
        /// </summary>
        private void OnRawFrame(object sender, EventArgs e) {
            // TODO: Desvincular del evento de la cámara para dejar de escuchar cuando ya se tenga calibrada la perspectiva??
            if (!PerspectiveCalibrated) {
                Image<Bgr, Byte> frame = ((Capture)sender).RetrieveBgrFrame().Clone();
                RawFrame(frame, e);
            }
        }

        private void GetNewFrame(object sender, EventArgs e) {
            if (_capturing && RawFrame != null)
                RawFrame(_camera.RetrieveBgrFrame(), e);
        }

        /// <summary>
        /// 
        /// </summary>
        private void FilterImage(Image<Bgr, Byte> i_img, EventArgs e) {
            _colorFilter.FilterImage<Bgr>(i_img);
        }

        private void OnFilteredImage(Image<Gray, Double> i_img, EventArgs e) {
            if (ColorFilteredFrame != null)
                ColorFilteredFrame(i_img, e);
        }
        #endregion
    }


}
