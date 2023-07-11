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
    public partial class TimelineControl : UserControl
    {
        public TimelineControl()
        {
            InitializeComponent();
        }
        private void TimelineControl_Load(object sender, EventArgs e)
        {

        }

        private void TimelinePanel_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the data being dragged is a file
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (CheckFileType.Check(files))
                    e.Effect = DragDropEffects.Copy; // Set the cursor to indicate a copy operation
            }
        }
        private void TimelinePanel_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            InsertVideo(files);
        }
        public void InsertVideo(string[] files)
        {
            // Filter again because user selected them
            files = files.Where(a => CheckFileType.Check(a)).ToArray();


        }

    }
}
