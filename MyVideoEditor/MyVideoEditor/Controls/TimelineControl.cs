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
    public partial class TimelineControl : UserControl
    {
        #region Props 

        MainForm MainForm { get; }

        ProjectService ProjectService => MainForm.ProjectService;
        MediaContainerService MediaContainerService => MainForm.MediaContainerService;
        TimelineService TimelineService => MainForm.TimelineService;
        TimeStampService TimeStampService => MainForm.TimeStampService;

        Project? Project => MainForm?.Project;
        Timeline? Timeline => MainForm?.Timeline;

        #endregion

        public TimelineControl(MainForm mainForm)
        {
            MainForm = mainForm;
            InitializeComponent();
        }

        private void TimelineControl_Paint(object sender, PaintEventArgs e)
        {
            if (Project == null) return;
            if (Timeline == null) return;

            // Huidige timecode bepalen / zichtbare selectie

            var timelinelength = Timeline.TotalLength;
            if (timelinelength < 60) timelinelength = 60;
            var timelinestart = Timeline.DisplayStart;
            var timelineend = timelinestart + timelinelength;

            // Video items tekenen
            var list = Timeline.TimelineVideos
                //.Where(a =>
                //    (a.TimelineStartTime >= timelinestart && a.TimelineStartTime <= timelineend) ||
                //    (a.TimelineEndTime >= timelinestart && a.TimelineEndTime <= timelineend))
                .OrderBy(a => a.TimelineStartTime)
                .ToArray();

            var marge = 8;

            var height = (Height - marge) / 2;
            var top1 = 0;
            var top2 = height + marge;

            var videorect = new Rectangle(Left, top1, Width, height);
            var audiorect = new Rectangle(Left, top2, Width, height);

            var g = e.Graphics;
            g.Clear(Color.White);
            g.FillRectangle(Brushes.LightGray, videorect);
            g.DrawLine(Pens.Black, videorect.Left, videorect.Top, videorect.Right, videorect.Top);
            g.DrawLine(Pens.Black, videorect.Left, videorect.Bottom, videorect.Right, videorect.Bottom);

            g.FillRectangle(Brushes.LightGray, audiorect);
            g.DrawLine(Pens.Black, audiorect.Left, audiorect.Top, audiorect.Right, audiorect.Top);
            g.DrawLine(Pens.Black, audiorect.Left, audiorect.Bottom, audiorect.Right, audiorect.Bottom);


            var items = 10;
            var step = 1d;
            while (true)
            {
                if (timelinelength / step > items)
                {
                    step = step * 10;
                }
                else if (timelinelength / step < items / 5)
                {
                    step = step * 10;
                }
                else break;
            }

            for (var i = timelinestart; i <= timelineend; i += step)
            {
                var left = Convert.ToInt32(videorect.Width / timelinelength * i);

                // Draw mark line 
                g.DrawLine(Pens.Gray, left, videorect.Top + 1, left, videorect.Bottom - 1);
                g.DrawLine(Pens.Gray, left, audiorect.Top + 1, left, audiorect.Bottom - 1);

                // Draw current time (i)
                var textrect = g.MeasureString($"{i:F2}", SystemFonts.DefaultFont);
                g.DrawString($"{i:F2}", SystemFonts.DefaultFont, Brushes.Black, left, audiorect.Bottom - textrect.Height);
            }


            foreach (var item in list)
            {
                var leftperc = (item.TimelineStartTime - timelinestart) / timelinelength;
                var rightperc = (item.TimelineEndTime - timelineend) / timelinelength;

                var left = Convert.ToInt32(videorect.Width * leftperc);
                var right = Convert.ToInt32(videorect.Width * rightperc);
                var width = right - left;

                var rect = new Rectangle(videorect.Left + left, videorect.Top + 5, width, videorect.Height - 10);

                g.FillRectangle(Brushes.Black, rect);
            }

            // Audio items tekenen
            foreach (var item in Timeline.TimelineAudios)
            {

            }

            g.Dispose();
        }

        private void TimelineControl_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
