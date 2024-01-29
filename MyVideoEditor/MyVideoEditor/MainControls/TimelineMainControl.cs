using MyVideoEditor.Controls;
using MyVideoEditor.DTOs;
using MyVideoEditor.Enums;
using MyVideoEditor.Models;
using MyVideoEditor.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyVideoEditor
{
    public partial class TimelineMainControl : UserControl
    {
        #region Props 

        public MainForm MainForm { get; }

        public ProjectService ProjectService => MainForm.ProjectService;
        public MediaContainerService MediaContainerService => MainForm.MediaContainerService;
        public TimelineService TimelineService => MainForm.TimelineService;
        public TimeStampService TimeStampService => MainForm.TimeStampService;

        public Project? Project => MainForm.Project;
        public Timeline? Timeline => MainForm.Timeline;

        #endregion

        TimelineControl TimelineControl { get; }
        Color ButtonBackColor { get; } = SystemColors.Control;
        Color ButtonBackColorEnabled { get; } = SystemColors.ControlDark;

        public TimelineMainControl(MainForm mainForm)
        {
            MainForm = mainForm;

            TimelineControl = new TimelineControl(MainForm);
            Controls.Add(TimelineControl);

            InitializeComponent();
        }

        public void ProjectSet()
        {
            if (Project == null)
                return;

            ProjectWidth.Text = Project.Width.ToString();
            ProjectHeight.Text = Project.Height.ToString();
            //ProjectFramerate.SelectedText = project.Framerate.ToString("F2");

            Invalidate();
        }



        private void TimelineControl_Load(object sender, EventArgs e)
        {
            TimelinePanel_Resize(sender, e);
        }
        public void TimelinePanel_Resize(object sender, EventArgs e)
        {
            var buttonwidth = 47;
            var buttonheight = 50;
            var buttonfontsize = 15f;
            var marge = 8;
            var panelheight = (ClientRectangle.Height - TimelineScrollBar.Height - marge) / 2;

            var panelleft = groupBoxProjectSettings.Right + marge;
            var panelwidth = ClientRectangle.Width - groupBoxProjectSettings.Width - groupBoxClipSettings.Width - marge * 3;


            TimelineDisplayPanel.Left = panelleft;
            TimelineDisplayPanel.Top = 0;
            TimelineDisplayPanel.Width = panelwidth;
            TimelineDisplayPanel.Height = panelheight;

            groupBoxProjectSettings.Height = panelheight - groupBoxTimelineTools.Height - marge * 2;
            groupBoxTimelineTools.Top = groupBoxProjectSettings.Bottom + marge;
            groupBoxClipSettings.Height = panelheight;

            buttonBackward.Width = buttonwidth;
            buttonBackward.Height = buttonheight;
            buttonPause.Width = buttonwidth;
            buttonPause.Height = buttonheight;
            buttonPlay.Width = buttonwidth;
            buttonPlay.Height = buttonheight;
            buttonForward.Width = buttonwidth;
            buttonForward.Height = buttonheight;

            buttonBackward.Left = (ClientRectangle.Width - buttonwidth * 3 + marge * 2) / 2;
            buttonPause.Left = buttonBackward.Right + marge;
            buttonPlay.Left = buttonBackward.Right + marge;
            buttonForward.Left = buttonPlay.Right + marge;

            buttonBackward.Top = TimelineDisplayPanel.Bottom - marge - buttonheight;
            buttonPause.Top = TimelineDisplayPanel.Bottom - marge - buttonheight;
            buttonPlay.Top = TimelineDisplayPanel.Bottom - marge - buttonheight;
            buttonForward.Top = TimelineDisplayPanel.Bottom - marge - buttonheight;

            buttonBackward.Font = new Font(buttonForward.Font.FontFamily.Name, buttonfontsize);
            buttonPause.Font = new Font(buttonForward.Font.FontFamily.Name, buttonfontsize);
            buttonPlay.Font = new Font(buttonForward.Font.FontFamily.Name, buttonfontsize);
            buttonForward.Font = new Font(buttonForward.Font.FontFamily.Name, buttonfontsize);

            TimelineControl.Left = 0;
            TimelineControl.Top = TimelineDisplayPanel.Bottom + marge;
            TimelineControl.Width = ClientRectangle.Width;
            TimelineControl.Height = panelheight;

            TimelineScrollBar.Left = 0;
            TimelineScrollBar.Top = TimelineControl.Bottom + marge;
            TimelineScrollBar.Width = ClientRectangle.Width;

        }

        private void TimelinePanel_DragEnter(object sender, DragEventArgs e)
        {
            if (Project == null || Timeline == null) return;
            ProjectService.DragEnter(Project, Timeline, sender, e);
        }
        private void TimelineAudioPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (Project == null || Timeline == null) return;
            ProjectService.DragEnter(Project, Timeline, sender, e);
        }
        private void TimelinePanel_DragDrop(object sender, DragEventArgs e)
        {
            if (Project == null || Timeline == null) return;
            ProjectService.DragDrop(Project, Timeline, sender, e);
        }
        private void TimelineAudioPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (Project == null || Timeline == null) return;
            ProjectService.DragDrop(Project, Timeline, sender, e);
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (Project == null) return;
            TimelineService.Play(Project, Timeline);
        }
        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (Project == null) return;
            TimelineService.Pause(Project, Timeline);
        }
        private void buttonForward_Click(object sender, EventArgs e)
        {
            if (Project == null) return;
            TimelineService.Forward(Project, Timeline);
        }
        private void buttonBackward_Click(object sender, EventArgs e)
        {
            if (Project == null) return;
            TimelineService.Backward(Project, Timeline);
        }


        private void buttonCutTool_Click(object sender, EventArgs e)
        {
            buttonCutTool.BackColor = ButtonBackColorEnabled;
            buttonSelectionTool.BackColor = ButtonBackColor;
        }
        private void buttonSelectionTool_Click(object sender, EventArgs e)
        {
            buttonCutTool.BackColor = ButtonBackColor;
            buttonSelectionTool.BackColor = ButtonBackColorEnabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Project == null || Timeline == null)
            {
                buttonPlay.Visible = true;
                buttonPause.Visible = false;
                buttonPlay.Enabled = false;
                buttonForward.Enabled = false;
                buttonBackward.Enabled = false;
                return;
            }

            buttonPlay.Visible = !TimelineService.IsPlaying(Project, Timeline);
            buttonPause.Visible = !TimelineService.IsPaused(Project, Timeline);
        }
    }
}
