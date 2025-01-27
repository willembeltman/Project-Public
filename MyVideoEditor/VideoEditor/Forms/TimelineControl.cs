using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace VideoEditor.Forms;

public partial class TimelineControl : UserControl
{
    public TimelineControl()
    {
        InitializeComponent();
        BackColor = Color.White;
        DoubleBuffered = true;
    }

    private double VisibleWidth { get; set; } = 100;
    private double VisibleStart { get; set; } = 0;
    private double PlayerPosition { get; set; } = 0;

    Engine? Engine { get; set; }
    Project? Project => Engine?.Project;
    Timeline? Timeline => Engine?.Timeline;
    public void SetEngine(Engine engine)
    {
        Engine = engine;
        Engine.SetTimelineControl(this);
    }

    private void TimelineControl_Load(object sender, EventArgs e)
    {
        //if (Engine.Timeline.VideoClips.Values.)
        //scrollBarControl.Maximum = Convert.ToInt32(Engine.Timeline.VideoClips.Values.Max(a => a.TimelineEndInSeconds));
        scrollBarControl.Minimum = 0;
    }
    private void TimelineControl_Paint(object? sender, PaintEventArgs e)
    {
        if (Timeline == null)
            return;

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Black);

        //scrollBarControl.Maximum = 

        //VisibleStart = scrollBarControl.Value;

        // Teken tijdsmarkeringen
        DrawTimeMarkers(g);

        // Teken clips
        DrawVideoClips(g);
    }
    private void DrawTimeMarkers(Graphics g)
    {
        using var pen = new Pen(Color.Gray, 1);
        using var font = new Font("Arial", 8);
        using var brush = new SolidBrush(Color.White);

        var add = 0.01D;
        var f = 2;
        while (Width / VisibleWidth * add < 50)
        {
            add *= 10;
            f--;
        }
        if (f < 0) f = 0;

        for (var sec = 0D; sec < int.MaxValue; sec += add) // Beperkt aantal om te testen
        {
            var x = Convert.ToInt32((sec - VisibleStart) / VisibleWidth * Width);
            if (x >= Width) break;

            g.DrawLine(pen, x, 0, x, Height);
            g.DrawString($"{sec.ToString("F" + f)}s", font, brush, x + 2, 2);
        }
    }
    private void DrawVideoClips(Graphics g)
    {
        if (Timeline == null) return;

        foreach (var clip in Timeline.VideoClips)
        {
            int x1 = Convert.ToInt32((clip.TimelineStartInSeconds - VisibleStart) / VisibleWidth * Width);
            int x2 = Convert.ToInt32((clip.TimelineEndInSeconds - VisibleStart) / VisibleWidth * Width);
            int width = x2 - x1;
            if (x1 > Width || x2 < 0) continue; // Clip buiten zichtbare range

            var rect = new Rectangle(x1, 20, width, 40);
            g.FillRectangle(Brushes.Blue, rect);
            g.DrawRectangle(Pens.White, rect);
        }
    }
    private void TimelineControl_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (e.Delta > 0)
        {
            for (int i = 0; i < e.Delta / 120; i++)
            {
                VisibleWidth += VisibleWidth / 10;
            }
        }
        if (e.Delta < 0)
        {
            for (int i = 0; i < e.Delta / -120; i++)
            {
                VisibleWidth -= VisibleWidth / 10;
            }
        }
        Invalidate();
    }
    private void ScrollBarControl_Scroll(object sender, ScrollEventArgs e)
    {
        if (Timeline == null) return;

        int timelineLengthInPixels = (int)(Timeline.VideoClips.Max(c => c.TimelineEndInSeconds) * VisibleWidth);
        scrollBarControl.Maximum = Math.Max(0, timelineLengthInPixels - Width);
    }
    private void TimelineControl_Resize(object sender, EventArgs e)
    {
        scrollBarControl.Left = 0;
        scrollBarControl.Top = ClientRectangle.Height - scrollBarControl.Height;
        scrollBarControl.Width = ClientRectangle.Width;
    }
    private void TimelineControl_DragEnter(object sender, DragEventArgs e)
    {
        var files = GetDragAndDropFiles(e);
        if (files.Length == 0) return;
        e.Effect = DragDropEffects.Copy;
        VisualizeInsertVideos(e, files);
    }
    private void TimelineControl_DragOver(object sender, DragEventArgs e)
    {
        var files = GetDragAndDropFiles(e);
        if (files.Length == 0) return;
        VisualizeInsertVideos(e, files);
    }
    private void TimelineControl_DragDrop(object sender, DragEventArgs e)
    {
        var files = GetDragAndDropFiles(e);
        if (files.Length == 0) return;
        InsertVideos(e, files);
    }
    private static string[] GetDragAndDropFiles(DragEventArgs e)
    {
        if (e == null || e.Data == null) return [];

        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return [];

        var filesobj = e.Data.GetData(DataFormats.FileDrop);
        if (filesobj == null) return [];

        var files = (string[])filesobj;
        if (files == null) return [];

        return files;
    }

    private void VisualizeInsertVideos(DragEventArgs e, string[] fullNames)
    {
        Point clientPoint = this.PointToClient(new Point(e.X, e.Y));
        int x = clientPoint.X;
        int y = clientPoint.Y;
        Debug.WriteLine($"X={x} Y={y}");

        // En dan?
    }
    private void InsertVideos(DragEventArgs e, string[] fullNames)
    {
        if (Engine == null) return;
        if (Project == null) return;
        if (Timeline == null) return;

        fullNames = fullNames.OrderBy(a => a).ToArray();

        var clientPoint = PointToClient(new Point(e.X, e.Y));
        var currentTime = TranslateToCurrentTime(clientPoint);
        var files = File.Open(fullNames);

        var pos = currentTime;
        foreach (var file in files)
        {
            if (file.Duration == null) continue;

            var start = pos;
            pos += file.Duration.Value;

            foreach (var videoStream in file.VideoStreams)
            {
                var videoClip = new TimelineVideoClip(Timeline, videoStream, start, pos, 0, file.Duration.Value);
                Timeline.VideoClips.Add(videoClip);
            }
            Project.Files.Add(file);
        }

        Invalidate();
    }

    private double TranslateToCurrentTime(Point clientPoint)
    {
        return VisibleStart + VisibleWidth * clientPoint.X / ClientRectangle.Width;        
    }
}
