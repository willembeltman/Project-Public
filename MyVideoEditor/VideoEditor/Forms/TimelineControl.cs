using VideoEditor.Static;

namespace VideoEditor.Forms;

public partial class TimelineControl : UserControl
{
    public TimelineControl()
    {
        InitializeComponent();
        Engine.TimelineControl = this;
    }

    private double VisibleWidth { get; set; } = 100;
    private double VisibleStart { get; set; } = 0;
    private double PlayerPosition { get; set; } = 0;

    private int FirstVisibleVideoLayer { get; set; } = 0;
    private int FirstVisibleAudioLayer { get; set; } = 0;
    private int VisibleVideoLayers { get; set; } = 3;
    private int VisibleAudioLayers { get; set; } = 3;
    private int Middle { get; set; } = 0;

    //List<File> TempFiles { get; set; } = new List<File>();

    readonly int MiddleOffset = 10;
    readonly int TextSize = 8;
    int VideoBlockHeight => (Middle - MiddleOffset) / VisibleVideoLayers;
    int AudioBlockHeight => (Middle - MiddleOffset) / VisibleAudioLayers;

    private void TimelineControl_Load(object sender, EventArgs e)
    {
        SetupScrollbar();
    }
    private void TimelineControl_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Black);

        // zoek het midden
        var height = ClientRectangle.Height - scrollBarControl.Height;
        Middle = height / 2;

        // Teken tijdsmarkeringen
        DrawTimeMarkers(g);

        // Teken clips
        DrawVideoClips(g);
    }

    private void SetupScrollbar()
    {
        var max = Engine.Timeline.AudioClips.Any() ? Engine.Timeline.AudioClips.Max(a => a.TimelineEndInSeconds) : VisibleStart + VisibleWidth;
        max = Math.Max(max, VisibleStart + VisibleWidth);
        scrollBarControl.Minimum = 0;
        scrollBarControl.Maximum = Convert.ToInt32(Math.Ceiling(max));
        scrollBarControl.SmallChange = 1;
        scrollBarControl.LargeChange = Convert.ToInt32(Math.Floor(VisibleWidth));
    }

    private void DrawTimeMarkers(Graphics g)
    {
        using var pen = new Pen(Color.FromArgb(0, 0, 128), 1);
        using var pen2 = new Pen(Color.FromArgb(64, 0, 0), 1);
        using var font = new Font("Arial", TextSize);
        using var brush = new SolidBrush(Color.White);


        for (var i = 0; i < VisibleVideoLayers; i++)
        {
            var y = Middle - i * VideoBlockHeight - MiddleOffset;
            g.DrawLine(pen2, 0, y, Width, y);

            var text = $"{i + FirstVisibleVideoLayer}";
            var textSize = g.MeasureString(text, font);
            var textY = y - VideoBlockHeight / 2 - (int)(textSize.Height / 2);
            g.DrawString(text, font, brush, 2, textY);
        }
        for (var i = 0; i < VisibleAudioLayers; i++)
        {
            var y = Middle + i * AudioBlockHeight + MiddleOffset;
            g.DrawLine(pen2, 0, y, Width, y);

            var text = $"{i + FirstVisibleAudioLayer}";
            var textSize = g.MeasureString(text, font);
            var textY = y + VideoBlockHeight / 2 - (int)(textSize.Height / 2);
            g.DrawString(text, font, brush, 2, textY);
        }

        var timeIncrease = 0.01D;
        var decimals = 2;
        while (Width / VisibleWidth * timeIncrease < 50)
        {
            timeIncrease *= 10;
            decimals--;
        }
        if (decimals < 0) decimals = 0;

        for (var sec = 0D; sec < double.MaxValue; sec += timeIncrease)
        {
            var x = Convert.ToInt32((sec - VisibleStart) / VisibleWidth * Width);
            if (x >= Width) break;
            g.DrawLine(pen, x, 0, x, Height);

            var text = $"{sec.ToString("F" + decimals)}s";
            var textSize = g.MeasureString(text, font);
            var textY = Middle - (int)(textSize.Height / 2);
            g.DrawString(text, font, brush, x + 2, textY);
        }

    }
    private void DrawVideoClips(Graphics g)
    {
        foreach (var clip in Engine.Timeline.VideoClips)
        {
            int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - VisibleStart) / VisibleWidth * Width);
            int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - VisibleStart) / VisibleWidth * Width);
            int width = x2 - x1;
            if (x1 > Width || x2 < 0) continue; // Clip buiten zichtbare range

            var layer = clip.Layer - FirstVisibleVideoLayer;
            if (layer < 0 || layer > VisibleVideoLayers) continue;
            int y = Middle - MiddleOffset - VideoBlockHeight - layer * VideoBlockHeight;

            var rect = new Rectangle(x1, y + Constants.Margin / 2, width, VideoBlockHeight - Constants.Margin);
            g.FillRectangle(Brushes.Blue, rect);
            g.DrawRectangle(Pens.White, rect);
        }

        foreach (var clip in Engine.Timeline.AudioClips)
        {
            int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - VisibleStart) / VisibleWidth * Width);
            int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - VisibleStart) / VisibleWidth * Width);
            int width = x2 - x1;
            if (x1 > Width || x2 < 0) continue; // Clip buiten zichtbare range

            var layer = clip.Layer - FirstVisibleAudioLayer;
            if (layer < 0 || layer > VisibleAudioLayers) continue;
            int y = Middle + MiddleOffset + (clip.Layer - FirstVisibleAudioLayer) * AudioBlockHeight;

            var rect = new Rectangle(x1, y + Constants.Margin / 2, width, AudioBlockHeight - Constants.Margin);
            g.FillRectangle(Brushes.Blue, rect);
            g.DrawRectangle(Pens.White, rect);
        }
    }

    //private IEnumerable<TimelineVideoClipControl> TimelineVideoClipControls()
    //{
    //    foreach (var control in this.Controls)
    //    {
    //        if (control is TimelineVideoClipControl)
    //        {
    //            yield return (TimelineVideoClipControl)control;
    //        }
    //    }
    //}

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

    private void ScrollY(int delta, Point clientPoint, double currentTime)
    {
        if (clientPoint.Y < Middle)
        {
            // Video
            FirstVisibleVideoLayer += delta;
            if (FirstVisibleVideoLayer < 0) FirstVisibleVideoLayer = 0;
        }
        if (clientPoint.Y > Middle)
        {
            // Audio
            FirstVisibleAudioLayer += delta;
            if (FirstVisibleAudioLayer < 0) FirstVisibleAudioLayer = 0;
        }
    }
    private void ZoomY(int delta, Point clientPoint, double currentTime)
    {
        if (clientPoint.Y < Middle)
        {
            // Video
            VisibleVideoLayers += delta;
            if (VisibleVideoLayers < 1) VisibleVideoLayers = 1;
        }
        if (clientPoint.Y > Middle)
        {
            // Audio
            VisibleAudioLayers += delta;
            if (VisibleAudioLayers < 1) VisibleAudioLayers = 1;
        }
    }

    private void ZoomX(int delta, Point clientPoint, double currentTime)
    {
        if (delta > 0)
        {
            for (int i = 0; i < delta; i++)
            {
                VisibleWidth -= VisibleWidth / 10;
            }
        }
        if (delta < 0)
        {
            for (int i = 0; i < delta * -1; i++)
            {
                VisibleWidth += VisibleWidth / 10;
            }
        }
    }

    int OldSmallScrollDelta = 0;
    int TotalBigScrollDelta = 0;

    private int GetScrollDelta(MouseEventArgs e)
    {
        TotalBigScrollDelta += e.Delta;

        if (TotalBigScrollDelta / 120 == OldSmallScrollDelta)
            return 0;

        var delta = TotalBigScrollDelta / 120 - OldSmallScrollDelta;
        OldSmallScrollDelta = TotalBigScrollDelta / 120;
        return delta;
    }

    private void ScrollBarControl_Scroll(object sender, ScrollEventArgs e)
    {
        VisibleStart = e.NewValue;
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
        var files = GetDragAndDropFiles(e);
        if (files.Length == 0) return;
        e.Effect = DragDropEffects.Copy;
        TimelineControl_DragOver(sender, e);
    }
    private void TimelineControl_DragOver(object sender, DragEventArgs e)
    {
        //var fullNames = GetDragAndDropFiles(e);
        //if (fullNames.Length == 0) return;
        //SyncTempFiles(fullNames);

        //var clientPoint = PointToClient(new Point(e.X, e.Y));
        //var currentTime = TranslateToCurrentTime(clientPoint);


        //Invalidate();
        //SetupScrollbar();

    }
    private void TimelineControl_DragDrop(object sender, DragEventArgs e)
    {
        var fullNames = GetDragAndDropFiles(e);
        if (fullNames.Length == 0) return;
        var files = fullNames.Select(a => new File(a));

        var clientPoint = PointToClient(new Point(e.X, e.Y));
        var currentTime = TranslateToCurrentTime(clientPoint);

        Engine.Timeline.AddFiles(currentTime, 0, files);

        Invalidate();
        SetupScrollbar();
    }

    //private void SyncTempFiles(string[] fullNames)
    //{
    //    foreach (var tempFile in TempFiles)
    //    {
    //        if (!fullNames.Contains(tempFile.FullName))
    //        {
    //            TempFiles.Remove(tempFile);
    //        }
    //    }

    //    foreach (var fullName in fullNames)
    //    {
    //        if (!TempFiles.Any(a => a.FullName == fullName))
    //        {
    //            TempFiles.Add(new File(fullName));
    //        }
    //    }
    //}

    private static string[] GetDragAndDropFiles(DragEventArgs e)
    {
        if (e == null || e.Data == null) return [];
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return [];
        var filesobj = e.Data.GetData(DataFormats.FileDrop);
        if (filesobj == null) return [];
        var files = (string[])filesobj;
        if (files == null) return [];
        return CheckFileType.Filter(files).ToArray();
    }

    private double TranslateToCurrentTime(Point clientPoint)
    {
        return VisibleStart + VisibleWidth * clientPoint.X / ClientRectangle.Width;
    }

}
