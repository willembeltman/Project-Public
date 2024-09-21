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
            DeckLeft.AudioSource = null;
            DeckLeft.Location = new Point(7, 6);
            DeckLeft.Margin = new Padding(1, 1, 1, 1);
            DeckLeft.Name = "DeckLeft";
            DeckLeft.Size = new Size(426, 276);
            DeckLeft.TabIndex = 0;
            // 
            // Mixer
            // 
            Mixer.Location = new Point(435, 6);
            Mixer.Margin = new Padding(1, 1, 1, 1);
            Mixer.Name = "Mixer";
            Mixer.Size = new Size(106, 276);
            Mixer.TabIndex = 1;
            // 
            // DeckRight
            // 
            DeckRight.AudioSource = null;
            DeckRight.Location = new Point(562, 6);
            DeckRight.Margin = new Padding(1, 1, 1, 1);
            DeckRight.Name = "DeckRight";
            DeckRight.Size = new Size(387, 276);
            DeckRight.TabIndex = 0;
            // 
            // TwoDeckForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(962, 312);
            Controls.Add(Mixer);
            Controls.Add(DeckRight);
            Controls.Add(DeckLeft);
            Margin = new Padding(3, 2, 3, 2);
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