namespace YoutubeMixer.UserControls
{
    partial class Mixer
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
            this.FaderCross = new System.Windows.Forms.TrackBar();
            this.LeftMixerChannel = new YoutubeMixer.UserControls.MixerChannel();
            this.RightMixerChannel = new YoutubeMixer.UserControls.MixerChannel();
            ((System.ComponentModel.ISupportInitialize)(this.FaderCross)).BeginInit();
            this.SuspendLayout();
            // 
            // FaderCross
            // 
            this.FaderCross.Location = new System.Drawing.Point(1, 504);
            this.FaderCross.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FaderCross.Maximum = 100;
            this.FaderCross.Name = "FaderCross";
            this.FaderCross.Size = new System.Drawing.Size(175, 80);
            this.FaderCross.TabIndex = 0;
            this.FaderCross.Value = 50;
            // 
            // LeftMixerChannel
            // 
            this.LeftMixerChannel.Controller = null;
            this.LeftMixerChannel.Location = new System.Drawing.Point(3, 3);
            this.LeftMixerChannel.Name = "LeftMixerChannel";
            this.LeftMixerChannel.Size = new System.Drawing.Size(86, 494);
            this.LeftMixerChannel.TabIndex = 1;
            // 
            // RightMixerChannel
            // 
            this.RightMixerChannel.Controller = null;
            this.RightMixerChannel.Location = new System.Drawing.Point(92, 3);
            this.RightMixerChannel.Name = "RightMixerChannel";
            this.RightMixerChannel.Size = new System.Drawing.Size(86, 494);
            this.RightMixerChannel.TabIndex = 2;
            // 
            // MixerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RightMixerChannel);
            this.Controls.Add(this.LeftMixerChannel);
            this.Controls.Add(this.FaderCross);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MixerControl";
            this.Size = new System.Drawing.Size(178, 552);
            ((System.ComponentModel.ISupportInitialize)(this.FaderCross)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TrackBar FaderCross;
        public MixerChannel LeftMixerChannel;
        public MixerChannel RightMixerChannel;
    }
}
