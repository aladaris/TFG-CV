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

    /// <summary>
    /// Defines at a global level, the values of the Figures used by
    /// the sequencer.
    /// </summary>
    static public class Figures {
        static private Figure _corchea = new Figure(FIGNAME.CORCHEA);
        static private Figure _negra = new Figure(FIGNAME.NEGRA);
        static private Figure _blanca = new Figure(FIGNAME.BLANCA);

        static public Figure Corchea {
            get { return _corchea; }
        }
        static public Figure Negra {
            get { return _negra; }
        }
        static public Figure Blanca {
            get { return _blanca; }
        }

        static public Figure GetFigure(int area) {
            if ((_corchea.MinArea <= area) && (area <= _corchea.MaxArea))
                return _corchea;
            if ((_negra.MinArea <= area) && (area <= _negra.MaxArea))
                return _negra;
            if ((_blanca.MinArea <= area) && (area <= _blanca.MaxArea))
                return _blanca;
            return null;
        }
    }

    public class Sequencer : IDisposable {
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
        // Tracks
        private Track[] _tracks;
        // CSound
        private CsoundHandler _csHandler;
        private csound6netlib.Csound6NetThread _csPerfThread;
        private int _bpm;
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
        //public event GeneratedImage<Gray, Double> ColorFilteredFrame;

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

        public csound6netlib.Csound6NetRealtime CSound {
            get {
                if (_csHandler != null)
                    return _csHandler.Csound;
                return null;
            }
        }

        public bool CSoundRunning { get; private set; }

        public Track[] Tracks {
            get {
                return _tracks;
            }
        }

        public bool FlipH { get; set; }
        public bool FlipV { get; set; }
        public int Bpm {
            get {
                return _bpm;
            }
            set {
                if (value > 0) {
                    _bpm = value;
                }
            }
        }
        public double MainVolumen { get; set; }
        #endregion

        /// <summary>
        /// This constructor doesnt initializes any of the tracks.
        /// They must be deffined after creation, by the user.
        /// </summary>
        /// <param name="i_disp">Main display used by the application.</param>
        public Sequencer(ImageBox i_disp, int tracks_count) {
            if ((i_disp != null) && (tracks_count > 0)) {
                _mainDisplay = i_disp;
                _camera = new Capture();  // TODO: Selección de cámara
                _fpsTimer = new System.Timers.Timer(1000 / _fps);
                _fpsTimer.Elapsed += GetNewFrame;
                _board = new Board();
                //_colorFilter = new ProbabilisticImageFiltering(3);
                CSoundRunning = false;
                InitCSound();
                _tracks = new Track[tracks_count];
                FlipH = true;
                FlipV = true;
            } else {
                throw new NullReferenceException("An ImageBox, and at least one (1) track, are required.");
            }
        }

        private void InitCSound() {
            if (_csHandler == null) {
                _csHandler = new CsoundHandler(new string[] { @"Files\sequencer.csd"});
                string scorePath = @"Files\score.txt";
                //_csHandler.Csound.ReadScore("i10 0 1000\n");  // TODO: Change me !!
                if (File.Exists(scorePath)){
                    var score = File.ReadAllText(scorePath);
                    _csHandler.Csound.ReadScore(score);
                }
                _csHandler.Csound.OutputChannelCallback += OnCSoundOutputChannel;
                _csHandler.Csound.InputChannelCallback += OnCSoundInputChannel;
                
            }
        }

        public void StartCSound() {
            if ((_csHandler != null)&&(!CSoundRunning)) {
                if (_csPerfThread != null) {
                    _csPerfThread.Dispose();
                }
                _csPerfThread = new csound6netlib.Csound6NetThread(_csHandler);
                CSoundRunning = true;
                //_csHandler.Csound.ReadScore("i10 0 100\n");
            }
        }

        /// <summary>
        /// Event generated on every "outvalue" instruction generated by
        /// the CSound instance.
        /// It triggers the step CV reading/updating procedure when it receives an Index change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCSoundOutputChannel(object sender, csound6netlib.Csound6ChannelEventArgs e) {
            if ((PerspectiveCalibrated) && (StepCount > 0) && (CSoundRunning)) {
                int index = 0;
                Image<Bgr, byte> step_img = null;
                Track track = null;
                int tNumber = -1;
                Int32.TryParse(e.Name[e.Name.Length - 1].ToString(), out tNumber);  // Los nombres de los canales acaban con el número de la pista
                if (tNumber >= 0) {
                    track = GetTrack(tNumber);
                }
                if (track != null) {
                    Int32.TryParse(e.Value.ToString(), out index);
                    index++;
                    if (index >= track.Length)
                        index = 0;
                    step_img = GetStepROI(index);
                    if (step_img != null)
                        track.ReadStepAsync<Bgr>(index, step_img);
                }
            }
        }

        /// <summary>
        /// Event generated on every "invalue" instruction generated by
        /// the CSound instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCSoundInputChannel(object sender, csound6netlib.Csound6ChannelEventArgs e) {
            Track track = null;
            switch (e.Name) {
                case "bpm": e.SetCsoundValue(CSound, (double)Bpm); break;
                case "MainVol": e.SetCsoundValue(CSound, MainVolumen); break;
                case "Vol1":
                    track = GetTrack(1);
                    if (track != null)
                        e.SetCsoundValue(CSound, track.Volumen);
                    break;
                case "Vol2":
                     track = GetTrack(2);
                    if (track != null)
                        e.SetCsoundValue(CSound, track.Volumen);
                    break;
                case "Vol3":
                     track = GetTrack(3);
                    if (track != null)
                        e.SetCsoundValue(CSound, track.Volumen);
                    break;
            }
        }

        /// <summary>
        /// Generates a image with the content in the specified step.
        /// Anything outside of the step is blacked out.
        /// </summary>
        /// <param name="step_id"></param>
        /// <returns></returns>
        public Image<Bgr, byte> GetStepROI(int step_id) {  // TODO: Private
            if ((_board != null)&&(_board.Steps.Count > 0)&&(step_id >= 0)&&(_board.StepsCount - 1 >= step_id)) {
                Step step = _board.Steps[step_id];
                var bRectangle = step.GetBoundingRectangle();
                var stepArea = _frame.Copy(bRectangle);  // Recortamos un rectángulo que contanga el paso.
                
                CvInvoke.cvShowImage("Step", stepArea.Ptr);  // DEBUG
                CvInvoke.cvWaitKey(0);  // DEBUG

                var mask = new Image<Gray, byte>(stepArea.Width, stepArea.Height, new Gray(0));
                mask.Draw(Step.GetDisplacedStep(step, bRectangle.Left, bRectangle.Top), new Gray(255), 0);

                CvInvoke.cvShowImage("Mask", mask.Ptr);  // DEBUG
                CvInvoke.cvWaitKey(0);  // DEBUG
                CvInvoke.cvShowImage("Result", stepArea.Copy(mask).Ptr);  // DEBUG
                CvInvoke.cvWaitKey(0);  // DEBUG

                return stepArea.Copy(mask);
            }
            return null;
        }

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
        public void SetFilterColor(Object i_sample, int track_id) {
            if (i_sample != null) {
                var track = GetTrack(track_id);
                if (track != null) {
                    try {
                        track.ColorFilter.SetDistributionValues<Bgr>(i_sample);
                    } catch (ArgumentException) {
                        MessageBox.Show("Área seleccionada inválida.");
                        return;
                    }
                    // Si ya está corregida la perspectiva, comienzamos con el filtrado de color
                    if (PerspectiveCalibrated) {
                        this.PerspectiveCorrectedFrame += FilterImage;
                        track.ColorFilter.ImageFiltered += OnFilteredImage;
                    }
                } else {
                    throw new ArgumentOutOfRangeException(String.Format("No Track with ID {0} on the Sequencer.", track_id));
                }
            } else {
                throw new NullReferenceException("A sample (or list of samples) is nedded to set the filter color");
            }
        }

        public Track GetTrack(int track_id) {
            if (_tracks != null){
                for (int i = 0; i < _tracks.Length; i++){
                    if (_tracks[i].Id == track_id)
                        return _tracks[i];
                }
            }
            return null;
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
        /// Draws the board
        /// </summary>
        private void PaintBoard(object sender, PaintEventArgs e) {
            if ((_board != null) && (DrawSteps)) {
                _board.DrawSteps(e);

                // Now we draw the current figures on each step table
                for (int i = 0; i < _tracks.Length; i++) {
                    var t = _tracks[i];

                    for (int j = 0; j < t.Durations.Length; j++ ) {
                        string figureCharacter = "";
                        switch ((int)t.Durations[j]) {
                            case 1: figureCharacter = "♪"; break;
                            case 2: figureCharacter = "♩"; break;
                            case 4: figureCharacter = "♭"; break;
                        }
                        if ((t.AvtiveSteps[j] > 0d) && (_board.Steps.Count > 0) && (_board.Steps.Count > j)) {
                            e.Graphics.DrawString(figureCharacter, new Font("Arial", 14), new SolidBrush(t.ColorFilter.SampleMeanColor), _board.Steps[j].Center);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Triggered on every "Elapsed" event of the FPS timer.
        /// Triggers a "RawFrame" event.
        /// </summary>
        private void GetNewFrame(object sender, EventArgs e) {
            if (_capturing && RawFrame != null) {
                var f = _camera.RetrieveBgrFrame();
                if (FlipH)
                    f._Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
                if (FlipV)
                    f._Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);
                RawFrame(f, e);
            }
        }

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
        /// This handler receives a perspective corrected frame and triggers
        /// a color filtering on the ProbabilisticImageFiltering object.
        /// </summary>
        /// <param name="i_img">Perspective corrected frame</param>
        private void FilterImage(Image<Bgr, Byte> i_img, EventArgs e) {
            //_colorFilter.FilterImage<Bgr>(i_img);
            /*
            var watch = System.Diagnostics.Stopwatch.StartNew();  // DEBUG: Time measure
            var filterTask = _colorFilter.FilterImageAsync<Bgr>(i_img);
            if (ColorFilteredFrame != null) {
                var img = await filterTask;
                ColorFilteredFrame(img, e);
            }
            watch.Stop();  // DEBUG: Time measure
            MessageBox.Show("Ellapsed (ms): " + watch.ElapsedMilliseconds.ToString());  // DEBUG: Time measure
            */
        }

        /// <summary>
        /// Handles the event generated by the ProbabilisticImageFiltering whenever
        /// it filters a frame.
        /// Triggers the "ColorFilteredFrame" event.
        /// </summary>
        /// <param name="i_img">The color filtered image, generated by the ProbabilisticImageFiltering</param>
        private void OnFilteredImage(Image<Gray, Double> i_img, EventArgs e) {
            /*if (ColorFilteredFrame != null)
                ColorFilteredFrame(i_img, e);
            */
        }
        #endregion
    }


}
