namespace MyVideoEditor
{
    partial class TimelineControl
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
            components = new System.ComponentModel.Container();
            TimelineScrollBar = new HScrollBar();
            TimelineVideoPanel = new Panel();
            TimelineAudioPanel = new Panel();
            TimelineDisplayPanel = new Panel();
            buttonBackward = new Button();
            buttonForward = new Button();
            buttonPlay = new Button();
            buttonPause = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            TimelineDisplayPanel.SuspendLayout();
            SuspendLayout();
            // 
            // TimelineScrollBar
            // 
            TimelineScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TimelineScrollBar.Location = new Point(0, 890);
            TimelineScrollBar.Name = "TimelineScrollBar";
            TimelineScrollBar.Size = new Size(853, 32);
            TimelineScrollBar.TabIndex = 4;
            // 
            // TimelineVideoPanel
            // 
            TimelineVideoPanel.AllowDrop = true;
            TimelineVideoPanel.BackColor = SystemColors.ControlLightLight;
            TimelineVideoPanel.Location = new Point(0, 425);
            TimelineVideoPanel.Margin = new Padding(2);
            TimelineVideoPanel.Name = "TimelineVideoPanel";
            TimelineVideoPanel.Size = new Size(853, 230);
            TimelineVideoPanel.TabIndex = 5;
            TimelineVideoPanel.DragDrop += TimelinePanel_DragDrop;
            TimelineVideoPanel.DragEnter += TimelinePanel_DragEnter;
            TimelineVideoPanel.Resize += TimelinePanel_Resize;
            // 
            // TimelineAudioPanel
            // 
            TimelineAudioPanel.AllowDrop = true;
            TimelineAudioPanel.BackColor = SystemColors.ControlLightLight;
            TimelineAudioPanel.Location = new Point(0, 690);
            TimelineAudioPanel.Margin = new Padding(2);
            TimelineAudioPanel.Name = "TimelineAudioPanel";
            TimelineAudioPanel.Size = new Size(853, 180);
            TimelineAudioPanel.TabIndex = 6;
            // 
            // TimelineDisplayPanel
            // 
            TimelineDisplayPanel.BackColor = SystemColors.ControlText;
            TimelineDisplayPanel.Controls.Add(buttonBackward);
            TimelineDisplayPanel.Controls.Add(buttonForward);
            TimelineDisplayPanel.Controls.Add(buttonPlay);
            TimelineDisplayPanel.Controls.Add(buttonPause);
            TimelineDisplayPanel.ForeColor = SystemColors.Control;
            TimelineDisplayPanel.Location = new Point(3, 3);
            TimelineDisplayPanel.Name = "TimelineDisplayPanel";
            TimelineDisplayPanel.Size = new Size(847, 394);
            TimelineDisplayPanel.TabIndex = 7;
            // 
            // buttonBackward
            // 
            buttonBackward.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonBackward.ForeColor = SystemColors.ControlText;
            buttonBackward.Location = new Point(287, 320);
            buttonBackward.Name = "buttonBackward";
            buttonBackward.Size = new Size(63, 53);
            buttonBackward.TabIndex = 0;
            buttonBackward.Text = "⏮️";
            buttonBackward.UseVisualStyleBackColor = true;
            buttonBackward.Click += buttonBackward_Click;
            // 
            // buttonForward
            // 
            buttonForward.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonForward.ForeColor = SystemColors.ControlText;
            buttonForward.Location = new Point(494, 320);
            buttonForward.Name = "buttonForward";
            buttonForward.Size = new Size(63, 53);
            buttonForward.TabIndex = 0;
            buttonForward.Text = "⏭️";
            buttonForward.UseVisualStyleBackColor = true;
            buttonForward.Click += buttonForward_Click;
            // 
            // buttonPlay
            // 
            buttonPlay.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonPlay.ForeColor = SystemColors.ControlText;
            buttonPlay.Location = new Point(356, 320);
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new Size(63, 53);
            buttonPlay.TabIndex = 0;
            buttonPlay.Text = "▶️";
            buttonPlay.UseVisualStyleBackColor = true;
            buttonPlay.Click += buttonPlay_Click;
            // 
            // buttonPause
            // 
            buttonPause.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonPause.ForeColor = SystemColors.ControlText;
            buttonPause.Location = new Point(425, 320);
            buttonPause.Name = "buttonPause";
            buttonPause.Size = new Size(63, 53);
            buttonPause.TabIndex = 0;
            buttonPause.Text = "⏸️";
            buttonPause.UseVisualStyleBackColor = true;
            buttonPause.Click += buttonPause_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Tick += timer1_Tick;
            // 
            // TimelineControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(TimelineDisplayPanel);
            Controls.Add(TimelineAudioPanel);
            Controls.Add(TimelineVideoPanel);
            Controls.Add(TimelineScrollBar);
            Margin = new Padding(2);
            Name = "TimelineControl";
            Size = new Size(853, 924);
            Load += TimelineControl_Load;
            Paint += TimelineControl_Paint;
            TimelineDisplayPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private HScrollBar TimelineScrollBar;
        private Panel TimelineVideoPanel;
        private Panel TimelineAudioPanel;
        private Panel TimelineDisplayPanel;
        private Button buttonForward;
        private Button buttonPlay;
        private Button buttonPause;
        private Button buttonBackward;
        private System.Windows.Forms.Timer timer1;
    }
}
