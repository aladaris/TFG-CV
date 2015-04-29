namespace Sequencer {
    partial class Form1 {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.splitContainer_Base = new System.Windows.Forms.SplitContainer();
            this.splitContainer_LeftSideBase = new System.Windows.Forms.SplitContainer();
            this.imageBox_mainDisplay = new Emgu.CV.UI.ImageBox();
            this.flowLayoutPanel_CamStuff = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBox_CameraList = new System.Windows.Forms.ComboBox();
            this.button_startCamera = new System.Windows.Forms.Button();
            this.button_setPersCalib = new System.Windows.Forms.Button();
            this.tabControl_Modes = new System.Windows.Forms.TabControl();
            this.tabPage_Calibration = new System.Windows.Forms.TabPage();
            this.cameraSettingsControl = new System.Windows.Forms.Integration.ElementHost();
            this.cameraSettingsControl1 = new Aladaris.CameraSettingsControl();
            this.imageBox_preview = new Emgu.CV.UI.ImageBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_setColor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_clearSteps = new System.Windows.Forms.Button();
            this.button_addSteps = new System.Windows.Forms.Button();
            this.button_saveBoard = new System.Windows.Forms.Button();
            this.button_loadBoard = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_state = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Base)).BeginInit();
            this.splitContainer_Base.Panel1.SuspendLayout();
            this.splitContainer_Base.Panel2.SuspendLayout();
            this.splitContainer_Base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_LeftSideBase)).BeginInit();
            this.splitContainer_LeftSideBase.Panel1.SuspendLayout();
            this.splitContainer_LeftSideBase.Panel2.SuspendLayout();
            this.splitContainer_LeftSideBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_mainDisplay)).BeginInit();
            this.flowLayoutPanel_CamStuff.SuspendLayout();
            this.tabControl_Modes.SuspendLayout();
            this.tabPage_Calibration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_preview)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Base
            // 
            this.splitContainer_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Base.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_Base.IsSplitterFixed = true;
            this.splitContainer_Base.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Base.Name = "splitContainer_Base";
            // 
            // splitContainer_Base.Panel1
            // 
            this.splitContainer_Base.Panel1.Controls.Add(this.splitContainer_LeftSideBase);
            // 
            // splitContainer_Base.Panel2
            // 
            this.splitContainer_Base.Panel2.Controls.Add(this.tabControl_Modes);
            this.splitContainer_Base.Size = new System.Drawing.Size(1187, 666);
            this.splitContainer_Base.SplitterDistance = 864;
            this.splitContainer_Base.TabIndex = 0;
            // 
            // splitContainer_LeftSideBase
            // 
            this.splitContainer_LeftSideBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_LeftSideBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_LeftSideBase.IsSplitterFixed = true;
            this.splitContainer_LeftSideBase.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_LeftSideBase.Name = "splitContainer_LeftSideBase";
            this.splitContainer_LeftSideBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_LeftSideBase.Panel1
            // 
            this.splitContainer_LeftSideBase.Panel1.Controls.Add(this.imageBox_mainDisplay);
            // 
            // splitContainer_LeftSideBase.Panel2
            // 
            this.splitContainer_LeftSideBase.Panel2.Controls.Add(this.flowLayoutPanel_CamStuff);
            this.splitContainer_LeftSideBase.Size = new System.Drawing.Size(864, 666);
            this.splitContainer_LeftSideBase.SplitterDistance = 486;
            this.splitContainer_LeftSideBase.TabIndex = 0;
            // 
            // imageBox_mainDisplay
            // 
            this.imageBox_mainDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox_mainDisplay.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_mainDisplay.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imageBox_mainDisplay.Location = new System.Drawing.Point(0, 0);
            this.imageBox_mainDisplay.Name = "imageBox_mainDisplay";
            this.imageBox_mainDisplay.Size = new System.Drawing.Size(864, 486);
            this.imageBox_mainDisplay.TabIndex = 2;
            this.imageBox_mainDisplay.TabStop = false;
            // 
            // flowLayoutPanel_CamStuff
            // 
            this.flowLayoutPanel_CamStuff.Controls.Add(this.comboBox_CameraList);
            this.flowLayoutPanel_CamStuff.Controls.Add(this.button_startCamera);
            this.flowLayoutPanel_CamStuff.Controls.Add(this.button_setPersCalib);
            this.flowLayoutPanel_CamStuff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_CamStuff.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel_CamStuff.Name = "flowLayoutPanel_CamStuff";
            this.flowLayoutPanel_CamStuff.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel_CamStuff.Size = new System.Drawing.Size(864, 176);
            this.flowLayoutPanel_CamStuff.TabIndex = 0;
            // 
            // comboBox_CameraList
            // 
            this.comboBox_CameraList.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox_CameraList.FormattingEnabled = true;
            this.comboBox_CameraList.Location = new System.Drawing.Point(8, 8);
            this.comboBox_CameraList.Name = "comboBox_CameraList";
            this.comboBox_CameraList.Size = new System.Drawing.Size(191, 21);
            this.comboBox_CameraList.TabIndex = 1;
            // 
            // button_startCamera
            // 
            this.button_startCamera.Location = new System.Drawing.Point(205, 8);
            this.button_startCamera.Name = "button_startCamera";
            this.button_startCamera.Size = new System.Drawing.Size(75, 60);
            this.button_startCamera.TabIndex = 0;
            this.button_startCamera.Text = "Start Camera";
            this.button_startCamera.UseVisualStyleBackColor = true;
            this.button_startCamera.Click += new System.EventHandler(this.button_startCamera_Click);
            // 
            // button_setPersCalib
            // 
            this.button_setPersCalib.Enabled = false;
            this.button_setPersCalib.Location = new System.Drawing.Point(286, 8);
            this.button_setPersCalib.Name = "button_setPersCalib";
            this.button_setPersCalib.Size = new System.Drawing.Size(75, 60);
            this.button_setPersCalib.TabIndex = 4;
            this.button_setPersCalib.Text = "Set perspective correction rectangle";
            this.button_setPersCalib.UseVisualStyleBackColor = true;
            this.button_setPersCalib.Click += new System.EventHandler(this.button_setPersCalib_Click);
            // 
            // tabControl_Modes
            // 
            this.tabControl_Modes.Controls.Add(this.tabPage_Calibration);
            this.tabControl_Modes.Controls.Add(this.tabPage2);
            this.tabControl_Modes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Modes.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Modes.Name = "tabControl_Modes";
            this.tabControl_Modes.SelectedIndex = 0;
            this.tabControl_Modes.Size = new System.Drawing.Size(319, 666);
            this.tabControl_Modes.TabIndex = 0;
            // 
            // tabPage_Calibration
            // 
            this.tabPage_Calibration.Controls.Add(this.cameraSettingsControl);
            this.tabPage_Calibration.Controls.Add(this.imageBox_preview);
            this.tabPage_Calibration.Controls.Add(this.groupBox2);
            this.tabPage_Calibration.Controls.Add(this.groupBox1);
            this.tabPage_Calibration.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Calibration.Name = "tabPage_Calibration";
            this.tabPage_Calibration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Calibration.Size = new System.Drawing.Size(311, 640);
            this.tabPage_Calibration.TabIndex = 0;
            this.tabPage_Calibration.Text = "Calibration";
            this.tabPage_Calibration.UseVisualStyleBackColor = true;
            // 
            // cameraSettingsControl
            // 
            this.cameraSettingsControl.Location = new System.Drawing.Point(6, 180);
            this.cameraSettingsControl.Name = "cameraSettingsControl";
            this.cameraSettingsControl.Size = new System.Drawing.Size(302, 263);
            this.cameraSettingsControl.TabIndex = 9;
            this.cameraSettingsControl.Text = "elementHost1";
            this.cameraSettingsControl.Child = this.cameraSettingsControl1;
            // 
            // imageBox_preview
            // 
            this.imageBox_preview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox_preview.Location = new System.Drawing.Point(6, 449);
            this.imageBox_preview.Name = "imageBox_preview";
            this.imageBox_preview.Size = new System.Drawing.Size(302, 170);
            this.imageBox_preview.TabIndex = 2;
            this.imageBox_preview.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_setColor);
            this.groupBox2.Location = new System.Drawing.Point(6, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 73);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image setup";
            // 
            // button_setColor
            // 
            this.button_setColor.Enabled = false;
            this.button_setColor.Location = new System.Drawing.Point(6, 19);
            this.button_setColor.Name = "button_setColor";
            this.button_setColor.Size = new System.Drawing.Size(75, 23);
            this.button_setColor.TabIndex = 6;
            this.button_setColor.Text = "Set color";
            this.button_setColor.UseVisualStyleBackColor = true;
            this.button_setColor.Click += new System.EventHandler(this.button_setColor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_clearSteps);
            this.groupBox1.Controls.Add(this.button_addSteps);
            this.groupBox1.Controls.Add(this.button_saveBoard);
            this.groupBox1.Controls.Add(this.button_loadBoard);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 89);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board setup";
            // 
            // button_clearSteps
            // 
            this.button_clearSteps.Enabled = false;
            this.button_clearSteps.Location = new System.Drawing.Point(87, 19);
            this.button_clearSteps.Name = "button_clearSteps";
            this.button_clearSteps.Size = new System.Drawing.Size(75, 23);
            this.button_clearSteps.TabIndex = 6;
            this.button_clearSteps.Text = "Clear steps";
            this.button_clearSteps.UseVisualStyleBackColor = true;
            this.button_clearSteps.Click += new System.EventHandler(this.button_clearSteps_Click);
            // 
            // button_addSteps
            // 
            this.button_addSteps.Enabled = false;
            this.button_addSteps.Location = new System.Drawing.Point(6, 19);
            this.button_addSteps.Name = "button_addSteps";
            this.button_addSteps.Size = new System.Drawing.Size(75, 23);
            this.button_addSteps.TabIndex = 5;
            this.button_addSteps.Text = "Add steps";
            this.button_addSteps.UseVisualStyleBackColor = true;
            this.button_addSteps.Click += new System.EventHandler(this.button_addSteps_Click);
            // 
            // button_saveBoard
            // 
            this.button_saveBoard.Enabled = false;
            this.button_saveBoard.Location = new System.Drawing.Point(221, 60);
            this.button_saveBoard.Name = "button_saveBoard";
            this.button_saveBoard.Size = new System.Drawing.Size(75, 23);
            this.button_saveBoard.TabIndex = 2;
            this.button_saveBoard.Text = "Save Board";
            this.button_saveBoard.UseVisualStyleBackColor = true;
            this.button_saveBoard.Click += new System.EventHandler(this.button_saveSteps_Click);
            // 
            // button_loadBoard
            // 
            this.button_loadBoard.Enabled = false;
            this.button_loadBoard.Location = new System.Drawing.Point(140, 60);
            this.button_loadBoard.Name = "button_loadBoard";
            this.button_loadBoard.Size = new System.Drawing.Size(75, 23);
            this.button_loadBoard.TabIndex = 3;
            this.button_loadBoard.Text = "Load Board";
            this.button_loadBoard.UseVisualStyleBackColor = true;
            this.button_loadBoard.Click += new System.EventHandler(this.button_loadSteps_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(283, 610);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_state});
            this.statusStrip1.Location = new System.Drawing.Point(0, 644);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1187, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_state
            // 
            this.toolStripStatusLabel_state.Name = "toolStripStatusLabel_state";
            this.toolStripStatusLabel_state.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel_state.Text = "State: None";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 666);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer_Base);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Sequencer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer_Base.Panel1.ResumeLayout(false);
            this.splitContainer_Base.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Base)).EndInit();
            this.splitContainer_Base.ResumeLayout(false);
            this.splitContainer_LeftSideBase.Panel1.ResumeLayout(false);
            this.splitContainer_LeftSideBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_LeftSideBase)).EndInit();
            this.splitContainer_LeftSideBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_mainDisplay)).EndInit();
            this.flowLayoutPanel_CamStuff.ResumeLayout(false);
            this.tabControl_Modes.ResumeLayout(false);
            this.tabPage_Calibration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_preview)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Base;
        private System.Windows.Forms.SplitContainer splitContainer_LeftSideBase;
        private System.Windows.Forms.TabControl tabControl_Modes;
        private System.Windows.Forms.TabPage tabPage_Calibration;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_CamStuff;
        private System.Windows.Forms.ComboBox comboBox_CameraList;
        private System.Windows.Forms.Button button_startCamera;
        private Emgu.CV.UI.ImageBox imageBox_mainDisplay;
        private System.Windows.Forms.Button button_saveBoard;
        private System.Windows.Forms.Button button_loadBoard;
        private System.Windows.Forms.Button button_setPersCalib;
        private System.Windows.Forms.Button button_addSteps;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_state;
        private System.Windows.Forms.Button button_setColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_clearSteps;
        private Emgu.CV.UI.ImageBox imageBox_preview;
        private System.Windows.Forms.Integration.ElementHost cameraSettingsControl;
        private Aladaris.CameraSettingsControl cameraSettingsControl1;
    }
}

