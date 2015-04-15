﻿using System;
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
        // Options
        private bool _drawSteps = false;
        // XML
        private string _configFilePath = "sequencer.config.xml";

        public Sequencer(ImageBox i_disp) {
            if (i_disp != null) {
                _mainDisplay = i_disp;
                _camera = new Capture();  // TODO: Selección de cámara
                _board = new Board();
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

        /// <summary>
        /// Matrix used on the image perspective correction
        /// done on each frame from the camera.
        /// </summary>
        public HomographyMatrix CalibrationMatrix {
            get { return _calibMatrix; }
        }

        public bool PerspectiveCalibrated {
            get { return _validCalibMatrix; }
        }

        public Image<Bgr, Byte> Frame {
            get { return _frame; }
            set { _frame = value; }
        }

        public int StepCount {
            get { return _board.StepsCount; }
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
        #endregion

        #region Public methods

            #region CV
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
                _camera.ImageGrabbed += CalibrateIncomingFrame;
            else
                _camera.ImageGrabbed -= CalibrateIncomingFrame;
        }

        public void ResetPerspectiveCalibration() {
            _validCalibMatrix = false;
            _camera.ImageGrabbed -= CalibrateIncomingFrame;
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
            #endregion

        /// <summary>
        /// Saves (serializes) the sequencer state, and all its meaningfull atributes,
        /// to the xml configuration file.
        /// </summary>
        public void Save() {
            XDocument _configFile;
            // Intentamos abrir el fichero, sino existe creamos uno con la estructura xml básica.
            try {
                _configFile = XDocument.Load(_configFilePath);
            } catch (IOException) {
                _configFile = new XDocument(
                    new XDeclaration("1.0", Encoding.UTF8.HeaderName, "yes"),
                    new XComment("Sequencer configuration file"),
                    new XElement("sequencer")
                    );
            }
            // Tests
            /*
            List<Step> pasos = new List<Step>() { 
                new Step(new List<Point>() { 
                    new Point(0, 0), new Point(0, 6), new Point(6, 6), new Point(6, 0) 
                }),
                new Step(new List<Point>() { 
                    new Point(6, 6), new Point(6, 12), new Point(12, 12), new Point(12, 6) 
                })
            };
            Board tabla = new Board();
            foreach (Step s in pasos)
                tabla.AddStep(s);
            */
            // TODO: A partir de aquí sería el método final (para serializar y guardar el sequencer), cambiando las cosas de Test por las reales
            XElement sequencerXml = _configFile.Descendants("sequencer").First();
            sequencerXml.RemoveNodes();
            sequencerXml.Add(Board.SerializeAsXElement(_board));
            _configFile.Save(_configFilePath);
        }

        public void Load() {
            XDocument _configFile;
            // Intentamos abrir el fichero, sino existe creamos uno con la estructura xml básica.
            try {
                _configFile = XDocument.Load(_configFilePath);
            } catch (IOException) {
                MessageBox.Show("Can't open the sequencer definition file", "IO error");
                return;
            }
            XElement boardXml = _configFile.Element("sequencer").Element("board");
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
        private void CalibrateIncomingFrame(object sender, EventArgs e) {
            Capture cam = (Capture)sender;
            if ((PerspectiveCalibrated) && (!_correctingPerspect)) {
                _correctingPerspect = true;
                Image<Bgr, Byte> nframe = cam.RetrieveBgrFrame().Clone().Resize(_mainDisplay.Size.Width, _mainDisplay.Size.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4);
                _frame = nframe.WarpPerspective(_calibMatrix, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4, Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new Bgr(Color.Black));
                _correctingPerspect = false;
                _mainDisplay.Image = _frame;
            }
        }

        private void PaintBoard(object sender, PaintEventArgs e) {
            if (DrawSteps)
                _board.DrawSteps(e);
        }
        #endregion
    }


}
