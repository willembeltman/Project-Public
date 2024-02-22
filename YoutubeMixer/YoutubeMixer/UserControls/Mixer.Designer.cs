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
            FaderCross.Location = new Point(1, 420);
            FaderCross.Maximum = 100;
            FaderCross.Name = "FaderCross";
            FaderCross.Size = new Size(146, 69);
            FaderCross.TabIndex = 0;
            FaderCross.Value = 50;
            // 
            // LeftMixerChannel
            // 
            LeftMixerChannel.Controller = null;
            LeftMixerChannel.Location = new Point(3, 3);
            LeftMixerChannel.Margin = new Padding(1, 2, 1, 2);
            LeftMixerChannel.Mixer = null;
            LeftMixerChannel.Name = "LeftMixerChannel";
            LeftMixerChannel.Size = new Size(72, 412);
            LeftMixerChannel.TabIndex = 1;
            // 
            // RightMixerChannel
            // 
            RightMixerChannel.Controller = null;
            RightMixerChannel.Location = new Point(77, 3);
            RightMixerChannel.Margin = new Padding(1, 2, 1, 2);
            RightMixerChannel.Mixer = null;
            RightMixerChannel.Name = "RightMixerChannel";
            RightMixerChannel.Size = new Size(71, 412);
            RightMixerChannel.TabIndex = 2;
            // 
            // Mixer
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(RightMixerChannel);
            Controls.Add(LeftMixerChannel);
            Controls.Add(FaderCross);
            Name = "Mixer";
            Size = new Size(149, 460);
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
