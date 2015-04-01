namespace Tests
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer_Base = new System.Windows.Forms.SplitContainer();
            this.pictureBox_display = new System.Windows.Forms.PictureBox();
            this.splitContainer_Controles_butones = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel_controles = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar_canThres = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar_canThresLink = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar_Threshold = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.trackBar_minLineWidth = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBar_gapBetweenLines = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Base)).BeginInit();
            this.splitContainer_Base.Panel1.SuspendLayout();
            this.splitContainer_Base.Panel2.SuspendLayout();
            this.splitContainer_Base.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_display)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Controles_butones)).BeginInit();
            this.splitContainer_Controles_butones.Panel1.SuspendLayout();
            this.splitContainer_Controles_butones.Panel2.SuspendLayout();
            this.splitContainer_Controles_butones.SuspendLayout();
            this.flowLayoutPanel_controles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_canThres)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_canThresLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Threshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_minLineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_gapBetweenLines)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer_Base
            // 
            this.splitContainer_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Base.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Base.Name = "splitContainer_Base";
            this.splitContainer_Base.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Base.Panel1
            // 
            this.splitContainer_Base.Panel1.Controls.Add(this.pictureBox_display);
            // 
            // splitContainer_Base.Panel2
            // 
            this.splitContainer_Base.Panel2.Controls.Add(this.splitContainer_Controles_butones);
            this.splitContainer_Base.Size = new System.Drawing.Size(742, 521);
            this.splitContainer_Base.SplitterDistance = 398;
            this.splitContainer_Base.TabIndex = 3;
            // 
            // pictureBox_display
            // 
            this.pictureBox_display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_display.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_display.Name = "pictureBox_display";
            this.pictureBox_display.Size = new System.Drawing.Size(742, 398);
            this.pictureBox_display.TabIndex = 0;
            this.pictureBox_display.TabStop = false;
            // 
            // splitContainer_Controles_butones
            // 
            this.splitContainer_Controles_butones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Controles_butones.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Controles_butones.Name = "splitContainer_Controles_butones";
            // 
            // splitContainer_Controles_butones.Panel1
            // 
            this.splitContainer_Controles_butones.Panel1.Controls.Add(this.flowLayoutPanel_controles);
            // 
            // splitContainer_Controles_butones.Panel2
            // 
            this.splitContainer_Controles_butones.Panel2.Controls.Add(this.button_start);
            this.splitContainer_Controles_butones.Size = new System.Drawing.Size(742, 119);
            this.splitContainer_Controles_butones.SplitterDistance = 638;
            this.splitContainer_Controles_butones.TabIndex = 2;
            // 
            // flowLayoutPanel_controles
            // 
            this.flowLayoutPanel_controles.Controls.Add(this.label1);
            this.flowLayoutPanel_controles.Controls.Add(this.trackBar_canThres);
            this.flowLayoutPanel_controles.Controls.Add(this.label2);
            this.flowLayoutPanel_controles.Controls.Add(this.trackBar_canThresLink);
            this.flowLayoutPanel_controles.Controls.Add(this.label3);
            this.flowLayoutPanel_controles.Controls.Add(this.trackBar_Threshold);
            this.flowLayoutPanel_controles.Controls.Add(this.label4);
            this.flowLayoutPanel_controles.Controls.Add(this.trackBar_minLineWidth);
            this.flowLayoutPanel_controles.Controls.Add(this.label5);
            this.flowLayoutPanel_controles.Controls.Add(this.trackBar_gapBetweenLines);
            this.flowLayoutPanel_controles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_controles.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel_controles.Name = "flowLayoutPanel_controles";
            this.flowLayoutPanel_controles.Size = new System.Drawing.Size(638, 119);
            this.flowLayoutPanel_controles.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Canny Threshold";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_canThres
            // 
            this.trackBar_canThres.Location = new System.Drawing.Point(69, 3);
            this.trackBar_canThres.Maximum = 300;
            this.trackBar_canThres.Minimum = 1;
            this.trackBar_canThres.Name = "trackBar_canThres";
            this.trackBar_canThres.Size = new System.Drawing.Size(104, 45);
            this.trackBar_canThres.TabIndex = 0;
            this.trackBar_canThres.Value = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(179, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 48);
            this.label2.TabIndex = 3;
            this.label2.Text = "Canny Threshold Linking";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_canThresLink
            // 
            this.trackBar_canThresLink.Location = new System.Drawing.Point(245, 3);
            this.trackBar_canThresLink.Maximum = 300;
            this.trackBar_canThresLink.Minimum = 1;
            this.trackBar_canThresLink.Name = "trackBar_canThresLink";
            this.trackBar_canThresLink.Size = new System.Drawing.Size(104, 45);
            this.trackBar_canThresLink.TabIndex = 2;
            this.trackBar_canThresLink.Value = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(355, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Threshold";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_Threshold
            // 
            this.trackBar_Threshold.Location = new System.Drawing.Point(421, 3);
            this.trackBar_Threshold.Maximum = 500;
            this.trackBar_Threshold.Minimum = 1;
            this.trackBar_Threshold.Name = "trackBar_Threshold";
            this.trackBar_Threshold.Size = new System.Drawing.Size(170, 45);
            this.trackBar_Threshold.TabIndex = 4;
            this.trackBar_Threshold.Value = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 29);
            this.label4.TabIndex = 7;
            this.label4.Text = "min Line Width";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(12, 28);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 48);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // trackBar_minLineWidth
            // 
            this.trackBar_minLineWidth.Location = new System.Drawing.Point(69, 54);
            this.trackBar_minLineWidth.Maximum = 500;
            this.trackBar_minLineWidth.Minimum = 1;
            this.trackBar_minLineWidth.Name = "trackBar_minLineWidth";
            this.trackBar_minLineWidth.Size = new System.Drawing.Size(170, 45);
            this.trackBar_minLineWidth.TabIndex = 6;
            this.trackBar_minLineWidth.Value = 1;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(245, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 48);
            this.label5.TabIndex = 9;
            this.label5.Text = "Gap Between Lines";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_gapBetweenLines
            // 
            this.trackBar_gapBetweenLines.Location = new System.Drawing.Point(311, 54);
            this.trackBar_gapBetweenLines.Maximum = 500;
            this.trackBar_gapBetweenLines.Minimum = 1;
            this.trackBar_gapBetweenLines.Name = "trackBar_gapBetweenLines";
            this.trackBar_gapBetweenLines.Size = new System.Drawing.Size(170, 45);
            this.trackBar_gapBetweenLines.TabIndex = 8;
            this.trackBar_gapBetweenLines.Value = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 521);
            this.Controls.Add(this.splitContainer_Base);
            this.Name = "Form1";
            this.Text = "Tests";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer_Base.Panel1.ResumeLayout(false);
            this.splitContainer_Base.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Base)).EndInit();
            this.splitContainer_Base.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_display)).EndInit();
            this.splitContainer_Controles_butones.Panel1.ResumeLayout(false);
            this.splitContainer_Controles_butones.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Controles_butones)).EndInit();
            this.splitContainer_Controles_butones.ResumeLayout(false);
            this.flowLayoutPanel_controles.ResumeLayout(false);
            this.flowLayoutPanel_controles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_canThres)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_canThresLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Threshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_minLineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_gapBetweenLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Base;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.PictureBox pictureBox_display;
        private System.Windows.Forms.SplitContainer splitContainer_Controles_butones;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_controles;
        private System.Windows.Forms.TrackBar trackBar_canThres;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar_canThresLink;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar_Threshold;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBar_minLineWidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBar_gapBetweenLines;
    }
}

