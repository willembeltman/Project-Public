namespace VideoEditor.Static
{
    public static class ITimelineClipHelper
    {
        public static Rectangle CalculateRectangle(this ITimelineClip clip, Timeline timeline, Rectangle timelineRectangle)
        {
            int Middle = timelineRectangle.Height / 2;
            int VideoBlockHeight = (Middle - Constants.MiddleOffset) / timeline.VisibleVideoLayers;
            int AudioBlockHeight = (Middle - Constants.MiddleOffset) / timeline.VisibleAudioLayers;

            int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - timeline.VisibleStart) / timeline.VisibleWidth * timelineRectangle.Width);
            int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - timeline.VisibleStart) / timeline.VisibleWidth * timelineRectangle.Width);
            int width = x2 - x1;

            int layer;
            int y;
            if (clip.IsVideoClip)
            {
                layer = clip.Layer - timeline.FirstVisibleVideoLayer;
                y = Middle - Constants.MiddleOffset - VideoBlockHeight - layer * VideoBlockHeight;
            }
            else
            {
                layer = clip.Layer - timeline.FirstVisibleAudioLayer;
                y = Middle + Constants.MiddleOffset + (clip.Layer - timeline.FirstVisibleAudioLayer) * AudioBlockHeight;
            }

            var rect = new Rectangle(x1, y + Constants.Margin / 2, width, VideoBlockHeight - Constants.Margin);
            return rect;
        }
    }
}