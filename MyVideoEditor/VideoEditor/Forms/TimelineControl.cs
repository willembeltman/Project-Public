using System.Reflection;
using VideoEditor.Static;
using System.Linq;
using System.ComponentModel;

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
    private int VisibleVideoLayers { get; set; } = 4;
    private int VisibleAudioLayers { get; set; } = 4;
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
        using var pen = new Pen(Color.Gray, 1);
        using var pen2 = new Pen(Color.FromArgb(64, 64, 64), 1);
        using var font = new Font("Arial", TextSize);
        using var brush = new SolidBrush(Color.White);


        for (var i = 0; i < VisibleVideoLayers; i++)
        {
            var y = Middle - i * VideoBlockHeight - MiddleOffset;
            g.DrawLine(pen2, 0, y, Width, y);
        }
        for (var i = 0; i < VisibleAudioLayers; i++)
        {
            var y = Middle + i * AudioBlockHeight + MiddleOffset;
            g.DrawLine(pen2, 0, y, Width, y);
        }

        var add = 0.01D;
        var f = 2;
        while (Width / VisibleWidth * add < 50)
        {
            add *= 10;
            f--;
        }
        if (f < 0) f = 0;

        for (var sec = 0D; sec < int.MaxValue; sec += add)
        {
            var x = Convert.ToInt32((sec - VisibleStart) / VisibleWidth * Width);
            if (x >= Width) break;

            g.DrawLine(pen, x, 0, x, Height);

            // Bereken de hoogte van de tekst
            var text = $"{sec.ToString("F" + f)}s";
            var textSize = g.MeasureString(text, font);

            // Zet de tekst verticaal in het midden van de opgegeven middle
            var textY = Middle - (int)(textSize.Height / 2);

            // Teken de tekst
            g.DrawString(text, font, brush, x + 2, textY);
        }

    }
    private void DrawVideoClips(Graphics g)
    {
        //var memoryVideoClips = Engine.Timeline.VideoClips;
        //foreach (var control in TimelineVideoClipControls())
        //{
        //    if (!memoryVideoClips.Contains(control.VideoClip))
        //    {
        //        Controls.Remove(control);
        //    }
        //}

        //var drawnVideoClips = TimelineVideoClipControls().Select(a => a.VideoClip).ToArray();
        //foreach (var clip in memoryVideoClips)
        //{
        //    if (!drawnVideoClips.Contains(clip))
        //    {
        //        Controls.Add(new TimelineVideoClipControl(this, clip));
        //    }
        //}

        //foreach (var clip in TimelineVideoClipControls())
        //{
        //    clip.Setup();
        //}


        foreach (var clip in Engine.Timeline.VideoClips)
        {
            int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - VisibleStart) / VisibleWidth * Width);
            int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - VisibleStart) / VisibleWidth * Width);
            int width = x2 - x1;
            if (x1 > Width || x2 < 0) continue; // Clip buiten zichtbare range

            int y = Middle - MiddleOffset - VideoBlockHeight - clip.Layer * VideoBlockHeight + Constants.Margin / 2;

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

            int y = Middle + MiddleOffset + clip.Layer * AudioBlockHeight;

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
        var delta = GetDelta(e);
        if ((ModifierKeys & Keys.Control) == Keys.Control)
        {
            ZoomX(delta);
        }
        else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            ZoomY(e, delta);
        }
        else
        {
            // Normaal op en neer scrollen
        }
        Invalidate();
        SetupScrollbar();
    }

    private void ZoomY(MouseEventArgs e, int delta)
    {
        var clientPoint = new Point(e.X, e.Y);
        var currentTime = TranslateToCurrentTime(clientPoint);
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

    private void ZoomX(int delta)
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

    private int GetDelta(MouseEventArgs e)
    {
        var divider = 1;
        if (e.Delta >= 120 || e.Delta <= -120) divider = 120;
        return e.Delta / divider;
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
