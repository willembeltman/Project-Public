namespace VideoEditor;

public class VideoInfo
{
    public VideoInfo(Size? resolution = null, Fps? fps = null)
    {
        Resolution = resolution ?? new Size(1920, 1080);
        Fps = fps ?? new Fps(25, 1);
    }
    public Size Resolution { get; set; }
    public Fps Fps { get; set; }
}

