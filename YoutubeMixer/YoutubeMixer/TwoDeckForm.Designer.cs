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
            DeckLeft = new Deck();
            Mixer = new Mixer();
            DeckRight = new Deck();
            SuspendLayout();
            // 
            // DeckLeft
            // 
            DeckLeft.Controller = null;
            DeckLeft.Location = new Point(10, 10);
            DeckLeft.Margin = new Padding(1, 2, 1, 2);
            DeckLeft.Name = "DeckLeft";
            DeckLeft.Size = new Size(609, 460);
            DeckLeft.TabIndex = 0;
            // 
            // Mixer
            // 
            Mixer.Location = new Point(621, 10);
            Mixer.Margin = new Padding(1, 2, 1, 2);
            Mixer.Name = "Mixer";
            Mixer.Size = new Size(151, 460);
            Mixer.TabIndex = 1;
            // 
            // DeckRight
            // 
            DeckRight.Controller = null;
            DeckRight.Location = new Point(803, 10);
            DeckRight.Margin = new Padding(1, 2, 1, 2);
            DeckRight.Name = "DeckRight";
            DeckRight.Size = new Size(553, 460);
            DeckRight.TabIndex = 0;
            // 
            // TwoDeckForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1374, 487);
            Controls.Add(Mixer);
            Controls.Add(DeckRight);
            Controls.Add(DeckLeft);
            Margin = new Padding(4, 3, 4, 3);
            Name = "TwoDeckForm";
            Text = "Form1";
            FormClosing += TwoDeckForm_FormClosing;
            Load += DeckForm_Load;
            Resize += TwoDeckForm_Resize;
            ResumeLayout(false);
        }

        #endregion

        private Deck DeckLeft;
        private Mixer Mixer;
        private Deck DeckRight;
    }
}