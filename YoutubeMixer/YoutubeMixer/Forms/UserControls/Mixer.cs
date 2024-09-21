namespace YoutubeMixer.UserControls
{
    public partial class Mixer : UserControl
    {
        public Mixer()
        {
            InitializeComponent();
        }

        internal void InitializeDraw()
        {
            LeftMixerChannel.InitializeDraw();
            RightMixerChannel.InitializeDraw();
        }

        private void Mixer_Load(object sender, EventArgs e)
        {
            Mixer_Resize(sender, e);
        }

        private void Mixer_Resize(object sender, EventArgs e)
        {
            var offset = 10;

            LeftMixerChannel.Top = 0;
            LeftMixerChannel.Left = 0;
            LeftMixerChannel.Width = ClientRectangle.Width / 2;
            LeftMixerChannel.Height = ClientRectangle.Height - FaderCross.Height + offset;

            RightMixerChannel.Top = 0;
            RightMixerChannel.Left = LeftMixerChannel.Right;
            RightMixerChannel.Width = ClientRectangle.Width / 2;
            RightMixerChannel.Height = ClientRectangle.Height - FaderCross.Height + offset;

            FaderCross.Top = LeftMixerChannel.Bottom + 5;
            FaderCross.Left = 0;
            FaderCross.Width = ClientRectangle.Width;
        }
    }
}
