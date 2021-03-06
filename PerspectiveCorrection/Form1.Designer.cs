﻿namespace PerspectiveCorrection {
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.checkBox_drawRefPoints = new System.Windows.Forms.CheckBox();
            this.checkBox_perspectivecorrect = new System.Windows.Forms.CheckBox();
            this.button_start = new System.Windows.Forms.Button();
            this.label_position = new System.Windows.Forms.Label();
            this.pictureBox_zoomPreview = new System.Windows.Forms.PictureBox();
            this.imageBox_display = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_zoomPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_display)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.imageBox_display);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(952, 588);
            this.splitContainer1.SplitterDistance = 782;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.checkBox_drawRefPoints);
            this.splitContainer2.Panel1.Controls.Add(this.checkBox_perspectivecorrect);
            this.splitContainer2.Panel1.Controls.Add(this.button_start);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label_position);
            this.splitContainer2.Panel2.Controls.Add(this.pictureBox_zoomPreview);
            this.splitContainer2.Size = new System.Drawing.Size(166, 588);
            this.splitContainer2.SplitterDistance = 441;
            this.splitContainer2.TabIndex = 0;
            // 
            // checkBox_drawRefPoints
            // 
            this.checkBox_drawRefPoints.AutoSize = true;
            this.checkBox_drawRefPoints.Location = new System.Drawing.Point(16, 107);
            this.checkBox_drawRefPoints.Name = "checkBox_drawRefPoints";
            this.checkBox_drawRefPoints.Size = new System.Drawing.Size(130, 17);
            this.checkBox_drawRefPoints.TabIndex = 2;
            this.checkBox_drawRefPoints.Text = "Draw reference points";
            this.checkBox_drawRefPoints.UseVisualStyleBackColor = true;
            this.checkBox_drawRefPoints.CheckedChanged += new System.EventHandler(this.checkBox_drawRefPoints_CheckedChanged);
            // 
            // checkBox_perspectivecorrect
            // 
            this.checkBox_perspectivecorrect.AutoSize = true;
            this.checkBox_perspectivecorrect.Location = new System.Drawing.Point(16, 130);
            this.checkBox_perspectivecorrect.Name = "checkBox_perspectivecorrect";
            this.checkBox_perspectivecorrect.Size = new System.Drawing.Size(118, 17);
            this.checkBox_perspectivecorrect.TabIndex = 1;
            this.checkBox_perspectivecorrect.Text = "Correct perspective";
            this.checkBox_perspectivecorrect.UseVisualStyleBackColor = true;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(52, 12);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 48);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "&Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click_1);
            // 
            // label_position
            // 
            this.label_position.AutoSize = true;
            this.label_position.BackColor = System.Drawing.Color.Transparent;
            this.label_position.Font = new System.Drawing.Font("Monospac821 BT", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_position.Location = new System.Drawing.Point(14, 10);
            this.label_position.Name = "label_position";
            this.label_position.Size = new System.Drawing.Size(35, 10);
            this.label_position.TabIndex = 1;
            this.label_position.Text = "xx, yy";
            // 
            // pictureBox_zoomPreview
            // 
            this.pictureBox_zoomPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_zoomPreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_zoomPreview.Name = "pictureBox_zoomPreview";
            this.pictureBox_zoomPreview.Size = new System.Drawing.Size(166, 143);
            this.pictureBox_zoomPreview.TabIndex = 0;
            this.pictureBox_zoomPreview.TabStop = false;
            this.pictureBox_zoomPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_zoomPreview_Paint);
            // 
            // imageBox_display
            // 
            this.imageBox_display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox_display.Location = new System.Drawing.Point(0, 0);
            this.imageBox_display.Name = "imageBox_display";
            this.imageBox_display.Size = new System.Drawing.Size(782, 588);
            this.imageBox_display.TabIndex = 2;
            this.imageBox_display.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 588);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Perspective Correction";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_zoomPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_display)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.PictureBox pictureBox_zoomPreview;
        private System.Windows.Forms.Label label_position;
        private System.Windows.Forms.CheckBox checkBox_perspectivecorrect;
        private System.Windows.Forms.CheckBox checkBox_drawRefPoints;
        private Emgu.CV.UI.ImageBox imageBox_display;
    }
}

