using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanCloud
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
            App.WriteLine("Gelukt");
        }

        internal void SettingsChanged()
        {
            throw new NotImplementedException();
        }
    }
}
