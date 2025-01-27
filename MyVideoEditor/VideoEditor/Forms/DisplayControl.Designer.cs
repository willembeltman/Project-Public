namespace VideoEditor.Forms
{
    partial class DisplayControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            videoControl = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)videoControl).BeginInit();
            SuspendLayout();
            // 
            // videoControl
            // 
            videoControl.BackColor = Color.Black;
            videoControl.Location = new Point(142, 3);
            videoControl.Name = "videoControl";
            videoControl.Size = new Size(462, 374);
            videoControl.TabIndex = 0;
            videoControl.TabStop = false;
            // 
            // DisplayControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(videoControl);
            Name = "DisplayControl";
            Size = new Size(725, 380);
            Load += DisplayControl_Load;
            Resize += DisplayControl_Resize;
            ((System.ComponentModel.ISupportInitialize)videoControl).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox videoControl;
    }
}
