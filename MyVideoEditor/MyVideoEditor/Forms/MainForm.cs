using MyVideoEditor.Controls;
using MyVideoEditor.DTOs;
using MyVideoEditor.Services;
using MyVideoEditor.VideoObjects;

namespace MyVideoEditor
{
    public partial class MainForm : Form
    {
        public MediaMainControl MediaControl { get; }
        public TimelinesMainControl TimelinesControl { get; }
        public TimelineMainControl TimelineControl { get; }

        public FfmpegExecuteblesPaths FfmpegExecuteblesPaths { get; }
        public ProjectService ProjectService { get; }
        public MediaContainerService MediaContainerService { get; }
        public TimelineService TimelineService { get; }
        public TimeStampService TimeStampService { get; }

        Project? _Project { get; set; }
        public Project? Project
        {
            get
            {
                return _Project;
            }
            set
            {
                _Project = value;
                MediaControl.ProjectSet();
                TimelinesControl.ProjectSet();
                TimelineControl.ProjectSet();
            }
        }

        Timeline? _Timeline { get; set; }
        public Timeline? Timeline
        {
            get
            {
                if (Project == null) return null;
                if (Project.CurrentTimelineId == Guid.Empty) return null;
                if (_Timeline == null || _Timeline.Id != Project.CurrentTimelineId)
                {
                    _Timeline = Project.Timelines.First(a => a.Id == Project.CurrentTimelineId);
                }
                return _Timeline;
            }
            set
            {
                if (Project == null) return;
                Project.CurrentTimelineId = value?.Id ?? Guid.Empty;
                _Timeline = value;
            }
        }

        public MainForm()
        {
            if (Environment.ProcessPath == null) throw new Exception("Cannot get execution path");

            FfmpegExecuteblesPaths =
                new FfmpegExecuteblesPaths(Environment.ProcessPath, "Executebles");
            MediaContainerService =
                new MediaContainerService(FfmpegExecuteblesPaths);
            ProjectService =
                new ProjectService(FfmpegExecuteblesPaths, MediaContainerService);
            TimelineService =
                new TimelineService();
            TimeStampService =
                new TimeStampService();

            MediaControl = new MediaMainControl(this);
            TimelinesControl = new TimelinesMainControl(this);
            TimelineControl = new TimelineMainControl(this);

            Controls.Add(TimelineControl);
            Controls.Add(TimelinesControl);
            Controls.Add(MediaControl);

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MainForm_Resize(sender, e);

            Project = ProjectService.NewProjectClicked_AfterConfirm();
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

            TimelineControl.Left = 0;
            TimelineControl.Top = menuStrip1.Height;
            TimelineControl.Width = ClientRectangle.Width;
            TimelineControl.Height = ClientRectangle.Height - menuStrip1.Height;

            TimelineControl.TimelinePanel_Resize(sender, e);
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project != null)
                if (ProjectService.Close(Project) == false)
                    return;

            Project = ProjectService.NewProjectClicked_AfterConfirm();
        }
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project != null)
                if (ProjectService.Close(Project) == false)
                    return;

            Project = ProjectService.OpenProjectClicked_AfterConfirm();
        }
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project == null)
            {
                MessageBox.Show("No project open?");
                return;
            }
            ProjectService.SaveProjectClicked(Project);
        }
        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project == null)
            {
                MessageBox.Show("No project open?");
                return;
            }
            ProjectService.SaveAsProjectClicked(Project);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project == null) return;
            ProjectService.Close(Project);
            Close();
        }

        private void mediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaToolStripMenuItem.BackColor = SystemColors.ControlDark;
            timelinesToolStripMenuItem.BackColor = SystemColors.Control;
            timelineToolStripMenuItem.BackColor = SystemColors.Control;

            MediaControl.Visible = true;
            TimelinesControl.Visible = false;
            TimelineControl.Visible = false;

            Invalidate();
        }
        private void timelinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaToolStripMenuItem.BackColor = SystemColors.Control;
            timelinesToolStripMenuItem.BackColor = SystemColors.ControlDark;
            timelineToolStripMenuItem.BackColor = SystemColors.Control;

            MediaControl.Visible = false;
            TimelinesControl.Visible = true;
            TimelineControl.Visible = false;

            Invalidate();
        }
        private void timelineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaToolStripMenuItem.BackColor = SystemColors.Control;
            timelinesToolStripMenuItem.BackColor = SystemColors.Control;
            timelineToolStripMenuItem.BackColor = SystemColors.ControlDark;

            MediaControl.Visible = false;
            TimelinesControl.Visible = false;
            TimelineControl.Visible = true;

            Invalidate();
        }

        private void insertAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project == null || Timeline == null)
            {
                MessageBox.Show("No project open?");
                return;
            }
            ProjectService.InsertVideosButtonClicked(Project, Timeline);
            Invalidate();
        }
    }
}