namespace LLMClient
{
    partial class Form1
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
            PromptTextbox = new TextBox();
            SendButton = new Button();
            OutputTextbox = new RichTextBox();
            ModelCombo = new ComboBox();
            StatusLabel = new Label();
            SuspendLayout();
            // 
            // PromptTextbox
            // 
            PromptTextbox.BackColor = Color.Black;
            PromptTextbox.ForeColor = Color.White;
            PromptTextbox.Location = new Point(12, 12);
            PromptTextbox.Multiline = true;
            PromptTextbox.Name = "PromptTextbox";
            PromptTextbox.Size = new Size(604, 88);
            PromptTextbox.TabIndex = 0;
            // 
            // SendButton
            // 
            SendButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SendButton.BackColor = Color.Black;
            SendButton.ForeColor = Color.White;
            SendButton.Location = new Point(622, 56);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(166, 44);
            SendButton.TabIndex = 1;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = false;
            SendButton.Click += SendButton_Click;
            // 
            // OutputTextbox
            // 
            OutputTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            OutputTextbox.BackColor = Color.Black;
            OutputTextbox.ForeColor = Color.White;
            OutputTextbox.Location = new Point(12, 106);
            OutputTextbox.Name = "OutputTextbox";
            OutputTextbox.Size = new Size(776, 332);
            OutputTextbox.TabIndex = 2;
            OutputTextbox.Text = "";
            // 
            // LLMCombo
            // 
            ModelCombo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ModelCombo.FormattingEnabled = true;
            ModelCombo.Location = new Point(622, 12);
            ModelCombo.Name = "LLMCombo";
            ModelCombo.Size = new Size(166, 23);
            ModelCombo.TabIndex = 3;
            ModelCombo.SelectedIndexChanged += ModelCombo_SelectedIndexChanged;
            // 
            // StatusLabel
            // 
            StatusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            StatusLabel.AutoSize = true;
            StatusLabel.Location = new Point(622, 38);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(166, 15);
            StatusLabel.TabIndex = 4;
            StatusLabel.Text = "Status: Application not started";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(StatusLabel);
            Controls.Add(ModelCombo);
            Controls.Add(OutputTextbox);
            Controls.Add(SendButton);
            Controls.Add(PromptTextbox);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox PromptTextbox;
        private Button SendButton;
        private RichTextBox OutputTextbox;
        private ComboBox ModelCombo;
        private Label StatusLabel;
    }
}
