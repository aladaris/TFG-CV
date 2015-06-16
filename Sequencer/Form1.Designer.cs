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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_fpsIn = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.p_colorPreview_Track1 = new System.Windows.Forms.Panel();
            this.button_setColor_Track1 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.p_colorPreview_Track2 = new System.Windows.Forms.Panel();
            this.button_setColor_Track2 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.p_colorPreview_Track3 = new System.Windows.Forms.Panel();
            this.button_setColor_Track3 = new System.Windows.Forms.Button();
            this.tabPage_sequencer = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button_StartInstrument = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_clearSteps = new System.Windows.Forms.Button();
            this.button_addSteps = new System.Windows.Forms.Button();
            this.button_saveBoard = new System.Windows.Forms.Button();
            this.button_loadBoard = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_figures = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDown_blancaMax = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown_blancaMin = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown_negraMax = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown_negraMin = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_TODO_Cambiar_por_simbolo_correcto = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown_corcheaMin = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_corcheaMax = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_state = new System.Windows.Forms.ToolStripStatusLabel();
            this.cb_FlipH = new System.Windows.Forms.CheckBox();
            this.cb_FlipV = new System.Windows.Forms.CheckBox();
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
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fpsIn)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage_sequencer.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel_figures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_blancaMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_blancaMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_negraMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_negraMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_corcheaMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_corcheaMax)).BeginInit();
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
            this.flowLayoutPanel_CamStuff.Controls.Add(this.cb_FlipH);
            this.flowLayoutPanel_CamStuff.Controls.Add(this.cb_FlipV);
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
            this.tabControl_Modes.Controls.Add(this.tabPage_sequencer);
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
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 168);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Visual configuration";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(296, 149);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown_fpsIn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(296, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "FPS in:";
            // 
            // numericUpDown_fpsIn
            // 
            this.numericUpDown_fpsIn.Location = new System.Drawing.Point(50, 3);
            this.numericUpDown_fpsIn.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDown_fpsIn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_fpsIn.Name = "numericUpDown_fpsIn";
            this.numericUpDown_fpsIn.Size = new System.Drawing.Size(34, 20);
            this.numericUpDown_fpsIn.TabIndex = 7;
            this.numericUpDown_fpsIn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_fpsIn.ValueChanged += new System.EventHandler(this.numericUpDown_fpsIn_ValueChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox6, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox7, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(290, 114);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.p_colorPreview_Track1);
            this.groupBox5.Controls.Add(this.button_setColor_Track1);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(86, 51);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Track 1";
            // 
            // p_colorPreview_Track1
            // 
            this.p_colorPreview_Track1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.p_colorPreview_Track1.Location = new System.Drawing.Point(66, 19);
            this.p_colorPreview_Track1.Name = "p_colorPreview_Track1";
            this.p_colorPreview_Track1.Size = new System.Drawing.Size(12, 23);
            this.p_colorPreview_Track1.TabIndex = 10;
            // 
            // button_setColor_Track1
            // 
            this.button_setColor_Track1.Location = new System.Drawing.Point(6, 19);
            this.button_setColor_Track1.Name = "button_setColor_Track1";
            this.button_setColor_Track1.Size = new System.Drawing.Size(57, 23);
            this.button_setColor_Track1.TabIndex = 6;
            this.button_setColor_Track1.Text = "Set color";
            this.button_setColor_Track1.UseVisualStyleBackColor = true;
            this.button_setColor_Track1.Click += new System.EventHandler(this.button_setColor_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.p_colorPreview_Track2);
            this.groupBox6.Controls.Add(this.button_setColor_Track2);
            this.groupBox6.Location = new System.Drawing.Point(99, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(86, 51);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Track 2";
            // 
            // p_colorPreview_Track2
            // 
            this.p_colorPreview_Track2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.p_colorPreview_Track2.Location = new System.Drawing.Point(66, 19);
            this.p_colorPreview_Track2.Name = "p_colorPreview_Track2";
            this.p_colorPreview_Track2.Size = new System.Drawing.Size(12, 23);
            this.p_colorPreview_Track2.TabIndex = 10;
            // 
            // button_setColor_Track2
            // 
            this.button_setColor_Track2.Location = new System.Drawing.Point(6, 19);
            this.button_setColor_Track2.Name = "button_setColor_Track2";
            this.button_setColor_Track2.Size = new System.Drawing.Size(57, 23);
            this.button_setColor_Track2.TabIndex = 6;
            this.button_setColor_Track2.Text = "Set color";
            this.button_setColor_Track2.UseVisualStyleBackColor = true;
            this.button_setColor_Track2.Click += new System.EventHandler(this.button_setColor_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.p_colorPreview_Track3);
            this.groupBox7.Controls.Add(this.button_setColor_Track3);
            this.groupBox7.Location = new System.Drawing.Point(195, 3);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(86, 51);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Track 3";
            // 
            // p_colorPreview_Track3
            // 
            this.p_colorPreview_Track3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.p_colorPreview_Track3.Location = new System.Drawing.Point(66, 19);
            this.p_colorPreview_Track3.Name = "p_colorPreview_Track3";
            this.p_colorPreview_Track3.Size = new System.Drawing.Size(12, 23);
            this.p_colorPreview_Track3.TabIndex = 10;
            // 
            // button_setColor_Track3
            // 
            this.button_setColor_Track3.Location = new System.Drawing.Point(6, 19);
            this.button_setColor_Track3.Name = "button_setColor_Track3";
            this.button_setColor_Track3.Size = new System.Drawing.Size(57, 23);
            this.button_setColor_Track3.TabIndex = 6;
            this.button_setColor_Track3.Text = "Set color";
            this.button_setColor_Track3.UseVisualStyleBackColor = true;
            this.button_setColor_Track3.Click += new System.EventHandler(this.button_setColor_Click);
            // 
            // tabPage_sequencer
            // 
            this.tabPage_sequencer.Controls.Add(this.groupBox8);
            this.tabPage_sequencer.Controls.Add(this.groupBox3);
            this.tabPage_sequencer.Location = new System.Drawing.Point(4, 22);
            this.tabPage_sequencer.Name = "tabPage_sequencer";
            this.tabPage_sequencer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_sequencer.Size = new System.Drawing.Size(311, 640);
            this.tabPage_sequencer.TabIndex = 1;
            this.tabPage_sequencer.Text = "Sequencer";
            this.tabPage_sequencer.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.button_StartInstrument);
            this.groupBox8.Location = new System.Drawing.Point(6, 260);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(299, 158);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Sequencer";
            // 
            // button_StartInstrument
            // 
            this.button_StartInstrument.Location = new System.Drawing.Point(9, 19);
            this.button_StartInstrument.Name = "button_StartInstrument";
            this.button_StartInstrument.Size = new System.Drawing.Size(75, 35);
            this.button_StartInstrument.TabIndex = 0;
            this.button_StartInstrument.Text = "Start instrument";
            this.button_StartInstrument.UseVisualStyleBackColor = true;
            this.button_StartInstrument.Click += new System.EventHandler(this.button_StartInstrument_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 248);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Configuration parameters";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_clearSteps);
            this.groupBox1.Controls.Add(this.button_addSteps);
            this.groupBox1.Controls.Add(this.button_saveBoard);
            this.groupBox1.Controls.Add(this.button_loadBoard);
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 89);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board setup";
            // 
            // button_clearSteps
            // 
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
            this.button_saveBoard.Location = new System.Drawing.Point(209, 60);
            this.button_saveBoard.Name = "button_saveBoard";
            this.button_saveBoard.Size = new System.Drawing.Size(75, 23);
            this.button_saveBoard.TabIndex = 2;
            this.button_saveBoard.Text = "Save Board";
            this.button_saveBoard.UseVisualStyleBackColor = true;
            this.button_saveBoard.Click += new System.EventHandler(this.button_saveSteps_Click);
            // 
            // button_loadBoard
            // 
            this.button_loadBoard.Location = new System.Drawing.Point(128, 60);
            this.button_loadBoard.Name = "button_loadBoard";
            this.button_loadBoard.Size = new System.Drawing.Size(75, 23);
            this.button_loadBoard.TabIndex = 3;
            this.button_loadBoard.Text = "Load Board";
            this.button_loadBoard.UseVisualStyleBackColor = true;
            this.button_loadBoard.Click += new System.EventHandler(this.button_loadSteps_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel_figures);
            this.groupBox4.Location = new System.Drawing.Point(6, 114);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(287, 114);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Figures areas";
            // 
            // tableLayoutPanel_figures
            // 
            this.tableLayoutPanel_figures.ColumnCount = 5;
            this.tableLayoutPanel_figures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_figures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_figures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_figures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_figures.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel_figures.Controls.Add(this.numericUpDown_blancaMax, 4, 2);
            this.tableLayoutPanel_figures.Controls.Add(this.label9, 3, 2);
            this.tableLayoutPanel_figures.Controls.Add(this.numericUpDown_blancaMin, 2, 2);
            this.tableLayoutPanel_figures.Controls.Add(this.label8, 1, 2);
            this.tableLayoutPanel_figures.Controls.Add(this.numericUpDown_negraMax, 4, 1);
            this.tableLayoutPanel_figures.Controls.Add(this.label7, 3, 1);
            this.tableLayoutPanel_figures.Controls.Add(this.numericUpDown_negraMin, 2, 1);
            this.tableLayoutPanel_figures.Controls.Add(this.label6, 1, 1);
            this.tableLayoutPanel_figures.Controls.Add(this.label5, 3, 0);
            this.tableLayoutPanel_figures.Controls.Add(this.label_TODO_Cambiar_por_simbolo_correcto, 0, 2);
            this.tableLayoutPanel_figures.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel_figures.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel_figures.Controls.Add(this.numericUpDown_corcheaMin, 2, 0);
            this.tableLayoutPanel_figures.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel_figures.Controls.Add(this.numericUpDown_corcheaMax, 4, 0);
            this.tableLayoutPanel_figures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_figures.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel_figures.Name = "tableLayoutPanel_figures";
            this.tableLayoutPanel_figures.RowCount = 3;
            this.tableLayoutPanel_figures.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_figures.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_figures.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_figures.Size = new System.Drawing.Size(281, 95);
            this.tableLayoutPanel_figures.TabIndex = 0;
            // 
            // numericUpDown_blancaMax
            // 
            this.numericUpDown_blancaMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_blancaMax.Location = new System.Drawing.Point(227, 65);
            this.numericUpDown_blancaMax.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_blancaMax.Name = "numericUpDown_blancaMax";
            this.numericUpDown_blancaMax.Size = new System.Drawing.Size(51, 20);
            this.numericUpDown_blancaMax.TabIndex = 12;
            this.numericUpDown_blancaMax.Value = new decimal(new int[] {
            3500,
            0,
            0,
            0});
            this.numericUpDown_blancaMax.ValueChanged += new System.EventHandler(this.numericUpDown_blancaMax_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(171, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 33);
            this.label9.TabIndex = 11;
            this.label9.Text = "Max. Area";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown_blancaMin
            // 
            this.numericUpDown_blancaMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_blancaMin.Location = new System.Drawing.Point(115, 65);
            this.numericUpDown_blancaMin.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_blancaMin.Name = "numericUpDown_blancaMin";
            this.numericUpDown_blancaMin.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_blancaMin.TabIndex = 10;
            this.numericUpDown_blancaMin.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown_blancaMin.ValueChanged += new System.EventHandler(this.numericUpDown_blancaMin_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(59, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 33);
            this.label8.TabIndex = 9;
            this.label8.Text = "Min. Area";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown_negraMax
            // 
            this.numericUpDown_negraMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_negraMax.Location = new System.Drawing.Point(227, 34);
            this.numericUpDown_negraMax.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_negraMax.Name = "numericUpDown_negraMax";
            this.numericUpDown_negraMax.Size = new System.Drawing.Size(51, 20);
            this.numericUpDown_negraMax.TabIndex = 8;
            this.numericUpDown_negraMax.Value = new decimal(new int[] {
            750,
            0,
            0,
            0});
            this.numericUpDown_negraMax.ValueChanged += new System.EventHandler(this.numericUpDown_negraMax_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(171, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 31);
            this.label7.TabIndex = 7;
            this.label7.Text = "Max. Area";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown_negraMin
            // 
            this.numericUpDown_negraMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_negraMin.Location = new System.Drawing.Point(115, 34);
            this.numericUpDown_negraMin.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_negraMin.Name = "numericUpDown_negraMin";
            this.numericUpDown_negraMin.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_negraMin.TabIndex = 6;
            this.numericUpDown_negraMin.Value = new decimal(new int[] {
            550,
            0,
            0,
            0});
            this.numericUpDown_negraMin.ValueChanged += new System.EventHandler(this.numericUpDown_negraMin_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(59, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 31);
            this.label6.TabIndex = 5;
            this.label6.Text = "Min. Area";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(171, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 31);
            this.label5.TabIndex = 4;
            this.label5.Text = "Max. Area";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_TODO_Cambiar_por_simbolo_correcto
            // 
            this.label_TODO_Cambiar_por_simbolo_correcto.AutoSize = true;
            this.label_TODO_Cambiar_por_simbolo_correcto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_TODO_Cambiar_por_simbolo_correcto.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_TODO_Cambiar_por_simbolo_correcto.Location = new System.Drawing.Point(3, 62);
            this.label_TODO_Cambiar_por_simbolo_correcto.Name = "label_TODO_Cambiar_por_simbolo_correcto";
            this.label_TODO_Cambiar_por_simbolo_correcto.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label_TODO_Cambiar_por_simbolo_correcto.Size = new System.Drawing.Size(50, 33);
            this.label_TODO_Cambiar_por_simbolo_correcto.TabIndex = 2;
            this.label_TODO_Cambiar_por_simbolo_correcto.Text = "♭";
            this.label_TODO_Cambiar_por_simbolo_correcto.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "♪";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 31);
            this.label4.TabIndex = 2;
            this.label4.Text = "♩";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // numericUpDown_corcheaMin
            // 
            this.numericUpDown_corcheaMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_corcheaMin.Location = new System.Drawing.Point(115, 3);
            this.numericUpDown_corcheaMin.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_corcheaMin.Name = "numericUpDown_corcheaMin";
            this.numericUpDown_corcheaMin.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_corcheaMin.TabIndex = 3;
            this.numericUpDown_corcheaMin.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDown_corcheaMin.ValueChanged += new System.EventHandler(this.numericUpDown_corcheaMin_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(59, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 31);
            this.label3.TabIndex = 1;
            this.label3.Text = "Min. Area";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown_corcheaMax
            // 
            this.numericUpDown_corcheaMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown_corcheaMax.Location = new System.Drawing.Point(227, 3);
            this.numericUpDown_corcheaMax.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_corcheaMax.Name = "numericUpDown_corcheaMax";
            this.numericUpDown_corcheaMax.Size = new System.Drawing.Size(51, 20);
            this.numericUpDown_corcheaMax.TabIndex = 3;
            this.numericUpDown_corcheaMax.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numericUpDown_corcheaMax.ValueChanged += new System.EventHandler(this.numericUpDown_corcheaMax_ValueChanged);
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
            // cb_FlipH
            // 
            this.cb_FlipH.AutoSize = true;
            this.cb_FlipH.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cb_FlipH.Location = new System.Drawing.Point(367, 8);
            this.cb_FlipH.Name = "cb_FlipH";
            this.cb_FlipH.Size = new System.Drawing.Size(75, 31);
            this.cb_FlipH.TabIndex = 5;
            this.cb_FlipH.Text = "Flip horizontal";
            this.cb_FlipH.UseVisualStyleBackColor = true;
            this.cb_FlipH.CheckedChanged += new System.EventHandler(this.cb_FlipH_CheckedChanged);
            // 
            // cb_FlipV
            // 
            this.cb_FlipV.AutoSize = true;
            this.cb_FlipV.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cb_FlipV.Location = new System.Drawing.Point(448, 8);
            this.cb_FlipV.Name = "cb_FlipV";
            this.cb_FlipV.Size = new System.Drawing.Size(64, 31);
            this.cb_FlipV.TabIndex = 5;
            this.cb_FlipV.Text = "Flip vertical";
            this.cb_FlipV.UseVisualStyleBackColor = true;
            this.cb_FlipV.CheckedChanged += new System.EventHandler(this.cb_FlipV_CheckedChanged);
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
            this.flowLayoutPanel_CamStuff.PerformLayout();
            this.tabControl_Modes.ResumeLayout(false);
            this.tabPage_Calibration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_preview)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_fpsIn)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabPage_sequencer.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel_figures.ResumeLayout(false);
            this.tableLayoutPanel_figures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_blancaMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_blancaMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_negraMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_negraMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_corcheaMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_corcheaMax)).EndInit();
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
        private System.Windows.Forms.TabPage tabPage_sequencer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_CamStuff;
        private System.Windows.Forms.ComboBox comboBox_CameraList;
        private System.Windows.Forms.Button button_startCamera;
        private Emgu.CV.UI.ImageBox imageBox_mainDisplay;
        private System.Windows.Forms.Button button_setPersCalib;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_state;
        private System.Windows.Forms.Button button_setColor_Track1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Emgu.CV.UI.ImageBox imageBox_preview;
        private System.Windows.Forms.Integration.ElementHost cameraSettingsControl;
        private Aladaris.CameraSettingsControl cameraSettingsControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_fpsIn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_figures;
        private System.Windows.Forms.NumericUpDown numericUpDown_blancaMax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDown_blancaMin;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown_negraMax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown_negraMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_TODO_Cambiar_por_simbolo_correcto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_corcheaMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown_corcheaMax;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_clearSteps;
        private System.Windows.Forms.Button button_addSteps;
        private System.Windows.Forms.Button button_saveBoard;
        private System.Windows.Forms.Button button_loadBoard;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Panel p_colorPreview_Track3;
        private System.Windows.Forms.Button button_setColor_Track3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Panel p_colorPreview_Track2;
        private System.Windows.Forms.Button button_setColor_Track2;
        private System.Windows.Forms.Panel p_colorPreview_Track1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button button_StartInstrument;
        private System.Windows.Forms.CheckBox cb_FlipH;
        private System.Windows.Forms.CheckBox cb_FlipV;
    }
}

