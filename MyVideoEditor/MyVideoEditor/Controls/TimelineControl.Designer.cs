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
            TimelineScrollBar = new HScrollBar();
            TimelinePanel = new Panel();
            SuspendLayout();
            // 
            // TimelineScrollBar
            // 
            TimelineScrollBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TimelineScrollBar.Location = new Point(0, 554);
            TimelineScrollBar.Name = "TimelineScrollBar";
            TimelineScrollBar.Size = new Size(1024, 52);
            TimelineScrollBar.TabIndex = 4;
            // 
            // TimelinePanel
            // 
            TimelinePanel.AllowDrop = true;
            TimelinePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TimelinePanel.Location = new Point(0, 0);
            TimelinePanel.Name = "TimelinePanel";
            TimelinePanel.Size = new Size(1024, 559);
            TimelinePanel.TabIndex = 5;
            TimelinePanel.DragDrop += TimelinePanel_DragDrop;
            TimelinePanel.DragEnter += TimelinePanel_DragEnter;
            // 
            // TimelineControl
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            Controls.Add(TimelinePanel);
            Controls.Add(TimelineScrollBar);
            Name = "TimelineControl";
            Size = new Size(1024, 606);
            Load += TimelineControl_Load;
            ResumeLayout(false);
        }

        #endregion

        private HScrollBar TimelineScrollBar;
        private Panel TimelinePanel;
    }
}
