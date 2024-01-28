using MyVideoEditor.Controls;
using MyVideoEditor.Models;

namespace MyVideoEditor
{
    public partial class MainForm : Form
    {
        public Project Project { get; }

        public MediaControl MediaControl => Project.MediaControl;
        public TimelinesControl TimelinesControl => Project.TimelinesControl;

        public MainForm()
        {
            InitializeComponent();

            if (Environment.ProcessPath == null) throw new Exception("Cannot get execution path");
            var ffmpegExecuteblesPaths = new FfmpegExecuteblesPaths(Environment.ProcessPath, "Executebles");

            Project = new Project(ffmpegExecuteblesPaths);

            Controls.Add(Project.TimelineControl);
            Controls.Add(TimelinesControl);
            Controls.Add(MediaControl);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            MainForm_Resize(sender, e);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            MediaControl.Left = 0;
            MediaControl.Top = menuStrip1.Height;
            MediaControl.Width = ClientRectangle.Width;
            MediaControl.Height = ClientRectangle.Height - menuStrip1.Height;


            TimelinesControl.Left = 0;
            TimelinesControl.Top = menuStrip1.Height;
            TimelinesControl.Width = ClientRectangle.Width;
            TimelinesControl.Height = ClientRectangle.Height - menuStrip1.Height;

            Project.TimelineControl.Left = 0;
            Project.TimelineControl.Top = menuStrip1.Height;
            Project.TimelineControl.Width = ClientRectangle.Width;
            Project.TimelineControl.Height = ClientRectangle.Height - menuStrip1.Height;
            Project.TimelineControl.TimelinePanel_Resize(sender, e);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project.Close();
            Close();
        }

        private void mediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaToolStripMenuItem.BackColor = SystemColors.ControlDark;
            timelinesToolStripMenuItem.BackColor = SystemColors.Control;
            timelineToolStripMenuItem.BackColor = SystemColors.Control;

            MediaControl.Visible = true;
            TimelinesControl.Visible = false;
            Project.TimelineControl.Visible = false;
            Invalidate();
        }
        private void timelinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaToolStripMenuItem.BackColor = SystemColors.Control;
            timelinesToolStripMenuItem.BackColor = SystemColors.ControlDark;
            timelineToolStripMenuItem.BackColor = SystemColors.Control;

            MediaControl.Visible = false;
            TimelinesControl.Visible = true;
            Project.TimelineControl.Visible = false;
            Invalidate();
        }
        private void timelineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaToolStripMenuItem.BackColor = SystemColors.Control;
            timelinesToolStripMenuItem.BackColor = SystemColors.Control;
            timelineToolStripMenuItem.BackColor = SystemColors.ControlDark;


            MediaControl.Visible = false;
            TimelinesControl.Visible = false;
            Project.TimelineControl.Visible = true;

            Invalidate();
        }


        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void insertAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv|All Files|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Project.InsertVideo(openFileDialog.FileNames);
            }
        }



    }
}