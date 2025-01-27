namespace VideoEditor;

public class Fps
{
    public Fps()
    {
        Base = 25;
        Divider = 1;
    }
    public Fps(long @base, long divider)
    {
        Base = @base;
        Divider = divider;
    }

    public long Base { get; set; }
    public long Divider { get; set; }
}

