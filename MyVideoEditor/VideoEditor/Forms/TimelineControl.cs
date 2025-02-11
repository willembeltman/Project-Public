using System.Linq;
using VideoEditor.Enums;
using VideoEditor.Static;

namespace VideoEditor.Forms;

public partial class TimelineControl : UserControl
{
    public TimelineControl()
    {
        InitializeComponent();
        Engine.TimelineControl = this;
    }

    int OldSmallScrollDelta = 0;
    int TotalBigScrollDelta = 0;

    Timeline Timeline => Engine.Timeline;
    Rectangle TimelineRectangle => new Rectangle(
        ClientRectangle.Left,
        ClientRectangle.Top,
        ClientRectangle.Width,
        ClientRectangle.Height - scrollBarControl.Height);
    List<File> TempFiles { get; } = new List<File>();
    List<TimelineClipVideo> TempTimelineClipVideos { get; } = new List<TimelineClipVideo>();
    List<TimelineClipAudio> TempTimelineClipAudios { get; } = new List<TimelineClipAudio>();
    IEnumerable<ITimelineClip> TempTimelineClips => 
        TempTimelineClipVideos
            .Select(a => a as ITimelineClip)
            .Concat(TempTimelineClipAudios);

    private void TimelineControl_Load(object sender, EventArgs e)
    {
        SetupScrollbar();
    }

    private void TimelineControl_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Black);

        // Teken tijdsmarkeringen
        DrawTimeMarkers(g);

        // Teken clips
        DrawVideoClips(g);
    }
    private void DrawTimeMarkers(Graphics g)
    {
        using var pen = new Pen(Color.FromArgb(0, 0, 128), 1);
        using var pen2 = new Pen(Color.FromArgb(64, 0, 0), 1);
        using var font = new Font("Arial", Constants.TextSize);
        using var brush = new SolidBrush(Color.White);

        int middle = (ClientRectangle.Height - scrollBarControl.Height) / 2;
        int videoBlockHeight = (middle - Constants.MiddleOffset) / Timeline.VisibleVideoLayers;
        int audioBlockHeight = (middle - Constants.MiddleOffset) / Timeline.VisibleAudioLayers;

        for (var i = 0; i < Timeline.VisibleVideoLayers; i++)
        {
            var y = middle - i * videoBlockHeight - Constants.MiddleOffset;
            g.DrawLine(pen2, 0, y, Width, y);

            var text = $"{i + Timeline.FirstVisibleVideoLayer}";
            var textSize = g.MeasureString(text, font);
            var textY = y - videoBlockHeight / 2 - (int)(textSize.Height / 2);
            g.DrawString(text, font, brush, 2, textY);
        }
        for (var i = 0; i < Timeline.VisibleAudioLayers; i++)
        {
            var y = middle + i * audioBlockHeight + Constants.MiddleOffset;
            g.DrawLine(pen2, 0, y, Width, y);

            var text = $"{i + Timeline.FirstVisibleAudioLayer}";
            var textSize = g.MeasureString(text, font);
            var textY = y + videoBlockHeight / 2 - (int)(textSize.Height / 2);
            g.DrawString(text, font, brush, 2, textY);
        }

        var timeIncrease = 0.01D;
        var decimals = 2;
        while (Width / Timeline.VisibleWidth * timeIncrease < 50)
        {
            timeIncrease *= 10;
            decimals--;
        }
        if (decimals < 0) decimals = 0;

        for (var sec = 0D; sec < double.MaxValue; sec += timeIncrease)
        {
            var x = Convert.ToInt32((sec - Timeline.VisibleStart) / Timeline.VisibleWidth * Width);
            if (x >= Width) break;
            g.DrawLine(pen, x, 0, x, Height);

            var text = $"{sec.ToString("F" + decimals)}s";
            var textSize = g.MeasureString(text, font);
            var textY = middle - (int)(textSize.Height / 2);
            g.DrawString(text, font, brush, x + 2, textY);
        }

    }
    private void DrawVideoClips(Graphics g)
    {
        var clips = Timeline.AllClips.Concat(TempTimelineClips);
        foreach (var clip in clips)
        {
            var rect = clip.CalculateRectangle(Timeline, TimelineRectangle);

            if (rect.Left > ClientRectangle.Width || rect.Right < 0) continue; // Clip buiten zichtbare range
            if (rect.Top > ClientRectangle.Height || rect.Bottom < 0) continue; // Clip buiten zichtbare range

            var selected = Timeline.SelectedClips.Contains(clip);
            var fillBrush = selected ? Brushes.Red : clip.IsVideoClip ? Brushes.DarkBlue : Brushes.Aqua;
            var borderPen = Pens.White;

            g.FillRectangle(fillBrush, rect);
            g.DrawRectangle(borderPen, rect);
        }
    }

    private void TimelineControl_MouseWheel(object? sender, MouseEventArgs e)
    {
        var delta = GetScrollDelta(e);
        var clientPoint = new Point(e.X, e.Y);
        var currentTime = TranslateToCurrentTime(clientPoint);
        if ((ModifierKeys & Keys.Control) == Keys.Control)
        {
            ZoomX(delta, clientPoint, currentTime);
        }
        else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            ZoomY(delta, clientPoint, currentTime);
        }
        else
        {
            ScrollY(delta, clientPoint, currentTime);
        }
        Invalidate();
        SetupScrollbar();
    }
    private int GetScrollDelta(MouseEventArgs e)
    {
        TotalBigScrollDelta += e.Delta;

        if (TotalBigScrollDelta / 120 == OldSmallScrollDelta)
            return 0;

        var delta = TotalBigScrollDelta / 120 - OldSmallScrollDelta;
        OldSmallScrollDelta = TotalBigScrollDelta / 120;
        return delta;
    }
    private void ScrollY(int delta, Point clientPoint, double currentTime)
    {
        int Middle = (ClientRectangle.Height - scrollBarControl.Height) / 2;
        if (clientPoint.Y < Middle)
        {
            // Video
            Timeline.FirstVisibleVideoLayer += delta;
            if (Timeline.FirstVisibleVideoLayer < 0) Timeline.FirstVisibleVideoLayer = 0;
        }
        if (clientPoint.Y > Middle)
        {
            // Audio
            Timeline.FirstVisibleAudioLayer += delta;
            if (Timeline.FirstVisibleAudioLayer < 0) Timeline.FirstVisibleAudioLayer = 0;
        }
    }
    private void ZoomY(int delta, Point clientPoint, double currentTime)
    {
        int Middle = (ClientRectangle.Height - scrollBarControl.Height) / 2;
        if (clientPoint.Y < Middle)
        {
            // Video
            Timeline.VisibleVideoLayers += delta;
            if (Timeline.VisibleVideoLayers < 1) Timeline.VisibleVideoLayers = 1;
        }
        if (clientPoint.Y > Middle)
        {
            // Audio
            Timeline.VisibleAudioLayers += delta;
            if (Timeline.VisibleAudioLayers < 1) Timeline.VisibleAudioLayers = 1;
        }
    }
    private void ZoomX(int delta, Point clientPoint, double currentTime)
    {
        if (delta > 0)
        {
            for (int i = 0; i < delta; i++)
            {
                Timeline.VisibleWidth -= Timeline.VisibleWidth / 10;
            }
        }
        if (delta < 0)
        {
            for (int i = 0; i < delta * -1; i++)
            {
                Timeline.VisibleWidth += Timeline.VisibleWidth / 10;
            }
        }
    }

    private void SetupScrollbar()
    {
        var max = Timeline.AudioClips.Any() ? Timeline.AudioClips.Max(a => a.TimelineEndInSeconds) : Timeline.VisibleStart + Timeline.VisibleWidth;
        max = Math.Max(max, Timeline.VisibleStart + Timeline.VisibleWidth);
        scrollBarControl.Minimum = 0;
        scrollBarControl.Maximum = Convert.ToInt32(Math.Ceiling(max));
        scrollBarControl.SmallChange = 1;
        scrollBarControl.LargeChange = Convert.ToInt32(Math.Floor(Timeline.VisibleWidth));
    }
    private void ScrollBarControl_Scroll(object sender, ScrollEventArgs e)
    {
        Timeline.VisibleStart = e.NewValue;
        Invalidate();
        //scrollBarControl.Value += e.NewValue - e.OldValue;
        //int timelineLengthInPixels = (int)(Timeline.VideoClips.Max(c => c.TimelineEndInSeconds) * VisibleWidth);
        //scrollBarControl.Maximum = Math.Max(0, timelineLengthInPixels - Width);
    }

    private void TimelineControl_Resize(object sender, EventArgs e)
    {
        scrollBarControl.Left = 0;
        scrollBarControl.Top = ClientRectangle.Height - scrollBarControl.Height;
        scrollBarControl.Width = ClientRectangle.Width;
        Invalidate();
    }

    private void TimelineControl_DragEnter(object sender, DragEventArgs e)
    {
        var fullNames = GetDragAndDropFiles(e);
        if (fullNames.Length == 0)
        {
            ClearTemp();
            return;
        }

        var timelinePosition = TranslateToTimelinePosition(e);
        if (timelinePosition == null)
        {
            ClearTemp();
            return;
        }

        e.Effect = DragDropEffects.Copy;

        var currentTime = timelinePosition.CurrentTime;
        var layerIndex = timelinePosition.Layer;
        foreach (var fullName in fullNames)
        {
            var file = new File(fullName);
            if (file.Duration == null) continue;

            var start = currentTime;
            currentTime += file.Duration.Value;
            var layer = layerIndex;
            foreach (var videoStream in file.VideoStreams)
            {
                var clip = new TimelineClipVideo(Timeline, videoStream, start, layerIndex)
                {
                    TimelineEndInSeconds = currentTime,
                    ClipStartInSeconds = 0,
                    ClipEndInSeconds = file.Duration.Value
                };
                TempTimelineClipVideos.Add(clip);
                layer++;
            }

            layer = 0;
            foreach (var audioStream in file.AudioStreams)
            {
                var clip = new TimelineClipAudio(Timeline, audioStream, start, layerIndex)
                {
                    TimelineEndInSeconds = currentTime,
                    ClipStartInSeconds = 0,
                    ClipEndInSeconds = file.Duration.Value
                };
                TempTimelineClipAudios.Add(clip);
                layer++;
            }

            TempFiles.Add(file);
        }
    }
    private void TimelineControl_DragOver(object sender, DragEventArgs e)
    {
        var fullNames = GetDragAndDropFiles(e);
        if (fullNames.Length == 0)
        {
            ClearTemp(); 
            return;
        }

        var timelinePosition = TranslateToTimelinePosition(e);
        if (timelinePosition == null)
        {
            ClearTemp();
            return;
        }

        SyncTempFiles(fullNames, timelinePosition);

        Invalidate();
        //SetupScrollbar();

    }
    private void TimelineControl_DragDrop(object sender, DragEventArgs e)
    {
        ClearTemp();

        //var fullNames = GetDragAndDropFiles(e);
        //if (fullNames.Length == 0) return;
        //var files = fullNames.Select(a => new File(a));

        //var timelinePosition = TranslateToTimelinePosition(e);
        //if (timelinePosition == null) return;

        //Engine.Timeline.AddFiles(files, timelinePosition.CurrentTime, timelinePosition.Layer);

        Invalidate();
        SetupScrollbar();
    }

    private void ClearTemp()
    {
        TempFiles.Clear();
        TempTimelineClipAudios.Clear();
        TempTimelineClipVideos.Clear();
    }

    private void SyncTempFiles(string[] fullNames, TimelinePosition timelinePosition)
    {
        foreach (var tempFile in TempFiles)
        {
            if (!fullNames.Contains(tempFile.FullName))
            {
                var videoclips = TempTimelineClipVideos.Where(a => a.StreamInfo.File == tempFile);
                foreach (var videoclip in videoclips)
                    TempTimelineClipVideos.Remove(videoclip);

                var audioclips = TempTimelineClipAudios.Where(a => a.StreamInfo.File == tempFile);
                foreach (var audioclip in audioclips)
                    TempTimelineClipAudios.Remove(audioclip);

                TempFiles.Remove(tempFile);
            }
        }

        foreach (var fullName in fullNames)
        {
            var tempFile = TempFiles.FirstOrDefault(a => a.FullName == fullName);
            if (tempFile == null)
            {
                var file = new File(fullName);

                foreach (var videoStream in file.VideoStreams)
                    TempTimelineClipVideos.Add(new TimelineClipVideo(Timeline, videoStream, timelinePosition.CurrentTime, timelinePosition.Layer));
                foreach (var audioStream in file.AudioStreams)
                    TempTimelineClipAudios.Add(new TimelineClipAudio(Timeline, audioStream, timelinePosition.CurrentTime, timelinePosition.Layer));

                TempFiles.Add(file);
            }
            else
            {

            }
        }
    }

    private void TimelineControl_MouseDown(object sender, MouseEventArgs e)
    {

    }
    private void TimelineControl_MouseMove(object sender, MouseEventArgs e)
    {

    }
    private void TimelineControl_MouseUp(object sender, MouseEventArgs e)
    {

    }

    private string[] GetDragAndDropFiles(DragEventArgs e)
    {
        if (e == null || e.Data == null) return [];
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return [];
        var filesobj = e.Data.GetData(DataFormats.FileDrop);
        if (filesobj == null) return [];
        var files = (string[])filesobj;
        if (files == null) return [];
        return CheckFileType.Filter(files)
            .OrderBy(a => a)
            .ToArray();
    }
    private TimelinePosition? TranslateToTimelinePosition(DragEventArgs e)
    {
        var applicationPoint = new Point(e.X, e.Y);
        var clientPoint = PointToClient(applicationPoint);
        var currentTime = Timeline.VisibleStart + Timeline.VisibleWidth * clientPoint.X / ClientRectangle.Width;

        // Bepaal de hoogte van de video- en audiotijdlijn
        var timelineHeight = TimelineRectangle.Height;
        var videoHeight = timelineHeight / 2;  // Bovenste helft is voor video
        var audioHeight = timelineHeight - videoHeight;  // Onderste helft is voor audio

        // Bepaal of de muis boven de videolaag of de audiolaag is
        if (clientPoint.Y < videoHeight)
        {
            // Video tijdlijn
            var videoLayerHeight = videoHeight / Timeline.VisibleVideoLayers;
            var layerIndex = (int)(clientPoint.Y / videoLayerHeight); // Bepaal de laag op de video tijdlijn
            return new TimelinePosition(MediaFormat.Video, currentTime, layerIndex);
        }
        else if (clientPoint.Y >= videoHeight && clientPoint.Y < timelineHeight)
        {
            // Audio tijdlijn
            var audioLayerHeight = audioHeight / Timeline.VisibleAudioLayers;
            var layerIndex = (int)((clientPoint.Y - videoHeight) / audioLayerHeight); // Bepaal de laag op de audio tijdlijn
            return new TimelinePosition(MediaFormat.Video, currentTime, layerIndex);
        }

        return null;
    }

    private int TranslateToLayerIndex(Point clientPoint)
    {
        var layerIndex = -1; // Standaard waarde wanneer geen laag wordt gevonden

        // Bepaal de hoogte van de video- en audiotijdlijn
        var timelineHeight = TimelineRectangle.Height;
        var videoHeight = timelineHeight / 2;  // Bovenste helft is voor video
        var audioHeight = timelineHeight - videoHeight;  // Onderste helft is voor audio

        // Bepaal of de muis boven de videolaag of de audiolaag is
        if (clientPoint.Y < videoHeight)
        {
            // Video tijdlijn
            var videoLayerHeight = videoHeight / Timeline.VisibleVideoLayers;
            layerIndex = (int)(clientPoint.Y / videoLayerHeight); // Bepaal de laag op de video tijdlijn
        }
        else if (clientPoint.Y >= videoHeight && clientPoint.Y < timelineHeight)
        {
            // Audio tijdlijn
            var audioLayerHeight = audioHeight / Timeline.VisibleAudioLayers;
            layerIndex = (int)((clientPoint.Y - videoHeight) / audioLayerHeight); // Bepaal de laag op de audio tijdlijn
        }

        return layerIndex;
    }

    private double TranslateToCurrentTime(Point clientPoint)
    {
        return Timeline.VisibleStart + Timeline.VisibleWidth * clientPoint.X / ClientRectangle.Width;
    }
}

public class TimelinePosition
{
    public TimelinePosition(MediaFormat mediaFormat, double currentTime, int layerIndex)
    {
        MediaFormat = mediaFormat;
        CurrentTime = currentTime;
        Layer = layerIndex;
    }

    public MediaFormat MediaFormat { get; }
    public double CurrentTime { get; }
    public int Layer { get; }
}
