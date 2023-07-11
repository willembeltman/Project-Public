using YoutubeMixer.UserControls;

namespace YoutubeMixer.Forms
{
    partial class TwoDeckForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DeckLeft = new YoutubeMixer.UserControls.Deck();
            this.Mixer = new YoutubeMixer.UserControls.Mixer();
            this.DeckRight = new YoutubeMixer.UserControls.Deck();
            this.SuspendLayout();
            // 
            // DeckLeft
            // 
            this.DeckLeft.Controller = null;
            this.DeckLeft.Location = new System.Drawing.Point(12, 12);
            this.DeckLeft.Margin = new System.Windows.Forms.Padding(2);
            this.DeckLeft.Name = "DeckLeft";
            this.DeckLeft.Size = new System.Drawing.Size(730, 553);
            this.DeckLeft.TabIndex = 0;
            // 
            // Mixer
            // 
            this.Mixer.Location = new System.Drawing.Point(746, 11);
            this.Mixer.Margin = new System.Windows.Forms.Padding(2);
            this.Mixer.Name = "Mixer";
            this.Mixer.Size = new System.Drawing.Size(182, 553);
            this.Mixer.TabIndex = 1;
            // 
            // DeckRight
            // 
            this.DeckRight.Controller = null;
            this.DeckRight.Location = new System.Drawing.Point(964, 11);
            this.DeckRight.Margin = new System.Windows.Forms.Padding(2);
            this.DeckRight.Name = "DeckRight";
            this.DeckRight.Size = new System.Drawing.Size(663, 553);
            this.DeckRight.TabIndex = 0;
            // 
            // TwoDeckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1649, 584);
            this.Controls.Add(this.Mixer);
            this.Controls.Add(this.DeckRight);
            this.Controls.Add(this.DeckLeft);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "TwoDeckForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TwoDeckForm_FormClosing);
            this.Load += new System.EventHandler(this.DeckForm_Load);
            this.Resize += new System.EventHandler(this.TwoDeckForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Deck DeckLeft;
        private Mixer Mixer;
        private Deck DeckRight;
    }
}