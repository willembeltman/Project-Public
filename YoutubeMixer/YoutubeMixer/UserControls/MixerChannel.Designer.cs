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
            this.VuMeter = new YoutubeMixer.Controls.VuMeter();
            this.EqBassControl = new YoutubeMixer.Controls.EqualizerControl();
            this.EqMidControl = new YoutubeMixer.Controls.EqualizerControl();
            this.EqHighControl = new YoutubeMixer.Controls.EqualizerControl();
            this.Fader = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.Fader)).BeginInit();
            this.SuspendLayout();
            // 
            // VuMeter
            // 
            this.VuMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.VuMeter.Location = new System.Drawing.Point(8, 280);
            this.VuMeter.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.VuMeter.Name = "VuMeter";
            this.VuMeter.Size = new System.Drawing.Size(12, 203);
            this.VuMeter.TabIndex = 7;
            this.VuMeter.Text = "vuMeter1";
            this.VuMeter.Value = 0D;
            // 
            // EqBassControl
            // 
            this.EqBassControl.BackColor = System.Drawing.Color.White;
            this.EqBassControl.Location = new System.Drawing.Point(5, 174);
            this.EqBassControl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqBassControl.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqBassControl.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqBassControl.Name = "EqBassControl";
            this.EqBassControl.Range = 24D;
            this.EqBassControl.Size = new System.Drawing.Size(75, 76);
            this.EqBassControl.TabIndex = 4;
            this.EqBassControl.Text = "dialControl1";
            this.EqBassControl.Value = 0D;
            this.EqBassControl.ValueChanged += new System.EventHandler(this.EqControl_ValueChanged);
            // 
            // EqMidControl
            // 
            this.EqMidControl.BackColor = System.Drawing.Color.White;
            this.EqMidControl.Location = new System.Drawing.Point(5, 89);
            this.EqMidControl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqMidControl.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqMidControl.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqMidControl.Name = "EqMidControl";
            this.EqMidControl.Range = 24D;
            this.EqMidControl.Size = new System.Drawing.Size(75, 76);
            this.EqMidControl.TabIndex = 5;
            this.EqMidControl.Text = "dialControl1";
            this.EqMidControl.Value = 0D;
            this.EqMidControl.ValueChanged += new System.EventHandler(this.EqControl_ValueChanged);
            // 
            // EqHighControl
            // 
            this.EqHighControl.BackColor = System.Drawing.Color.White;
            this.EqHighControl.Location = new System.Drawing.Point(5, 4);
            this.EqHighControl.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqHighControl.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqHighControl.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqHighControl.Name = "EqHighControl";
            this.EqHighControl.Range = 24D;
            this.EqHighControl.Size = new System.Drawing.Size(75, 76);
            this.EqHighControl.TabIndex = 6;
            this.EqHighControl.Text = "dialControl1";
            this.EqHighControl.Value = 0D;
            this.EqHighControl.ValueChanged += new System.EventHandler(this.EqControl_ValueChanged);
            // 
            // Fader
            // 
            this.Fader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Fader.Location = new System.Drawing.Point(21, 257);
            this.Fader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Fader.Maximum = 100;
            this.Fader.Name = "Fader";
            this.Fader.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Fader.Size = new System.Drawing.Size(80, 248);
            this.Fader.TabIndex = 3;
            this.Fader.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.Fader.Value = 100;
            this.Fader.Scroll += new System.EventHandler(this.Fader_Scroll);
            // 
            // MixerChannelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VuMeter);
            this.Controls.Add(this.EqBassControl);
            this.Controls.Add(this.EqMidControl);
            this.Controls.Add(this.EqHighControl);
            this.Controls.Add(this.Fader);
            this.Name = "MixerChannelControl";
            this.Size = new System.Drawing.Size(87, 500);
            ((System.ComponentModel.ISupportInitialize)(this.Fader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.VuMeter VuMeter;
        private Controls.EqualizerControl EqBassControl;
        private Controls.EqualizerControl EqMidControl;
        private Controls.EqualizerControl EqHighControl;
        private TrackBar Fader;
    }
}
