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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.labelPitch = new System.Windows.Forms.Label();
            this.buttonPitchControl = new System.Windows.Forms.Button();
            this.buttonPitchRange = new System.Windows.Forms.Button();
            this.buttonSetHotcue = new System.Windows.Forms.Button();
            this.buttonCue = new System.Windows.Forms.Button();
            this.buttonPlayPause = new System.Windows.Forms.Button();
            this.buttonHotcue4 = new System.Windows.Forms.Button();
            this.buttonHotcue3 = new System.Windows.Forms.Button();
            this.buttonHotcue2 = new System.Windows.Forms.Button();
            this.buttonHotcue1 = new System.Windows.Forms.Button();
            this.trackBarPitch = new System.Windows.Forms.TrackBar();
            this.DisplayControl = new YoutubeMixer.Controls.DisplayControl();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPitch)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPitch
            // 
            this.labelPitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPitch.AutoSize = true;
            this.labelPitch.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPitch.Location = new System.Drawing.Point(1027, 668);
            this.labelPitch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPitch.Name = "labelPitch";
            this.labelPitch.Size = new System.Drawing.Size(38, 22);
            this.labelPitch.TabIndex = 16;
            this.labelPitch.Text = "0%";
            // 
            // buttonPitchControl
            // 
            this.buttonPitchControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPitchControl.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonPitchControl.Font = new System.Drawing.Font("Arial", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonPitchControl.Location = new System.Drawing.Point(1022, 41);
            this.buttonPitchControl.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPitchControl.Name = "buttonPitchControl";
            this.buttonPitchControl.Size = new System.Drawing.Size(84, 33);
            this.buttonPitchControl.TabIndex = 13;
            this.buttonPitchControl.Text = "PitchControl";
            this.buttonPitchControl.UseVisualStyleBackColor = false;
            this.buttonPitchControl.Click += new System.EventHandler(this.buttonPitchControl_Click);
            // 
            // buttonPitchRange
            // 
            this.buttonPitchRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPitchRange.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonPitchRange.Location = new System.Drawing.Point(1022, 0);
            this.buttonPitchRange.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPitchRange.Name = "buttonPitchRange";
            this.buttonPitchRange.Size = new System.Drawing.Size(84, 33);
            this.buttonPitchRange.TabIndex = 14;
            this.buttonPitchRange.Text = "16%";
            this.buttonPitchRange.UseVisualStyleBackColor = true;
            this.buttonPitchRange.Click += new System.EventHandler(this.buttonPitchRange_Click);
            // 
            // buttonSetHotcue
            // 
            this.buttonSetHotcue.Font = new System.Drawing.Font("Arial", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonSetHotcue.Location = new System.Drawing.Point(0, 332);
            this.buttonSetHotcue.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSetHotcue.Name = "buttonSetHotcue";
            this.buttonSetHotcue.Size = new System.Drawing.Size(103, 32);
            this.buttonSetHotcue.TabIndex = 15;
            this.buttonSetHotcue.Text = "Set Hotcue";
            this.buttonSetHotcue.UseVisualStyleBackColor = true;
            this.buttonSetHotcue.Click += new System.EventHandler(this.buttonSetHotcue_Click);
            // 
            // buttonCue
            // 
            this.buttonCue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCue.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonCue.Location = new System.Drawing.Point(0, 540);
            this.buttonCue.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCue.Name = "buttonCue";
            this.buttonCue.Size = new System.Drawing.Size(103, 75);
            this.buttonCue.TabIndex = 7;
            this.buttonCue.Text = "Cue";
            this.buttonCue.UseVisualStyleBackColor = true;
            this.buttonCue.Click += new System.EventHandler(this.buttonCue_Click);
            // 
            // buttonPlayPause
            // 
            this.buttonPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPlayPause.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonPlayPause.Location = new System.Drawing.Point(0, 622);
            this.buttonPlayPause.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPlayPause.Name = "buttonPlayPause";
            this.buttonPlayPause.Size = new System.Drawing.Size(103, 75);
            this.buttonPlayPause.TabIndex = 8;
            this.buttonPlayPause.Text = "Play";
            this.buttonPlayPause.UseVisualStyleBackColor = true;
            this.buttonPlayPause.Click += new System.EventHandler(this.buttonPlayPause_Click);
            // 
            // buttonHotcue4
            // 
            this.buttonHotcue4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonHotcue4.Location = new System.Drawing.Point(0, 249);
            this.buttonHotcue4.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHotcue4.Name = "buttonHotcue4";
            this.buttonHotcue4.Size = new System.Drawing.Size(103, 75);
            this.buttonHotcue4.TabIndex = 9;
            this.buttonHotcue4.Text = "Hotcue 4";
            this.buttonHotcue4.UseVisualStyleBackColor = true;
            this.buttonHotcue4.Click += new System.EventHandler(this.buttonHotcue4_Click);
            // 
            // buttonHotcue3
            // 
            this.buttonHotcue3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonHotcue3.Location = new System.Drawing.Point(0, 166);
            this.buttonHotcue3.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHotcue3.Name = "buttonHotcue3";
            this.buttonHotcue3.Size = new System.Drawing.Size(103, 75);
            this.buttonHotcue3.TabIndex = 10;
            this.buttonHotcue3.Text = "Hotcue 3";
            this.buttonHotcue3.UseVisualStyleBackColor = true;
            this.buttonHotcue3.Click += new System.EventHandler(this.buttonHotcue3_Click);
            // 
            // buttonHotcue2
            // 
            this.buttonHotcue2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonHotcue2.Location = new System.Drawing.Point(0, 82);
            this.buttonHotcue2.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHotcue2.Name = "buttonHotcue2";
            this.buttonHotcue2.Size = new System.Drawing.Size(103, 75);
            this.buttonHotcue2.TabIndex = 11;
            this.buttonHotcue2.Text = "Hotcue 2";
            this.buttonHotcue2.UseVisualStyleBackColor = true;
            this.buttonHotcue2.Click += new System.EventHandler(this.buttonHotcue2_Click);
            // 
            // buttonHotcue1
            // 
            this.buttonHotcue1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.buttonHotcue1.Location = new System.Drawing.Point(0, 0);
            this.buttonHotcue1.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHotcue1.Name = "buttonHotcue1";
            this.buttonHotcue1.Size = new System.Drawing.Size(103, 75);
            this.buttonHotcue1.TabIndex = 12;
            this.buttonHotcue1.Text = "Hotcue 1";
            this.buttonHotcue1.UseVisualStyleBackColor = true;
            this.buttonHotcue1.Click += new System.EventHandler(this.buttonHotcue1_Click);
            // 
            // trackBarPitch
            // 
            this.trackBarPitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarPitch.Location = new System.Drawing.Point(1018, 82);
            this.trackBarPitch.Margin = new System.Windows.Forms.Padding(4);
            this.trackBarPitch.Maximum = 10000;
            this.trackBarPitch.Name = "trackBarPitch";
            this.trackBarPitch.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarPitch.Size = new System.Drawing.Size(80, 582);
            this.trackBarPitch.TabIndex = 5;
            this.trackBarPitch.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarPitch.Value = 5000;
            this.trackBarPitch.Scroll += new System.EventHandler(this.trackBarPitch_Scroll);
            // 
            // DisplayControl
            // 
            this.DisplayControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplayControl.CurrentTime = System.TimeSpan.Parse("00:00:31.2000000");
            this.DisplayControl.Location = new System.Drawing.Point(111, 0);
            this.DisplayControl.Margin = new System.Windows.Forms.Padding(4);
            this.DisplayControl.Name = "DisplayControl";
            this.DisplayControl.Size = new System.Drawing.Size(907, 698);
            this.DisplayControl.TabIndex = 17;
            this.DisplayControl.Text = "displayControl1";
            this.DisplayControl.Title = null;
            this.DisplayControl.TotalTime = System.TimeSpan.Parse("00:01:00");
            this.DisplayControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlaybackDisplay_MouseDown);
            this.DisplayControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PlaybackDisplay_MouseMove);
            this.DisplayControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlaybackDisplay_MouseUp);
            // 
            // Deck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DisplayControl);
            this.Controls.Add(this.labelPitch);
            this.Controls.Add(this.buttonPitchControl);
            this.Controls.Add(this.buttonPitchRange);
            this.Controls.Add(this.buttonSetHotcue);
            this.Controls.Add(this.buttonCue);
            this.Controls.Add(this.buttonPlayPause);
            this.Controls.Add(this.buttonHotcue4);
            this.Controls.Add(this.buttonHotcue3);
            this.Controls.Add(this.buttonHotcue2);
            this.Controls.Add(this.buttonHotcue1);
            this.Controls.Add(this.trackBarPitch);
            this.Name = "Deck";
            this.Size = new System.Drawing.Size(1106, 698);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPitch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
