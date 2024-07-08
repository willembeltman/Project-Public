using System;
using System.Windows.Forms;

namespace LanCloud.Forms
{
    public partial class MainForm : Form
    {
        public MainForm(App app)
        {
            App = app;

            InitializeComponent();
        }

        public App App { get; }

        private void MainForm_Load(object sender, EventArgs e)
        {
            App.Logger.WriteLine("Gelukt");
        }
    }
}
