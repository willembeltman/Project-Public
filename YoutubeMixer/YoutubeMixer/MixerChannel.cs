using YoutubeMixer.Controls;
using YoutubeMixer.UserControls;

namespace YoutubeMixer
{
    public class MixerChannel
    {
        public MixerChannel(
            MixerControl mixer,
            TrackBar fader,
            TrackBar faderCross,
            double faderCrossMinValue,
            double faderCrossMaxValue,
            VuMeter vuMeter)
        {
            Mixer = mixer;
            Fader = fader;
            CrossFader = faderCross;
            CrossFaderMutedPosition = faderCrossMinValue;
            CrossFaderFullVolumePosition = faderCrossMaxValue;
            VuMeterControl = vuMeter;
        }

        public MixerControl Mixer { get; }

        private TrackBar Fader { get; }
        private TrackBar CrossFader { get; }
        private double CrossFaderMutedPosition { get; }
        private double CrossFaderFullVolumePosition { get; }
        private VuMeter VuMeterControl { get; }

        public YoutubeController? Controller { get; internal set; }

        // Controller => Controls
        public void SetVuMeter(double vuMeter)
        {
            VuMeterControl.Value = vuMeter;
            VuMeterControl.UpdateDisplay();
        }

        // Controls => Controller

        /// <summary>
        /// Will be called when the faders are changed
        /// </summary>
        /// <param name="channelFaderVolume">The volume of the fader (min = 0 max = 1)</param>
        /// <param name="crossFaderPosition">The volume of the crossfader (min = 0 max = 1)</param>
        public void FaderChanged(double channelFaderVolume, double crossFaderPosition)
        {
            var crossFaderMutedPosition = CrossFaderMutedPosition; // The start position for the crossfader for this channel, on the left deck this will be 0
            var crossFaderFullVolumePosition = CrossFaderFullVolumePosition; // The end position for the crossfader for this channel, on the left deck this will be 1

            // Calculate the volume for the current crossfader position
            var crossFaderVolume = 0; // TODO: Use the crossfaderValue, crossFaderStart and crossFaderEnd to calculate the volume that the crossfader will let through
            
            // Calculate the channel volume based on crossfader settings
            var calculatedVolume = channelFaderVolume * crossFaderVolume;

            // Set the volume to the player
            Controller?.SetVolume(calculatedVolume);
        }
        public void CrossFaderChanged(double crossfaderVolume)
        {
        }
        public void EqualizerChanged(double bassVolume, double midVolume, double highVolume)
        {
            Controller?.SetEqualizer(bassVolume, midVolume, highVolume);
        }

    }
}