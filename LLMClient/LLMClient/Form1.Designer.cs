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
            PromptTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PromptTextbox.Location = new Point(12, 420);
            PromptTextbox.Multiline = true;
            PromptTextbox.Name = "PromptTextbox";
            PromptTextbox.Size = new Size(634, 60);
            PromptTextbox.TabIndex = 0;
            // 
            // SendButton
            // 
            SendButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SendButton.Location = new Point(652, 420);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(120, 60);
            SendButton.TabIndex = 1;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // OutputTextbox
            // 
            OutputTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            OutputTextbox.Location = new Point(12, 44);
            OutputTextbox.Name = "OutputTextbox";
            OutputTextbox.ReadOnly = true;
            OutputTextbox.Size = new Size(760, 360);
            OutputTextbox.TabIndex = 2;
            OutputTextbox.Text = "";
            // 
            // ModelCombo
            // 
            ModelCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ModelCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ModelCombo.FormattingEnabled = true;
            ModelCombo.Location = new Point(12, 12);
            ModelCombo.Name = "ModelCombo";
            ModelCombo.Size = new Size(600, 23);
            ModelCombo.TabIndex = 3;
            ModelCombo.SelectedIndexChanged += ModelCombo_SelectedIndexChanged;
            // 
            // StatusLabel
            // 
            StatusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            StatusLabel.Location = new Point(620, 12);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new Size(152, 24);
            StatusLabel.TabIndex = 4;
            StatusLabel.Text = "Ready";
            StatusLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            ClientSize = new Size(784, 491);
            Controls.Add(StatusLabel);
            Controls.Add(ModelCombo);
            Controls.Add(OutputTextbox);
            Controls.Add(SendButton);
            Controls.Add(PromptTextbox);
            Name = "Form1";
            Text = "Foundry Client";
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
