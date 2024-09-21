using YoutubeMixer.Library.Models;

namespace YoutubeMixer.Library.Interfaces
{
    public interface IAudioOutput
    {
        void ReceivedAudioData(AudioData audioData);
    }
}