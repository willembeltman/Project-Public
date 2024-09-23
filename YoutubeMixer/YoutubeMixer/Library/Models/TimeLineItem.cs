namespace YoutubeMixer.Library.Models
{
    public class TimeLineItem
    {
        public double Timestamp { get; set; }
        public double KickVolume { get; set; }
        public double LowVolume { get; set; }
        public double MidVolume { get; set; }
        public double HighVolume { get; set; }
        public double Volume { get; set; }
        public bool KickDetected { get; set; }
    }
}