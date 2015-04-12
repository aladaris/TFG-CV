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
            this.tabControl_Modes = new System.Windows.Forms.TabControl();
            this.tabPage_Calibration = new System.Windows.Forms.TabPage();
            this.checkBox_perspectRectangle = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_addSteps = new System.Windows.Forms.CheckBox();
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
            this.SuspendLayout();
            // 
            // splitContainer_Base
            // 
            this.splitContainer_Base.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer_Base.Size = new System.Drawing.Size(1159, 636);
            this.splitContainer_Base.SplitterDistance = 861;
            this.splitContainer_Base.TabIndex = 0;
            // 
            // splitContainer_LeftSideBase
            // 
            this.splitContainer_LeftSideBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_LeftSideBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
            this.splitContainer_LeftSideBase.Size = new System.Drawing.Size(861, 636);
            this.splitContainer_LeftSideBase.SplitterDistance = 539;
            this.splitContainer_LeftSideBase.TabIndex = 0;
            // 
            // imageBox_mainDisplay
            // 
            this.imageBox_mainDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox_mainDisplay.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_mainDisplay.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.imageBox_mainDisplay.Location = new System.Drawing.Point(0, 0);
            this.imageBox_mainDisplay.Name = "imageBox_mainDisplay";
            this.imageBox_mainDisplay.Size = new System.Drawing.Size(861, 539);
            this.imageBox_mainDisplay.TabIndex = 2;
            this.imageBox_mainDisplay.TabStop = false;
            // 
            // flowLayoutPanel_CamStuff
            // 
            this.flowLayoutPanel_CamStuff.Controls.Add(this.comboBox_CameraList);
            this.flowLayoutPanel_CamStuff.Controls.Add(this.button_startCamera);
            this.flowLayoutPanel_CamStuff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_CamStuff.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel_CamStuff.Name = "flowLayoutPanel_CamStuff";
            this.flowLayoutPanel_CamStuff.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel_CamStuff.Size = new System.Drawing.Size(861, 93);
            this.flowLayoutPanel_CamStuff.TabIndex = 0;
            // 
            // comboBox_CameraList
            // 
            this.comboBox_CameraList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_CameraList.FormattingEnabled = true;
            this.comboBox_CameraList.Location = new System.Drawing.Point(8, 18);
            this.comboBox_CameraList.Name = "comboBox_CameraList";
            this.comboBox_CameraList.Size = new System.Drawing.Size(191, 21);
            this.comboBox_CameraList.TabIndex = 1;
            // 
            // button_startCamera
            // 
            this.button_startCamera.Location = new System.Drawing.Point(205, 8);
            this.button_startCamera.Name = "button_startCamera";
            this.button_startCamera.Size = new System.Drawing.Size(75, 42);
            this.button_startCamera.TabIndex = 0;
            this.button_startCamera.Text = "Start Camera";
            this.button_startCamera.UseVisualStyleBackColor = true;
            this.button_startCamera.Click += new System.EventHandler(this.button_startCamera_Click);
            // 
            // tabControl_Modes
            // 
            this.tabControl_Modes.Controls.Add(this.tabPage_Calibration);
            this.tabControl_Modes.Controls.Add(this.tabPage2);
            this.tabControl_Modes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Modes.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Modes.Name = "tabControl_Modes";
            this.tabControl_Modes.SelectedIndex = 0;
            this.tabControl_Modes.Size = new System.Drawing.Size(294, 636);
            this.tabControl_Modes.TabIndex = 0;
            // 
            // tabPage_Calibration
            // 
            this.tabPage_Calibration.Controls.Add(this.checkBox_addSteps);
            this.tabPage_Calibration.Controls.Add(this.checkBox_perspectRectangle);
            this.tabPage_Calibration.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Calibration.Name = "tabPage_Calibration";
            this.tabPage_Calibration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Calibration.Size = new System.Drawing.Size(286, 610);
            this.tabPage_Calibration.TabIndex = 0;
            this.tabPage_Calibration.Text = "Calibration";
            this.tabPage_Calibration.UseVisualStyleBackColor = true;
            // 
            // checkBox_perspectRectangle
            // 
            this.checkBox_perspectRectangle.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_perspectRectangle.AutoSize = true;
            this.checkBox_perspectRectangle.Enabled = false;
            this.checkBox_perspectRectangle.Location = new System.Drawing.Point(6, 6);
            this.checkBox_perspectRectangle.Name = "checkBox_perspectRectangle";
            this.checkBox_perspectRectangle.Size = new System.Drawing.Size(181, 23);
            this.checkBox_perspectRectangle.TabIndex = 0;
            this.checkBox_perspectRectangle.Text = "Set perspective correction polygon";
            this.checkBox_perspectRectangle.UseVisualStyleBackColor = true;
            this.checkBox_perspectRectangle.CheckedChanged += new System.EventHandler(this.checkBox_perspectRectangle_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(286, 610);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_addSteps
            // 
            this.checkBox_addSteps.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_addSteps.AutoSize = true;
            this.checkBox_addSteps.Enabled = false;
            this.checkBox_addSteps.Location = new System.Drawing.Point(6, 35);
            this.checkBox_addSteps.Name = "checkBox_addSteps";
            this.checkBox_addSteps.Size = new System.Drawing.Size(66, 23);
            this.checkBox_addSteps.TabIndex = 1;
            this.checkBox_addSteps.Text = "Add Steps";
            this.checkBox_addSteps.UseVisualStyleBackColor = true;
            this.checkBox_addSteps.CheckedChanged += new System.EventHandler(this.checkBox_addSteps_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 636);
            this.Controls.Add(this.splitContainer_Base);
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
            this.tabPage_Calibration.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.CheckBox checkBox_perspectRectangle;
        private System.Windows.Forms.CheckBox checkBox_addSteps;
    }
}

