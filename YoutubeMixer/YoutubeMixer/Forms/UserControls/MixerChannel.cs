using YoutubeMixer.Library.Interfaces;

namespace YoutubeMixer.UserControls
{
    public partial class MixerChannel : UserControl
    {
        public MixerChannel()
        {
            InitializeComponent();
        }

        public void InitializeDraw()
        {
            VuMeter.InitializeDraw();
        }

        public IAudioSource? AudioSource { get { return VuMeter.AudioSource; } set { VuMeter.AudioSource = value; } }

        private void Fader_Scroll(object sender, EventArgs e)
        {
            if (AudioSource == null) return;
            AudioSource.Volume = Convert.ToDouble(Fader.Value) / Fader.Maximum;
        }
        private void EqControl_ValueChanged(object sender, EventArgs e)
        {
            if (AudioSource == null) return;
            AudioSource.BassVolume = EqBassControl.Value;
            AudioSource.MidVolume = EqMidControl.Value;
            AudioSource.HighVolume = EqHighControl.Value;
        }

        private void MixerChannel_Resize(object sender, EventArgs e)
        {
            EqHighControl.Top = 0;
            EqHighControl.Left = 0;
            EqHighControl.Width = ClientRectangle.Width;
            EqHighControl.Height = ClientRectangle.Width;
            EqMidControl.Top = EqHighControl.Bottom;
            EqMidControl.Left = 0;
            EqMidControl.Width = ClientRectangle.Width;
            EqMidControl.Height = ClientRectangle.Width;
            EqBassControl.Top = EqMidControl.Bottom;
            EqBassControl.Left = 0;
            EqBassControl.Width = ClientRectangle.Width;
            EqBassControl.Height = ClientRectangle.Width;
            VuMeter.Top = EqBassControl.Bottom;
            VuMeter.Left = 0;
            VuMeter.Height = ClientRectangle.Height - EqBassControl.Bottom;
            Fader.Top = EqBassControl.Bottom - 10;
            Fader.Left = VuMeter.Right;
            Fader.Width = ClientRectangle.Width - VuMeter.Right;
            Fader.Height = ClientRectangle.Height - EqBassControl.Bottom + 20;
        }

        private void MixerChannel_Load(object sender, EventArgs e)
        {
            MixerChannel_Resize(sender, e);
        }
    }
}
