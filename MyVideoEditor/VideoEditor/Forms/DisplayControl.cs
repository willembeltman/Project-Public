namespace VideoEditor.Forms;

public partial class DisplayControl : UserControl
{
    public DisplayControl()
    {
        InitializeComponent();
    }

    Engine? Engine { get; set; }

    public void SetEngine(Engine engine)
    {
        Engine = engine;
        Engine.SetDisplayControl(this);
        DisplayControl_Resize(this, new EventArgs());
    }

    private void DisplayControl_Load(object sender, EventArgs e)
    {

    }

    private void DisplayControl_Resize(object sender, EventArgs e)
    {
        //if (Engine == null) return;

        var width = ClientRectangle.Width;
        var height = ClientRectangle.Height;

        var screenWidthBasedOnHeight = height * (Engine?.Timeline.VideoInfo.Resolution.Width ?? 1920) / (Engine?.Timeline.VideoInfo.Resolution.Height ?? 1080);
        var screenHeightBasedOnWidth = width * (Engine?.Timeline.VideoInfo.Resolution.Height ?? 1080) / (Engine?.Timeline.VideoInfo.Resolution.Width ?? 1920);

        if (height > screenHeightBasedOnWidth)
        {
            height = screenHeightBasedOnWidth;
        }
        if (width > screenWidthBasedOnHeight)
        {
            width = screenWidthBasedOnHeight;
        }

        var offsetX = (ClientRectangle.Width - width) / 2;
        var offsetY = (ClientRectangle.Height - height) / 2;

        videoControl.Top = offsetY;
        videoControl.Left = offsetX;
        videoControl.Width = width;
        videoControl.Height = height;

        //videoElement.Height = videoElement.Width * Engine.Timeline.Settings.Resolution.Height / Engine.Timeline.Settings.Resolution.Width;

        //if (videoElement.Height > Convert.ToInt32((ClientRectangle.Height - Constants.Margin * 3) * VerdelingY))
        //{
        //    videoElement.Height = Convert.ToInt32((ClientRectangle.Height - Constants.Margin * 3) * VerdelingY);
        //    videoElement.Width = videoElement.Height * Engine.Timeline.Settings.Resolution.Width / Engine.Timeline.Settings.Resolution.Height;
        //    videoElement.Left = (ClientRectangle.Width - Constants.Margin * 2 - videoElement.Width) / 2 + Constants.Margin;
        //}
    }
}
