namespace YoutubeMixer.Library.Interfaces
{
    public interface IAudioSource
    {
        void PlayPause();

        string? Title { get; }
        bool IsReady { get; }
        bool IsPlaying { get; }
        double CurrentTime { get; }
        double TotalDuration { get; }
        double VuMeter { get; }

        double Volume { get; set; }
        double BassVolume { get; set; }
        double MidVolume { get; set; }
        double HighVolume { get; set; }
        double PlaybackSpeed { get; set; }
        bool PitchControl { get; set; }
        bool KickDetected { get; }
    }
}