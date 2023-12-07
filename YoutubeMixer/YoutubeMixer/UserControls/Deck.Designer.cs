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
            labelPitch.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            labelPitch.Location = new Point(392, 312);
            labelPitch.Margin = new Padding(2, 0, 2, 0);
            labelPitch.Name = "labelPitch";
            labelPitch.Size = new Size(23, 14);
            labelPitch.TabIndex = 16;
            labelPitch.Text = "0%";
            // 
            // buttonPitchControl
            // 
            buttonPitchControl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPitchControl.BackColor = SystemColors.ButtonHighlight;
            buttonPitchControl.Font = new Font("Arial", 5F, FontStyle.Regular, GraphicsUnit.Point);
            buttonPitchControl.Location = new Point(389, 20);
            buttonPitchControl.Margin = new Padding(2, 2, 2, 2);
            buttonPitchControl.Name = "buttonPitchControl";
            buttonPitchControl.Size = new Size(49, 16);
            buttonPitchControl.TabIndex = 13;
            buttonPitchControl.Text = "PitchControl";
            buttonPitchControl.UseVisualStyleBackColor = false;
            buttonPitchControl.Click += buttonPitchControl_Click;
            // 
            // buttonPitchRange
            // 
            buttonPitchRange.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPitchRange.Font = new Font("Arial", 8F, FontStyle.Bold, GraphicsUnit.Point);
            buttonPitchRange.Location = new Point(389, 0);
            buttonPitchRange.Margin = new Padding(2, 2, 2, 2);
            buttonPitchRange.Name = "buttonPitchRange";
            buttonPitchRange.Size = new Size(49, 16);
            buttonPitchRange.TabIndex = 14;
            buttonPitchRange.Text = "16%";
            buttonPitchRange.UseVisualStyleBackColor = true;
            buttonPitchRange.Click += buttonPitchRange_Click;
            // 
            // buttonSetHotcue
            // 
            buttonSetHotcue.Font = new Font("Arial", 5F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSetHotcue.Location = new Point(0, 166);
            buttonSetHotcue.Margin = new Padding(2, 2, 2, 2);
            buttonSetHotcue.Name = "buttonSetHotcue";
            buttonSetHotcue.Size = new Size(60, 16);
            buttonSetHotcue.TabIndex = 15;
            buttonSetHotcue.Text = "Set Hotcue";
            buttonSetHotcue.UseVisualStyleBackColor = true;
            buttonSetHotcue.Click += buttonSetHotcue_Click;
            // 
            // buttonCue
            // 
            buttonCue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonCue.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCue.Location = new Point(0, 248);
            buttonCue.Margin = new Padding(2, 2, 2, 2);
            buttonCue.Name = "buttonCue";
            buttonCue.Size = new Size(60, 38);
            buttonCue.TabIndex = 7;
            buttonCue.Text = "Cue";
            buttonCue.UseVisualStyleBackColor = true;
            buttonCue.Click += buttonCue_Click;
            // 
            // buttonPlayPause
            // 
            buttonPlayPause.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonPlayPause.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            buttonPlayPause.Location = new Point(0, 289);
            buttonPlayPause.Margin = new Padding(2, 2, 2, 2);
            buttonPlayPause.Name = "buttonPlayPause";
            buttonPlayPause.Size = new Size(60, 38);
            buttonPlayPause.TabIndex = 8;
            buttonPlayPause.Text = "Play";
            buttonPlayPause.UseVisualStyleBackColor = true;
            buttonPlayPause.Click += buttonPlayPause_Click;
            // 
            // buttonHotcue4
            // 
            buttonHotcue4.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            buttonHotcue4.Location = new Point(0, 124);
            buttonHotcue4.Margin = new Padding(2, 2, 2, 2);
            buttonHotcue4.Name = "buttonHotcue4";
            buttonHotcue4.Size = new Size(60, 38);
            buttonHotcue4.TabIndex = 9;
            buttonHotcue4.Text = "Hotcue 4";
            buttonHotcue4.UseVisualStyleBackColor = true;
            buttonHotcue4.Click += buttonHotcue4_Click;
            // 
            // buttonHotcue3
            // 
            buttonHotcue3.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            buttonHotcue3.Location = new Point(0, 83);
            buttonHotcue3.Margin = new Padding(2, 2, 2, 2);
            buttonHotcue3.Name = "buttonHotcue3";
            buttonHotcue3.Size = new Size(60, 38);
            buttonHotcue3.TabIndex = 10;
            buttonHotcue3.Text = "Hotcue 3";
            buttonHotcue3.UseVisualStyleBackColor = true;
            buttonHotcue3.Click += buttonHotcue3_Click;
            // 
            // buttonHotcue2
            // 
            buttonHotcue2.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            buttonHotcue2.Location = new Point(0, 41);
            buttonHotcue2.Margin = new Padding(2, 2, 2, 2);
            buttonHotcue2.Name = "buttonHotcue2";
            buttonHotcue2.Size = new Size(60, 38);
            buttonHotcue2.TabIndex = 11;
            buttonHotcue2.Text = "Hotcue 2";
            buttonHotcue2.UseVisualStyleBackColor = true;
            buttonHotcue2.Click += buttonHotcue2_Click;
            // 
            // buttonHotcue1
            // 
            buttonHotcue1.Font = new Font("Arial", 8F, FontStyle.Regular, GraphicsUnit.Point);
            buttonHotcue1.Location = new Point(0, 0);
            buttonHotcue1.Margin = new Padding(2, 2, 2, 2);
            buttonHotcue1.Name = "buttonHotcue1";
            buttonHotcue1.Size = new Size(60, 38);
            buttonHotcue1.TabIndex = 12;
            buttonHotcue1.Text = "Hotcue 1";
            buttonHotcue1.UseVisualStyleBackColor = true;
            buttonHotcue1.Click += buttonHotcue1_Click;
            // 
            // trackBarPitch
            // 
            trackBarPitch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            trackBarPitch.Location = new Point(387, 41);
            trackBarPitch.Margin = new Padding(2, 2, 2, 2);
            trackBarPitch.Maximum = 10000;
            trackBarPitch.Name = "trackBarPitch";
            trackBarPitch.Orientation = Orientation.Vertical;
            trackBarPitch.Size = new Size(45, 269);
            trackBarPitch.TabIndex = 5;
            trackBarPitch.TickStyle = TickStyle.Both;
            trackBarPitch.Value = 5000;
            trackBarPitch.Scroll += trackBarPitch_Scroll;
            // 
            // DisplayControl
            // 
            DisplayControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DisplayControl.CurrentTime = TimeSpan.Parse("00:00:31.2000000");
            DisplayControl.Location = new Point(65, 0);
            DisplayControl.Margin = new Padding(2, 2, 2, 2);
            DisplayControl.Name = "DisplayControl";
            DisplayControl.Size = new Size(322, 327);
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
            AutoScaleDimensions = new SizeF(7F, 15F);
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
            Margin = new Padding(2, 2, 2, 2);
            Name = "Deck";
            Size = new Size(438, 327);
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
