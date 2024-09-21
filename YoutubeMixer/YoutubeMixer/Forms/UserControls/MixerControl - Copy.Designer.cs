namespace YoutubeMixer.UserControls
{
    partial class MixerControl
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
            this.FaderRight = new System.Windows.Forms.TrackBar();
            this.EqHighControlRight = new YoutubeMixer.Controls.EqualizerControl();
            this.EqMidControlRight = new YoutubeMixer.Controls.EqualizerControl();
            this.EqBassControlRight = new YoutubeMixer.Controls.EqualizerControl();
            this.vuMeterRight = new YoutubeMixer.Controls.VuMeter();
            this.EqHighControlLeft = new YoutubeMixer.Controls.EqualizerControl();
            this.EqMidControlLeft = new YoutubeMixer.Controls.EqualizerControl();
            this.EqBassControlLeft = new YoutubeMixer.Controls.EqualizerControl();
            this.vuMeterLeft = new YoutubeMixer.Controls.VuMeter();
            this.FaderLeft = new System.Windows.Forms.TrackBar();
            this.FaderCross = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.FaderRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaderLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaderCross)).BeginInit();
            this.SuspendLayout();
            // 
            // FaderRight
            // 
            this.FaderRight.Location = new System.Drawing.Point(109, 261);
            this.FaderRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FaderRight.Maximum = 100;
            this.FaderRight.Name = "FaderRight";
            this.FaderRight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.FaderRight.Size = new System.Drawing.Size(80, 244);
            this.FaderRight.TabIndex = 0;
            this.FaderRight.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.FaderRight.Value = 100;
            this.FaderRight.Scroll += new System.EventHandler(this.FaderRight_Scroll);
            // 
            // EqHighControlRight
            // 
            this.EqHighControlRight.BackColor = System.Drawing.Color.White;
            this.EqHighControlRight.Location = new System.Drawing.Point(94, 8);
            this.EqHighControlRight.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqHighControlRight.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqHighControlRight.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqHighControlRight.Name = "EqHighControlRight";
            this.EqHighControlRight.Range = 24D;
            this.EqHighControlRight.Size = new System.Drawing.Size(75, 76);
            this.EqHighControlRight.TabIndex = 1;
            this.EqHighControlRight.Text = "dialControl1";
            this.EqHighControlRight.Value = 0D;
            this.EqHighControlRight.ValueChanged += new System.EventHandler(this.EqControlRight_ValueChanged);
            // 
            // EqMidControlRight
            // 
            this.EqMidControlRight.BackColor = System.Drawing.Color.White;
            this.EqMidControlRight.Location = new System.Drawing.Point(94, 93);
            this.EqMidControlRight.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqMidControlRight.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqMidControlRight.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqMidControlRight.Name = "EqMidControlRight";
            this.EqMidControlRight.Range = 24D;
            this.EqMidControlRight.Size = new System.Drawing.Size(75, 76);
            this.EqMidControlRight.TabIndex = 1;
            this.EqMidControlRight.Text = "dialControl1";
            this.EqMidControlRight.Value = 0D;
            this.EqMidControlRight.ValueChanged += new System.EventHandler(this.EqControlRight_ValueChanged);
            // 
            // EqBassControlRight
            // 
            this.EqBassControlRight.BackColor = System.Drawing.Color.White;
            this.EqBassControlRight.Location = new System.Drawing.Point(94, 178);
            this.EqBassControlRight.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqBassControlRight.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqBassControlRight.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqBassControlRight.Name = "EqBassControlRight";
            this.EqBassControlRight.Range = 24D;
            this.EqBassControlRight.Size = new System.Drawing.Size(75, 76);
            this.EqBassControlRight.TabIndex = 1;
            this.EqBassControlRight.Text = "dialControl1";
            this.EqBassControlRight.Value = 0D;
            this.EqBassControlRight.ValueChanged += new System.EventHandler(this.EqControlRight_ValueChanged);
            // 
            // vuMeterRight
            // 
            this.vuMeterRight.Location = new System.Drawing.Point(93, 284);
            this.vuMeterRight.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.vuMeterRight.Name = "vuMeterRight";
            this.vuMeterRight.Size = new System.Drawing.Size(12, 199);
            this.vuMeterRight.TabIndex = 3;
            this.vuMeterRight.Text = "vuMeter2";
            this.vuMeterRight.Value = 0D;
            // 
            // EqHighControlLeft
            // 
            this.EqHighControlLeft.BackColor = System.Drawing.Color.White;
            this.EqHighControlLeft.Location = new System.Drawing.Point(8, 8);
            this.EqHighControlLeft.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqHighControlLeft.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqHighControlLeft.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqHighControlLeft.Name = "EqHighControlLeft";
            this.EqHighControlLeft.Range = 24D;
            this.EqHighControlLeft.Size = new System.Drawing.Size(75, 76);
            this.EqHighControlLeft.TabIndex = 1;
            this.EqHighControlLeft.Text = "dialControl1";
            this.EqHighControlLeft.Value = 0D;
            this.EqHighControlLeft.ValueChanged += new System.EventHandler(this.EqControlLeft_ValueChanged);
            // 
            // EqMidControlLeft
            // 
            this.EqMidControlLeft.BackColor = System.Drawing.Color.White;
            this.EqMidControlLeft.Location = new System.Drawing.Point(8, 93);
            this.EqMidControlLeft.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqMidControlLeft.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqMidControlLeft.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqMidControlLeft.Name = "EqMidControlLeft";
            this.EqMidControlLeft.Range = 24D;
            this.EqMidControlLeft.Size = new System.Drawing.Size(75, 76);
            this.EqMidControlLeft.TabIndex = 1;
            this.EqMidControlLeft.Text = "dialControl1";
            this.EqMidControlLeft.Value = 0D;
            this.EqMidControlLeft.ValueChanged += new System.EventHandler(this.EqControlLeft_ValueChanged);
            // 
            // EqBassControlLeft
            // 
            this.EqBassControlLeft.BackColor = System.Drawing.Color.White;
            this.EqBassControlLeft.Location = new System.Drawing.Point(8, 178);
            this.EqBassControlLeft.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.EqBassControlLeft.MaximumSize = new System.Drawing.Size(300, 300);
            this.EqBassControlLeft.MinimumSize = new System.Drawing.Size(75, 76);
            this.EqBassControlLeft.Name = "EqBassControlLeft";
            this.EqBassControlLeft.Range = 24D;
            this.EqBassControlLeft.Size = new System.Drawing.Size(75, 76);
            this.EqBassControlLeft.TabIndex = 1;
            this.EqBassControlLeft.Text = "dialControl1";
            this.EqBassControlLeft.Value = 0D;
            this.EqBassControlLeft.ValueChanged += new System.EventHandler(this.EqControlLeft_ValueChanged);
            // 
            // vuMeterLeft
            // 
            this.vuMeterLeft.Location = new System.Drawing.Point(11, 284);
            this.vuMeterLeft.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.vuMeterLeft.Name = "vuMeterLeft";
            this.vuMeterLeft.Size = new System.Drawing.Size(12, 199);
            this.vuMeterLeft.TabIndex = 2;
            this.vuMeterLeft.Text = "vuMeter1";
            this.vuMeterLeft.Value = 0D;
            // 
            // FaderLeft
            // 
            this.FaderLeft.Location = new System.Drawing.Point(24, 261);
            this.FaderLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FaderLeft.Maximum = 100;
            this.FaderLeft.Name = "FaderLeft";
            this.FaderLeft.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.FaderLeft.Size = new System.Drawing.Size(80, 244);
            this.FaderLeft.TabIndex = 0;
            this.FaderLeft.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.FaderLeft.Value = 100;
            this.FaderLeft.Scroll += new System.EventHandler(this.FaderLeft_Scroll);
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
            this.FaderCross.Scroll += new System.EventHandler(this.FaderCross_Scroll);
            // 
            // MixerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vuMeterRight);
            this.Controls.Add(this.vuMeterLeft);
            this.Controls.Add(this.EqBassControlRight);
            this.Controls.Add(this.EqBassControlLeft);
            this.Controls.Add(this.EqMidControlRight);
            this.Controls.Add(this.EqMidControlLeft);
            this.Controls.Add(this.EqHighControlRight);
            this.Controls.Add(this.EqHighControlLeft);
            this.Controls.Add(this.FaderRight);
            this.Controls.Add(this.FaderLeft);
            this.Controls.Add(this.FaderCross);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MixerControl";
            this.Size = new System.Drawing.Size(178, 552);
            ((System.ComponentModel.ISupportInitialize)(this.FaderRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaderLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FaderCross)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TrackBar FaderRight;
        private Controls.EqualizerControl EqHighControlRight;
        private Controls.EqualizerControl EqMidControlRight;
        private Controls.EqualizerControl EqBassControlRight;
        private Controls.VuMeter vuMeterRight;
        private Controls.EqualizerControl EqHighControlLeft;
        private Controls.EqualizerControl EqMidControlLeft;
        private Controls.EqualizerControl EqBassControlLeft;
        private Controls.VuMeter vuMeterLeft;
        private TrackBar FaderLeft;
        private TrackBar FaderCross;
    }
}
