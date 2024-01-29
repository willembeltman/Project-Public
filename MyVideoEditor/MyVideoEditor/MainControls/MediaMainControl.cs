using MyVideoEditor.Controls;
using MyVideoEditor.DTOs;
using MyVideoEditor.Models;
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

namespace MyVideoEditor
{
    public partial class MediaMainControl : UserControl
    {
        #region Props 

        public MainForm MainForm { get; }

        public ProjectService ProjectService => MainForm.ProjectService;
        public MediaContainerService MediaContainerService => MainForm.MediaContainerService;
        public TimelineService TimelineService => MainForm.TimelineService;

        public Project? Project => MainForm.Project;
        public Timeline? Timeline => MainForm.Timeline;

        #endregion

        public MediaMainControl(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();
        }


        public void ProjectSet()
        {
            if (Project == null)
                return;

            //Medias = MediaContainerService.Open(project.MediaContainersFullnames)
            //    .Select(a => new MediaItem(a))
            //    .ToArray();

            //dataGridView1.DataSource = Medias;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {

        }
    }

    internal class MediaControlItem
    {
        private MediaContainer a;

        public MediaControlItem(MediaContainer a)
        {
            this.a = a;

        }
        public string Name => a.FullName;
    }
}
