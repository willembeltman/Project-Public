using YoutubeMixer.Controls;

namespace YoutubeMixer.UserControls
{
    public partial class MixerChannel : UserControl
    {
        public MixerChannel()
        {
            InitializeComponent();
        }

        public Mixer? Mixer { get; set; }
        public YoutubeController? Controller { get; set; }

        // Controller => Controls
        public void SetVuMeter(double vuMeter)
        {
            VuMeter.Value = vuMeter;
            VuMeter.UpdateDisplay();
        }

        // Controls => Controller
        private void Fader_Scroll(object sender, EventArgs e)
            => Controller?.SetVolume(
                Convert.ToDouble(Fader.Value) / Fader.Maximum);
        private void EqControl_ValueChanged(object sender, EventArgs e)
        {
            Controller?.SetEqualizer(
                EqBassControl.Value,
                EqMidControl.Value,
                EqHighControl.Value);
        }
    }
}
