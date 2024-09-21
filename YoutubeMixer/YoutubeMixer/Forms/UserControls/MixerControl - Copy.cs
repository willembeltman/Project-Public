namespace YoutubeMixer.UserControls
{
    public partial class MixerControl : UserControl
    {
        public MixerControl()
        {
            InitializeComponent();

            LeftMixerChannel = new MixerChannel(this, FaderLeft, FaderCross, 0, 0.5, vuMeterLeft);
            RightMixerChannel = new MixerChannel(this, FaderRight, FaderCross, 1, 0.5, vuMeterRight);
        }

        public MixerChannel LeftMixerChannel { get; }
        public MixerChannel RightMixerChannel { get; }

        private void FaderLeft_Scroll(object sender, EventArgs e)
            => LeftMixerChannel.FaderChanged(
                Convert.ToDouble(FaderLeft.Value) / FaderLeft.Maximum,
                Convert.ToDouble(FaderCross.Value) / FaderCross.Maximum);

        private void FaderRight_Scroll(object sender, EventArgs e) 
            => RightMixerChannel.FaderChanged(
                Convert.ToDouble(FaderRight.Value) / FaderRight.Maximum,
                Convert.ToDouble(FaderCross.Value) / FaderCross.Maximum);

        private void FaderCross_Scroll(object sender, EventArgs e)
            => RightMixerChannel.CrossFaderChanged(
                Convert.ToDouble(FaderCross.Value) / FaderCross.Maximum);

        private void EqControlLeft_ValueChanged(object sender, EventArgs e)
        {
            LeftMixerChannel.EqualizerChanged(
                EqBassControlLeft.Value,
                EqMidControlLeft.Value,
                EqHighControlLeft.Value);
        }

        private void EqControlRight_ValueChanged(object sender, EventArgs e)
        {
            RightMixerChannel.EqualizerChanged(
                EqBassControlRight.Value,
                EqMidControlRight.Value,
                EqHighControlRight.Value);

        }
    }
}
