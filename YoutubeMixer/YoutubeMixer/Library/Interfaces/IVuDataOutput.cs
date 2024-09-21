namespace YoutubeMixer.Library.Interfaces
{
    public interface IVuDataOutput
    {
        void ReceivedVuChunk(double currentTime, double previousTime, double vuMeter);
    }
}
