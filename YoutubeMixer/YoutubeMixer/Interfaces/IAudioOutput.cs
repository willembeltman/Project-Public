using YoutubeMixer.AudioSources;
using YoutubeMixer.Models;

namespace YoutubeMixer.Interfaces
{
    public interface IAudioOutput
    {
        void ReceivedAudioData(AudioData audioData);
    }
}