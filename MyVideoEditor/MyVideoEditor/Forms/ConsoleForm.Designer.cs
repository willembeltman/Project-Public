namespace MyVideoEditor.Forms
{
    partial class ConsoleForm
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
            textBoxConsoleInput = new TextBox();
            textBoxConsole = new TextBox();
            SuspendLayout();
            // 
            // textBoxConsoleInput
            // 
            textBoxConsoleInput.BackColor = Color.Black;
            textBoxConsoleInput.ForeColor = Color.White;
            textBoxConsoleInput.Location = new Point(12, 342);
            textBoxConsoleInput.Multiline = true;
            textBoxConsoleInput.Name = "textBoxConsoleInput";
            textBoxConsoleInput.Size = new Size(776, 81);
            textBoxConsoleInput.TabIndex = 0;
            // 
            // textBoxConsole
            // 
            textBoxConsole.BackColor = Color.Black;
            textBoxConsole.ForeColor = Color.White;
            textBoxConsole.Location = new Point(12, 12);
            textBoxConsole.Multiline = true;
            textBoxConsole.Name = "textBoxConsole";
            textBoxConsole.Size = new Size(776, 324);
            textBoxConsole.TabIndex = 0;
            // 
            // ConsoleForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 433);
            Controls.Add(textBoxConsole);
            Controls.Add(textBoxConsoleInput);
            Name = "ConsoleForm";
            Load += ConsoleForm_Load;
            Resize += ConsoleForm_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxConsoleInput;
        private TextBox textBoxConsole;
    }
}