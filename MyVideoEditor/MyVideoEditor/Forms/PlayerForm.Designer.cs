namespace MyVideoEditor.Forms
{
    partial class PlayerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            direct3dVideoDisplay1 = new Controls.Direct3DVideoDisplay();
            SuspendLayout();
            // 
            // direct3dVideoDisplay1
            // 
            direct3dVideoDisplay1.Location = new Point(12, 12);
            direct3dVideoDisplay1.Name = "direct3dVideoDisplay1";
            direct3dVideoDisplay1.Size = new Size(776, 426);
            direct3dVideoDisplay1.TabIndex = 0;
            // 
            // PlayerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(direct3dVideoDisplay1);
            Name = "PlayerForm";
            Text = "PlayerForm";
            Load += PlayerForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Controls.Direct3DVideoDisplay direct3dVideoDisplay1;
    }
}