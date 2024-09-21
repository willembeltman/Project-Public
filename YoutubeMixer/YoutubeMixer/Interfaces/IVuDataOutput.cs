using YoutubeMixer.AudioSources;

namespace YoutubeMixer.Interfaces
{
    public interface IVuDataOutput
    {
        void ReceivedVuChunk(double currentTime, double previousTime, double vuMeter);
    }
}
