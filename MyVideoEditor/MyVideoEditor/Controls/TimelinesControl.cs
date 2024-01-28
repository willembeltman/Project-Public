using MyVideoEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyVideoEditor.Controls
{
    public partial class TimelinesControl : UserControl
    {
        public TimelinesControl(Project project)
        {
            Project = project;
            InitializeComponent();
        }

        public Project Project { get; }
    }
}
