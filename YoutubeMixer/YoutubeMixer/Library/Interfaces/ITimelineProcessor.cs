using YoutubeMixer.Library.Models;

namespace YoutubeMixer.Library.Interfaces
{
    public interface ITimelineProcessor
    {
        void ProcessTimelineData(double currentTime, double previousTime, TimeLineItem[] timelineItems);
    }
}
