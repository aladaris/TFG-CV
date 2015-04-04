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

using VisualTools;

namespace PerspectiveCorrection {
    public partial class Form1 : Form {

        private Capture _cap;
        private Image<Bgr, Byte> _frame;
        private Rectangle _rectPreview = new Rectangle();
        private PointF[] _referencePoints;
        private const int _rectPreviewSize = 25;
        private bool captureInProgress;
        private PolygonTools _polyTool;

        public Form1() {
            InitializeComponent();
            _polyTool = new PolygonTools(pictureBox_display);
            _polyTool.ReturnPolygon += OnPolygonReturned;
            _rectPreview.Size = new Size(_rectPreviewSize, _rectPreviewSize);
        }

        private void ProcessFrame(object sender, EventArgs arg) {
            _frame = _cap.RetrieveBgrFrame().Clone().Resize(pictureBox_display.Width, pictureBox_display.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true);
            //_frame = _frame.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);

            if (_referencePoints != null) {
                if (checkBox_perspectivecorrect.Checked) {
                    pictureBox_display.Image = CorrectPerspective(_frame, _referencePoints).ToBitmap();
                    return;
                }
            }
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
                        pictureBox_zoomPreview.Image = _frame.Copy(_rectPreview).Resize(pictureBox_zoomPreview.Width, pictureBox_zoomPreview.Height, Emgu.CV.CvEnum.INTER.CV_INTER_NN).ToBitmap();
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

        private PointF GetMassCenter(List<Point> i_polygon) {
            PointF center = new PointF(0f, 0f);
            foreach (Point p in i_polygon) {
                center.X += p.X;
                center.Y += p.Y;
            }
            center.X *= (1f / i_polygon.Count);
            center.Y *= (1f / i_polygon.Count);

            return center;
        }

        /// <summary>
        /// Organiza los cuatro vértices de un rectángulo, para que
        /// su format(orden en la lista) sea el correcto para su uso.
        /// </summary>
        /// <param name="i_corners">Lista de puntos a ordenar</param>
        /// <returns></returns>
        private PointF[] SortCorners(List<Point> i_corners) {
            PointF massCenter = GetMassCenter(i_corners);
            Point[] top = new Point[2];
            Point[] bott = new Point[2];
            int top_pos = 0;
            int bott_pos = 0;

            if (i_corners.Count != 4)
                throw new ArgumentException("Four corners expected", "i_corners");

            foreach (Point c in i_corners) {
                if (c.Y < massCenter.Y)
                    top[top_pos++] = c;
                else
                    bott[bott_pos++] = c;
            }

            PointF[] result = new PointF[4];
            result[0] = top[0].X > top[1].X ? top[1] : top[0];  // Top-Left
            result[1] = top[0].X > top[1].X ? top[0] : top[1];  // Top-Right
            result[2] = bott[0].X > bott[1].X ? bott[0] : bott[1];  // Bottom-Right
            result[3] = bott[0].X > bott[1].X ? bott[1] : bott[0];  // Bottom-Left

            return result;
        }

        private Image<Bgr, Byte> CorrectPerspective(Image<Bgr, Byte> i_img, PointF[] i_corners) {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(pictureBox_display.Width, pictureBox_display.Height);
            PointF[] dest_corners = new PointF[4];
            dest_corners[0] = new PointF(0f, 0f);
            dest_corners[1] = new PointF(result.Cols, 0f);
            dest_corners[2] = new PointF(result.Cols, result.Rows);
            dest_corners[3] = new PointF(0f, result.Rows);

            HomographyMatrix transform_matrix = CameraCalibration.GetPerspectiveTransform(i_corners, dest_corners);

            result = i_img.WarpPerspective(transform_matrix, Emgu.CV.CvEnum.INTER.CV_INTER_LANCZOS4, Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new Bgr(Color.Aqua));

            return result;
        }

        private void checkBox_drawRefPoints_CheckedChanged(object sender, EventArgs e) {
            _polyTool.Enabled = checkBox_drawRefPoints.Checked;
        }

        private void OnPolygonReturned(List<Point> i_poly) {
            _referencePoints = SortCorners(i_poly);
        }
    }
}
