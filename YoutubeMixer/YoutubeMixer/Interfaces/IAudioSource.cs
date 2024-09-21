namespace YoutubeMixer.Interfaces
{
    public interface IAudioSource
    {
        void Play();
        void Pause();

        bool IsPlaying { get; }
        double CurrentTime { get; }
        double TotalDuration {  get; }

        double Volume { get; set; }
        double BassVolume { get; set; }
        double MidVolume { get; set; }
        double HighVolume { get; set; }
        double PlaybackSpeed { get; set; }
        string? Title { get; }
        double VuMeter { get; }
    }
}