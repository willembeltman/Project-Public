using VideoEditor.Forms;

namespace VideoEditor;

public static class Engine
{
    public static Project Project { get; set; } = new Project();
    public static Timeline Timeline => Project.CurrentTimeline;

    public static TimelineControl TimelineControl { get; internal set; }
    public static DisplayControl DisplayControl { get; internal set; }
    public static MainForm MainForm { get; internal set; }
    public static PropertiesControl PropertiesControl { get; internal set; }

    public static void Start()
    {
        //throw new NotImplementedException();
    }
}
