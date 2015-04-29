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
        private ImageBox _mainDisplay;  // Main display, on the form, for showing the frames  // TODO: Sacar de aquí; solo necesito el tamaño.
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
        /// <summary>
        /// Triggered at the stablished frames per second.
        /// Only triggered if "PerspectiveCalibrated" is false.
        /// </summary>
        public event GeneratedImage<Bgr, Byte> RawFrame;
        /// <summary>
        /// Triggered on every perspective corrected frame.
        /// </summary>
        public event GeneratedImage<Bgr, Byte> PerspectiveCorrectedFrame;
        /// <summary>
        /// Triggered on every color filtered frame.
        /// </summary>
        public event GeneratedImage<Gray, Double> ColorFilteredFrame;

        /// <summary>
        /// </summary>
        /// <param name="i_disp">Main display used by the application.</param>
        public Sequencer(ImageBox i_disp) {
            if (i_disp != null) {
                _mainDisplay = i_disp;
                _camera = new Capture();  // TODO: Selección de cámara
                _fpsTimer = new System.Timers.Timer(1000 / _fps);
                _fpsTimer.Elapsed += GetNewFrame;
                _board = new Board();
                _colorFilter = new ProbabilisticImageFiltering(3);
            } else {
                throw new NullReferenceException("An ImageBox is required.");
            }
        }

        #region Properties
        /// <summary>
        /// True if the sequencer is retrieving frames from
        /// the capture device.
        /// </summary>
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

        /// <summary>
        /// Configure if the board steps are drawn or not.
        /// </summary>
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

        /// <summary>
        /// Configure the frames per second retrieved
        /// from the capture device.
        /// </summary>
        public int FpsIn {
            get { return _fps; }
            set {
                _fps = value >= 1 ? value : 1;
                _fpsTimer.Interval = 1000 / _fps;
            }
        }

        /// <summary>
        /// Indicates if the sequencer frames are
        /// being perspective calibrated.
        /// </summary>
        public bool PerspectiveCalibrated {
            get { return _validCalibMatrix; }
        }

        /// <summary>
        /// Bet the number of steps on the board.
        /// </summary>
        public int StepCount {
            get { return _board.StepsCount; }
        }

        #endregion

        #region Public methods

            #region CV
        /// <summary>
        /// Sterts retrieving frames from the capture device.
        /// </summary>
        public void StartCapture() {
            if (_camera != null) {
                _camera.Start();
                _capturing = true;
                _fpsTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Stops retrieving frames from the capture device.
        /// </summary>
        public void StopCapture() {
            if (_camera != null) {
                _camera.Stop();
                _capturing = false;
                _fpsTimer.Enabled = false;
            }
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
            else
                this.RawFrame -= CalibrateIncomingFrame;
        }

        /// <summary>
        /// Resets the perspective calibration parameters.
        /// </summary>
        public void ResetPerspectiveCalibration() {
            _validCalibMatrix = false;
            this.RawFrame -= CalibrateIncomingFrame;
        }

        /// <summary>
        /// Configures the PerspectiveImageFilter to fit a
        /// sample (or a collection of samples) from an image.
        /// </summary>
        /// <param name="i_sample">An Image object or a List<Image> with a collection of samples.</param>
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
        /// <summary>
        /// Add a step to the board
        /// </summary>
        /// <param name="i_step">The step to be added</param>
        public void AddStep(Step i_step) {
            _board.AddStep(i_step);
        }

        /// <summary>
        /// Add a step to the board from a polygon.
        /// </summary>
        /// <param name="i_points">A collection of points representing the step polygon</param>
        public void AddStep(IEnumerable<Point> i_points) {
            Step s = new Step(i_points);
            _board.AddStep(s);
        }

        /// <summary>
        /// Clear all the steps in the board.
        /// </summary>
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

        /// <summary>
        /// Loads the XML file with the serialized board data.
        /// Then it deserializes it and load the board data.
        /// </summary>
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

        /// <summary>
        /// Stops the capture device and releases any resource.
        /// </summary>
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
        /// Triggered on every "Elapsed" event of the timer.
        /// Triggers a "RawFrame" event.
        /// </summary>
        private void GetNewFrame(object sender, EventArgs e) {
            if (_capturing && RawFrame != null)
                RawFrame(_camera.RetrieveBgrFrame(), e);
        }

        /// <summary>
        /// This handler receives a perspective corrected frame and triggers
        /// a color filtering on the ProbabilisticImageFiltering object.
        /// </summary>
        /// <param name="i_img">Perspective corrected frame</param>
        private void FilterImage(Image<Bgr, Byte> i_img, EventArgs e) {
            _colorFilter.FilterImage<Bgr>(i_img);
        }

        /// <summary>
        /// Handles the event generated by the ProbabilisticImageFiltering whenever
        /// it filters a frame.
        /// Triggers the "ColorFilteredFrame" event.
        /// </summary>
        /// <param name="i_img">The color filtered image, generated by the ProbabilisticImageFiltering</param>
        private void OnFilteredImage(Image<Gray, Double> i_img, EventArgs e) {
            if (ColorFilteredFrame != null)
                ColorFilteredFrame(i_img, e);
        }
        #endregion
    }


}
