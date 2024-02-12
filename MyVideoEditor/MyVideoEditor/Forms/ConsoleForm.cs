using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyVideoEditor.Forms
{
    public delegate void SendCommand(string commandJson);

    public partial class ConsoleForm : Form
    {
        public MainForm MainForm { get; }

        public ConsoleForm(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();
        }
        string terminaloutput { get; set; }
        string terminalinput { get; set; }

        public string Text
        {
            get
            {
                return terminaloutput;
            }
            set
            {
                terminaloutput = value;

                textBoxConsoleInput.Text =
                    terminalinput + Environment.NewLine +
                    terminaloutput;
            }
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            ConsoleForm_Resize(sender, e);  
        }

        private void ConsoleForm_Resize(object sender, EventArgs e)
        {
            textBoxConsoleInput.Height = 
                textBoxConsoleInput.Font.Height +
                textBoxConsoleInput.Margin.Top +
                textBoxConsoleInput.Margin.Bottom;

            textBoxConsole.Top = 0;
            textBoxConsole.Left = 0;
            textBoxConsole.Width = ClientRectangle.Width;
            textBoxConsole.Height = ClientRectangle.Height - textBoxConsoleInput.Height;

            textBoxConsoleInput.Top = textBoxConsole.Bottom;
            textBoxConsoleInput.Left = 0;
            textBoxConsoleInput.Width = ClientRectangle.Width;


        }
    }
}
