using System.ComponentModel;
using VideoEditor.Static;

namespace VideoEditor.Forms;

public partial class TimelineVideoClipControl : UserControl
{
    public TimelineVideoClipControl(TimelineControl timelineControl, TimelineVideoClip videoClip)
    {
        InitializeComponent();
        TimelineControl = timelineControl;
        VideoClip = videoClip;
        Setup();
    }

    public TimelineControl TimelineControl { get; }
    public TimelineVideoClip VideoClip { get; }


    public void Setup()
    {

        //var yoffset = 10;
        //var margin = Constants.Margin / 2;
        //var VideoBlockHeight = (TimelineControl.Middle - yoffset) / VideoLayerCount;

        //foreach (var clip in Engine.Timeline.VideoClips)
        //{
        //    int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - VisibleStart) / VisibleWidth * Width);
        //    int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - VisibleStart) / VisibleWidth * Width);
        //    int width = x2 - x1;
        //    if (x1 > Width || x2 < 0) continue; // Clip buiten zichtbare range

        //    int y = Middle - yoffset - VideoBlockHeight - clip.Layer * VideoBlockHeight + margin;

        //    var rect = new Rectangle(x1, y, width, VideoBlockHeight - margin);
        //    g.FillRectangle(Brushes.Blue, rect);
        //    g.DrawRectangle(Pens.White, rect);
        //}

        //AudioLayerCount = Timeline.VideoClips.Any() ? Timeline.VideoClips.Max(a => a.Layer) + 2 : 1;
        //AudioBlockHeight = (Middle - yoffset) / AudioLayerCount;

        //foreach (var clip in Timeline.AudioClips)
        //{
        //    int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - VisibleStart) / VisibleWidth * Width);
        //    int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - VisibleStart) / VisibleWidth * Width);
        //    int width = x2 - x1;
        //    if (x1 > Width || x2 < 0) continue; // Clip buiten zichtbare range

        //    int y = Middle + yoffset + clip.Layer * AudioBlockHeight;

        //    var rect = new Rectangle(x1, y, width, AudioBlockHeight - margin);
        //    g.FillRectangle(Brushes.Blue, rect);
        //    g.DrawRectangle(Pens.White, rect);
        //}
    }
}
