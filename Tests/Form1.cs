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

namespace Tests {
    public partial class Form1 : Form {

        private Capture _cap;
        private bool captureInProgress;

        public Form1() {
            InitializeComponent();
        }

        private void ProcessFrame(object sender, EventArgs arg) {
            Image<Bgr, Byte> frame = _cap.QueryFrame();
            frame = frame.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);

            Image<Gray, Byte> bw = frame.Convert<Gray, Byte>();

            bw = bw.ThresholdBinaryInv(new Gray(80), new Gray(160));

            LineSegment2D[][] lines = bw.HoughLinesBinary(1, Math.PI / 180, trackBar_Threshold.Value, (double)(trackBar_minLineWidth.Value), (double)(trackBar_gapBetweenLines.Value));
            //LineSegment2D[][] lines = frame.HoughLines(100, 100, 50, Math.PI / 180, 70, 30, 10);

 
            
            //foreach (LineSegment2D[] color_lines in lines) {
            foreach (var line in lines[0]) {
                    LineSegment2D l = line;
                    frame.Draw(line, new Bgr(255, 0, 255), 2);
                }
            //}



            pictureBox_display.Image = frame.ToBitmap();
        }

        private void ReleaseData() {
            if (_cap != null)
                _cap.Dispose();
        }

        private void button_start_Click(object sender, EventArgs e) {
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            ReleaseData();
        }
    }
}
