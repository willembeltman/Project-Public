namespace MyVideoEditor.Models
{
    public class AudioStreamReader
    {
        public AudioStreamReader(DirectoryInfo ffmpegDirectory, MediaContainer containerReader, StreamInfo streamInfo)
        {
            StreamInfo = streamInfo;
        }

        public StreamInfo StreamInfo { get; }
    }
}