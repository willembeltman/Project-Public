using VideoEditor.Forms;

namespace VideoEditor;

public class Engine
{
    public Project Project { get; private set; } = new Project();

    public MainForm? MainForm { get; private set; }
    public DisplayControl? DiplayControl { get; private set; }
    public TimelineControl? TimelineControl { get; private set; }
    public PropertiesControl? PropertiesControl { get; private set; }

    public Timeline Timeline => Project.CurrentTimeline;

    public void SetMainForm(MainForm mainForm) => MainForm = mainForm;
    public void SetDisplayControl(DisplayControl displayControl) => DiplayControl = displayControl;
    public void SetTimelineControl(TimelineControl timelineControl) => TimelineControl = timelineControl;
    public void SetPropertiesControl(PropertiesControl propertiesControl) => PropertiesControl = propertiesControl;

    public void Start()
    {
        //throw new NotImplementedException();
    }

}
