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

namespace Sequencer {
    public partial class Form1 : Form {

        private Board _board;
        private PolygonDrawingTool _polyDrawTool;

        public Form1() {
            InitializeComponent();
            _polyDrawTool = new PolygonDrawingTool(imageBox_mainDisplay);
        }
    }
}
