using MyVideoEditor.Controls;
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
    public partial class TimelineControl : UserControl
    {
        public Project Project { get; set; }

        public TimelineControl(Project project)
        {
            Project = project;

            InitializeComponent();
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
            var marge = 6;
            var panelheight = (ClientRectangle.Height - TimelineScrollBar.Height - marge * 3) / 4;

            TimelineDisplayPanel.Left = 0;
            TimelineDisplayPanel.Top = 0;
            TimelineDisplayPanel.Width = ClientRectangle.Width;
            TimelineDisplayPanel.Height = panelheight * 2;

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


            TimelineVideoPanel.Left = 0;
            TimelineVideoPanel.Top = TimelineDisplayPanel.Bottom + marge;
            TimelineVideoPanel.Width = ClientRectangle.Width;
            TimelineVideoPanel.Height = panelheight;

            TimelineAudioPanel.Left = 0;
            TimelineAudioPanel.Top = TimelineVideoPanel.Bottom + marge;
            TimelineAudioPanel.Width = ClientRectangle.Width;
            TimelineAudioPanel.Height = panelheight;

            TimelineScrollBar.Left = 0;
            TimelineScrollBar.Top = TimelineAudioPanel.Bottom + marge;
            TimelineScrollBar.Width = ClientRectangle.Width;
        }
        private void TimelineControl_Paint(object sender, PaintEventArgs e)
        {
            if (Project == null) return;

            foreach (var item in Project.CurrentTimeline.VideoItems)
            {

            }

        }

        private void TimelinePanel_DragEnter(object sender, DragEventArgs e)
        {
            Project.DragEnter(sender, e);
        }
        private void TimelinePanel_DragDrop(object sender, DragEventArgs e)
        {
            Project.DragDrop(sender, e);
        }

        private void buttonPlay_Click(object sender, EventArgs e) => Project.Play();
        private void buttonPause_Click(object sender, EventArgs e) => Project.Pause();
        private void buttonForward_Click(object sender, EventArgs e) => Project.Forward();
        private void buttonBackward_Click(object sender, EventArgs e) => Project.Backward();

        private void timer1_Tick(object sender, EventArgs e)
        {
            buttonPlay.Visible = !Project.IsPlaying();
            buttonPause.Visible = !Project.IsPaused();
        }
    }
}
