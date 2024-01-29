using MyVideoEditor.DTOs;
using MyVideoEditor.Services;
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
    public partial class TimelinesMainControl : UserControl
    {
        #region Props 

        public MainForm MainForm { get; }

        public ProjectService ProjectService => MainForm.ProjectService;
        public MediaContainerService MediaContainerService => MainForm.MediaContainerService;
        public TimelineService TimelineService => MainForm.TimelineService;

        public Project? Project => MainForm.Project;
        public Timeline? Timeline => MainForm.Timeline;

        #endregion

        public TimelinesMainControl(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();
        }

        public void ProjectSet()
        {
            if (Project == null)
                return;

        }
    }
}
