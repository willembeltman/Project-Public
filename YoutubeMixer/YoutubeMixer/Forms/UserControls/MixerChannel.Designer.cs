using YoutubeMixer.Forms.Controls;

namespace YoutubeMixer.UserControls
{
    partial class MixerChannel
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
            VuMeter = new VuMeter();
            EqBassControl = new EqualizerControl();
            EqMidControl = new EqualizerControl();
            EqHighControl = new EqualizerControl();
            Fader = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)Fader).BeginInit();
            SuspendLayout();
            // 
            // VuMeter
            // 
            VuMeter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            VuMeter.AudioSource = null;
            VuMeter.Location = new Point(5, 140);
            VuMeter.Margin = new Padding(3, 2, 3, 2);
            VuMeter.Name = "VuMeter";
            VuMeter.Size = new Size(7, 250);
            VuMeter.TabIndex = 7;
            VuMeter.Text = "vuMeter1";
            // 
            // EqBassControl
            // 
            EqBassControl.BackColor = Color.White;
            EqBassControl.Location = new Point(3, 87);
            EqBassControl.Margin = new Padding(3, 2, 3, 2);
            EqBassControl.MaximumSize = new Size(175, 150);
            EqBassControl.MinimumSize = new Size(44, 38);
            EqBassControl.Name = "EqBassControl";
            EqBassControl.Range = 24D;
            EqBassControl.Size = new Size(44, 38);
            EqBassControl.TabIndex = 4;
            EqBassControl.Text = "dialControl1";
            EqBassControl.Value = 0D;
            EqBassControl.ValueChanged += EqControl_ValueChanged;
            // 
            // EqMidControl
            // 
            EqMidControl.BackColor = Color.White;
            EqMidControl.Location = new Point(3, 44);
            EqMidControl.Margin = new Padding(3, 2, 3, 2);
            EqMidControl.MaximumSize = new Size(175, 150);
            EqMidControl.MinimumSize = new Size(44, 38);
            EqMidControl.Name = "EqMidControl";
            EqMidControl.Range = 24D;
            EqMidControl.Size = new Size(44, 38);
            EqMidControl.TabIndex = 5;
            EqMidControl.Text = "dialControl1";
            EqMidControl.Value = 0D;
            EqMidControl.ValueChanged += EqControl_ValueChanged;
            // 
            // EqHighControl
            // 
            EqHighControl.BackColor = Color.White;
            EqHighControl.Location = new Point(3, 2);
            EqHighControl.Margin = new Padding(3, 2, 3, 2);
            EqHighControl.MaximumSize = new Size(175, 150);
            EqHighControl.MinimumSize = new Size(44, 38);
            EqHighControl.Name = "EqHighControl";
            EqHighControl.Range = 24D;
            EqHighControl.Size = new Size(44, 38);
            EqHighControl.TabIndex = 6;
            EqHighControl.Text = "dialControl1";
            EqHighControl.Value = 0D;
            EqHighControl.ValueChanged += EqControl_ValueChanged;
            // 
            // Fader
            // 
            Fader.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            Fader.Location = new Point(12, 128);
            Fader.Margin = new Padding(2);
            Fader.Maximum = 100;
            Fader.Name = "Fader";
            Fader.Orientation = Orientation.Vertical;
            Fader.Size = new Size(45, 270);
            Fader.TabIndex = 3;
            Fader.TickStyle = TickStyle.Both;
            Fader.Value = 100;
            Fader.Scroll += Fader_Scroll;
            // 
            // MixerChannel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(VuMeter);
            Controls.Add(EqBassControl);
            Controls.Add(EqMidControl);
            Controls.Add(EqHighControl);
            Controls.Add(Fader);
            Margin = new Padding(2);
            Name = "MixerChannel";
            Size = new Size(64, 398);
            Load += MixerChannel_Load;
            Resize += MixerChannel_Resize;
            ((System.ComponentModel.ISupportInitialize)Fader).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private VuMeter VuMeter;
        private EqualizerControl EqBassControl;
        private EqualizerControl EqMidControl;
        private EqualizerControl EqHighControl;
        private TrackBar Fader;
    }
}
