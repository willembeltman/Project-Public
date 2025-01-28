namespace VideoEditor.Info;

public class VideoInfo
{
    public VideoInfo(Resolution? resolution = null, Fps? fps = null)
    {
        Resolution = resolution ?? new Resolution(1920, 1080);
        Fps = fps ?? new Fps(25, 1);
    }
    public Resolution Resolution { get; set; }
    public Fps Fps { get; set; }
}

