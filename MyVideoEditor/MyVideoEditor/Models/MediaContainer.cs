using MyVideoEditor.Enums;
using MyVideoEditor.VideoObjects;
using System.Diagnostics;
using System.Linq;

namespace MyVideoEditor.Models
{
    public class MediaContainer
    {
        public MediaContainer(string fullName,
            StreamInfo[] videoInfos,
            StreamInfo[] audioInfos,
            VideoStreamReader[] videoStreams, 
            AudioStreamReader[] audioStreams)
        {
            FullName = fullName;
            VideoInfos = videoInfos;
            AudioInfos = audioInfos;
            VideoStreams = videoStreams;
            AudioStreams = audioStreams;
        }

        public string FullName { get; }
        public StreamInfo[] VideoInfos { get; }
        public StreamInfo[] AudioInfos { get; }
        public VideoStreamReader[] VideoStreams { get; set; }
        public AudioStreamReader[] AudioStreams { get; set; } 
    }
}