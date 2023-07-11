namespace YoutubeMixer.UserControls
{
    public partial class Mixer : UserControl
    {
        public Mixer()
        {
            InitializeComponent();

            LeftMixerChannel.Mixer = this;
            RightMixerChannel.Mixer = this;
        }
    }
}
