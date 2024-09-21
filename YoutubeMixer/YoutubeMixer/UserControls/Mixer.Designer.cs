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
            FaderCross = new TrackBar();
            LeftMixerChannel = new MixerChannel();
            RightMixerChannel = new MixerChannel();
            ((System.ComponentModel.ISupportInitialize)FaderCross).BeginInit();
            SuspendLayout();
            // 
            // FaderCross
            // 
            FaderCross.Location = new Point(1, 252);
            FaderCross.Margin = new Padding(2, 2, 2, 2);
            FaderCross.Maximum = 100;
            FaderCross.Name = "FaderCross";
            FaderCross.Size = new Size(102, 45);
            FaderCross.TabIndex = 0;
            FaderCross.Value = 50;
            // 
            // LeftMixerChannel
            // 
            LeftMixerChannel.AudioSource = null;
            LeftMixerChannel.Location = new Point(2, 2);
            LeftMixerChannel.Margin = new Padding(1, 1, 1, 1);
            LeftMixerChannel.Name = "LeftMixerChannel";
            LeftMixerChannel.Size = new Size(50, 247);
            LeftMixerChannel.TabIndex = 1;
            // 
            // RightMixerChannel
            // 
            RightMixerChannel.AudioSource = null;
            RightMixerChannel.Location = new Point(54, 2);
            RightMixerChannel.Margin = new Padding(1, 1, 1, 1);
            RightMixerChannel.Name = "RightMixerChannel";
            RightMixerChannel.Size = new Size(50, 247);
            RightMixerChannel.TabIndex = 2;
            // 
            // Mixer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(RightMixerChannel);
            Controls.Add(LeftMixerChannel);
            Controls.Add(FaderCross);
            Margin = new Padding(2, 2, 2, 2);
            Name = "Mixer";
            Size = new Size(104, 276);
            Load += Mixer_Load;
            Resize += Mixer_Resize;
            ((System.ComponentModel.ISupportInitialize)FaderCross).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TrackBar FaderCross;
        public MixerChannel LeftMixerChannel;
        public MixerChannel RightMixerChannel;
    }
}
