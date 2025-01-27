namespace VideoEditor.Forms;

public partial class PropertiesControl : UserControl
{
    public PropertiesControl()
    {
        InitializeComponent();
        BackColor = Color.White;
    }
    Engine? Engine { get; set; }

    public void SetEngine(Engine engine)
    {
        Engine = engine;
        Engine.SetPropertiesControl(this);
    }

    private void PropertiesControl_Load(object sender, EventArgs e)
    {

    }

    private void PropertiesControl_Resize(object sender, EventArgs e)
    {
        vScrollBar1.Top = 0;
        vScrollBar1.Left = ClientRectangle.Width - vScrollBar1.Width;
        vScrollBar1.Height = ClientRectangle.Height;
    }
}
