namespace YoutubeMixer.UserControls
{
    partial class Deck
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
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            labelPitch = new Label();
            buttonPitchControl = new Button();
            buttonPitchRange = new Button();
            buttonSetHotcue = new Button();
            buttonCue = new Button();
            buttonPlayPause = new Button();
            buttonHotcue4 = new Button();
            buttonHotcue3 = new Button();
            buttonHotcue2 = new Button();
            buttonHotcue1 = new Button();
            trackBarPitch = new TrackBar();
            DisplayControl = new Controls.DisplayControl();
            ((System.ComponentModel.ISupportInitialize)trackBarPitch).BeginInit();
            SuspendLayout();
            // 
            // labelPitch
            // 
            labelPitch.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            labelPitch.AutoSize = true;
            labelPitch.Font = new Font("Arial", 8F);
            labelPitch.Location = new Point(828, 624);
            labelPitch.Name = "labelPitch";
            labelPitch.Size = new Size(31, 18);
            labelPitch.TabIndex = 16;
            labelPitch.Text = "0%";
            // 
            // buttonPitchControl
            // 
            buttonPitchControl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPitchControl.BackColor = SystemColors.ButtonHighlight;
            buttonPitchControl.Font = new Font("Arial", 5F);
            buttonPitchControl.Location = new Point(824, 33);
            buttonPitchControl.Name = "buttonPitchControl";
            buttonPitchControl.Size = new Size(70, 27);
            buttonPitchControl.TabIndex = 13;
            buttonPitchControl.Text = "PitchControl";
            buttonPitchControl.UseVisualStyleBackColor = false;
            buttonPitchControl.Click += buttonPitchControl_Click;
            // 
            // buttonPitchRange
            // 
            buttonPitchRange.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPitchRange.Font = new Font("Arial", 8F, FontStyle.Bold);
            buttonPitchRange.Location = new Point(824, 0);
            buttonPitchRange.Name = "buttonPitchRange";
            buttonPitchRange.Size = new Size(70, 27);
            buttonPitchRange.TabIndex = 14;
            buttonPitchRange.Text = "16%";
            buttonPitchRange.UseVisualStyleBackColor = true;
            buttonPitchRange.Click += buttonPitchRange_Click;
            // 
            // buttonSetHotcue
            // 
            buttonSetHotcue.Font = new Font("Arial", 5F);
            buttonSetHotcue.Location = new Point(0, 277);
            buttonSetHotcue.Name = "buttonSetHotcue";
            buttonSetHotcue.Size = new Size(86, 27);
            buttonSetHotcue.TabIndex = 15;
            buttonSetHotcue.Text = "Set Hotcue";
            buttonSetHotcue.UseVisualStyleBackColor = true;
            buttonSetHotcue.Click += buttonSetHotcue_Click;
            // 
            // buttonCue
            // 
            buttonCue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonCue.Font = new Font("Arial", 8F);
            buttonCue.Location = new Point(0, 517);
            buttonCue.Name = "buttonCue";
            buttonCue.Size = new Size(86, 63);
            buttonCue.TabIndex = 7;
            buttonCue.Text = "Cue";
            buttonCue.UseVisualStyleBackColor = true;
            buttonCue.Click += buttonCue_Click;
            // 
            // buttonPlayPause
            // 
            buttonPlayPause.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonPlayPause.Font = new Font("Arial", 8F);
            buttonPlayPause.Location = new Point(0, 586);
            buttonPlayPause.Name = "buttonPlayPause";
            buttonPlayPause.Size = new Size(86, 63);
            buttonPlayPause.TabIndex = 8;
            buttonPlayPause.Text = "Play";
            buttonPlayPause.UseVisualStyleBackColor = true;
            buttonPlayPause.Click += buttonPlayPause_Click;
            // 
            // buttonHotcue4
            // 
            buttonHotcue4.Font = new Font("Arial", 8F);
            buttonHotcue4.Location = new Point(0, 207);
            buttonHotcue4.Name = "buttonHotcue4";
            buttonHotcue4.Size = new Size(86, 63);
            buttonHotcue4.TabIndex = 9;
            buttonHotcue4.Text = "Hotcue 4";
            buttonHotcue4.UseVisualStyleBackColor = true;
            buttonHotcue4.Click += buttonHotcue4_Click;
            // 
            // buttonHotcue3
            // 
            buttonHotcue3.Font = new Font("Arial", 8F);
            buttonHotcue3.Location = new Point(0, 138);
            buttonHotcue3.Name = "buttonHotcue3";
            buttonHotcue3.Size = new Size(86, 63);
            buttonHotcue3.TabIndex = 10;
            buttonHotcue3.Text = "Hotcue 3";
            buttonHotcue3.UseVisualStyleBackColor = true;
            buttonHotcue3.Click += buttonHotcue3_Click;
            // 
            // buttonHotcue2
            // 
            buttonHotcue2.Font = new Font("Arial", 8F);
            buttonHotcue2.Location = new Point(0, 68);
            buttonHotcue2.Name = "buttonHotcue2";
            buttonHotcue2.Size = new Size(86, 63);
            buttonHotcue2.TabIndex = 11;
            buttonHotcue2.Text = "Hotcue 2";
            buttonHotcue2.UseVisualStyleBackColor = true;
            buttonHotcue2.Click += buttonHotcue2_Click;
            // 
            // buttonHotcue1
            // 
            buttonHotcue1.Font = new Font("Arial", 8F);
            buttonHotcue1.Location = new Point(0, 0);
            buttonHotcue1.Name = "buttonHotcue1";
            buttonHotcue1.Size = new Size(86, 63);
            buttonHotcue1.TabIndex = 12;
            buttonHotcue1.Text = "Hotcue 1";
            buttonHotcue1.UseVisualStyleBackColor = true;
            buttonHotcue1.Click += buttonHotcue1_Click;
            // 
            // trackBarPitch
            // 
            trackBarPitch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            trackBarPitch.Location = new Point(821, 68);
            trackBarPitch.Maximum = 10000;
            trackBarPitch.Name = "trackBarPitch";
            trackBarPitch.Orientation = Orientation.Vertical;
            trackBarPitch.Size = new Size(69, 552);
            trackBarPitch.TabIndex = 5;
            trackBarPitch.TickStyle = TickStyle.Both;
            trackBarPitch.Value = 5000;
            trackBarPitch.Scroll += trackBarPitch_Scroll;
            // 
            // DisplayControl
            // 
            DisplayControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DisplayControl.CurrentTime = TimeSpan.Parse("00:00:31.2000000");
            DisplayControl.Location = new Point(93, 0);
            DisplayControl.Name = "DisplayControl";
            DisplayControl.Size = new Size(728, 649);
            DisplayControl.TabIndex = 17;
            DisplayControl.Text = "displayControl1";
            DisplayControl.Title = null;
            DisplayControl.TotalTime = TimeSpan.Parse("00:01:00");
            DisplayControl.MouseDown += PlaybackDisplay_MouseDown;
            DisplayControl.MouseMove += PlaybackDisplay_MouseMove;
            DisplayControl.MouseUp += PlaybackDisplay_MouseUp;
            // 
            // Deck
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DisplayControl);
            Controls.Add(labelPitch);
            Controls.Add(buttonPitchControl);
            Controls.Add(buttonPitchRange);
            Controls.Add(buttonSetHotcue);
            Controls.Add(buttonCue);
            Controls.Add(buttonPlayPause);
            Controls.Add(buttonHotcue4);
            Controls.Add(buttonHotcue3);
            Controls.Add(buttonHotcue2);
            Controls.Add(buttonHotcue1);
            Controls.Add(trackBarPitch);
            Name = "Deck";
            Size = new Size(894, 649);
            ((System.ComponentModel.ISupportInitialize)trackBarPitch).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label labelPitch;
        private Button buttonPitchControl;
        private Button buttonPitchRange;
        private Button buttonSetHotcue;
        private Button buttonCue;
        private Button buttonPlayPause;
        private Button buttonHotcue4;
        private Button buttonHotcue3;
        private Button buttonHotcue2;
        private Button buttonHotcue1;
        private TrackBar trackBarPitch;
        private Controls.DisplayControl DisplayControl;
    }
}
