using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;

namespace PerspectiveCorrection {
    public partial class Form1 : Form {

        private Capture _cap;
        private Image<Bgr, Byte> _frame;
        private Rectangle _rectPreview = new Rectangle();
        private List<Point> _polygon = new List<Point>();
        private const int _rectPreviewSize = 25;
        private bool captureInProgress;

        public Form1() {
            InitializeComponent();
            _rectPreview.Size = new Size(_rectPreviewSize, _rectPreviewSize);
        }

        private void ProcessFrame(object sender, EventArgs arg) {
            _frame = _cap.RetrieveBgrFrame().Clone().Resize(pictureBox_display.Width, pictureBox_display.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true);
            //_frame = _frame.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);

            pictureBox_display.Image = _frame.ToBitmap();
        }

        private void ReleaseData() {
            if (_cap != null)
                _cap.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            ReleaseData();
        }

        private void button_start_Click_1(object sender, EventArgs e) {
            if (_cap == null) {
                try {
                    _cap = new Capture();
                } catch (NullReferenceException excp) {
                    MessageBox.Show(excp.Message);
                }
            }
            if (_cap != null) {
                if (captureInProgress) {
                    button_start.Text = "Start";
                    Application.Idle -= ProcessFrame;
                } else {
                    button_start.Text = "Stop";
                    Application.Idle += ProcessFrame;
                }
                captureInProgress = !captureInProgress;
            }
        }

        private void pictureBox_display_MouseMove(object sender, MouseEventArgs e) {
            if (_frame != null) {
                Point pos = e.Location;

                if ((pos.X - (_rectPreview.Width / 2) > 0) && (pos.Y - (_rectPreview.Height / 2) > 0)) {

                    label_position.Text = pos.ToString();  // DEBUG
                    pos.X = pos.X - (_rectPreview.Size.Width / 2);
                    pos.Y = pos.Y - (_rectPreview.Size.Height / 2);
                    _rectPreview.Location = pos;
                    try {
                        pictureBox_zoomPreview.Image = _frame.Copy(_rectPreview).Resize(pictureBox_zoomPreview.Width, pictureBox_zoomPreview.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR).ToBitmap();
                    } catch {
                        return;
                    }
                }
            }
        }

        // Dibujamos el racténgulo que representa el área a extrae
        // También se dibuja el poligono dibujado (puntos y lineas)
        private void pictureBox_display_Paint(object sender, PaintEventArgs e) {
            Brush _previewBrush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
            
            // Draw preview Box
            e.Graphics.FillRectangle(_previewBrush, _rectPreview);

            _previewBrush = new SolidBrush(Color.FromArgb(100, 200, 20, 200));
            // Draw Points
            Point POLYPOINT_SIZE = new Point(5, 5);
            foreach (Point p in _polygon) {
                e.Graphics.FillEllipse(_previewBrush, p.X - (POLYPOINT_SIZE.X / 2), p.Y - (POLYPOINT_SIZE.Y / 2), POLYPOINT_SIZE.X, POLYPOINT_SIZE.Y);
            }
            // Draw Lines
            if (_polygon.Count > 1) {
                Pen pen = new Pen(_previewBrush);
                pen.Width = 2;
                e.Graphics.DrawPolygon(pen, _polygon.ToArray());
            }
        }

        // Se pinta una "mirilla", en el área de preview, para calcular el centro de lo apuntado
        private void pictureBox_zoomPreview_Paint(object sender, PaintEventArgs e) {
            Pen pen = new Pen(Color.FromArgb(255, 255, 0, 255));
            Rectangle rect = new Rectangle();
            rect.Width = 10;
            rect.Height = 10;
            rect.X = (pictureBox_zoomPreview.Width / 2) - (rect.Width / 2);
            rect.Y = (pictureBox_zoomPreview.Height / 2) - (rect.Height / 2);
            e.Graphics.DrawEllipse(pen, rect);
            e.Graphics.DrawLine(pen, new Point(rect.X - 3, rect.Y + (rect.Height / 2)), new Point(rect.X + rect.Width + 3, rect.Y + (rect.Height / 2)));
            e.Graphics.DrawLine(pen, new Point(rect.X + (rect.Width / 2), rect.Y - 3), new Point(rect.X + (rect.Width / 2), rect.Y + rect.Height + 3));
        }

        // Al hacer click izquierdo, añadimos un punto al polygon
        // Con click derecho borramos el último punto añadido
        private void pictureBox_display_Click(object sender, EventArgs e) {
            MouseEventArgs eM = (MouseEventArgs)e;
            if (eM.Button == MouseButtons.Left)
                _polygon.Add(eM.Location);
            else if ((eM.Button == MouseButtons.Right) && (_polygon.Count >= 1))
                _polygon.RemoveAt(_polygon.Count - 1);
        }
    }
}
