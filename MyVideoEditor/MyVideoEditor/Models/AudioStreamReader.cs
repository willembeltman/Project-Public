namespace MyVideoEditor.Models
{
    public class AudioStreamReader
    {
        public AudioStreamReader(FfmpegExecuteblesPaths ffmpegExecuteblesPaths, MediaContainer containerReader, StreamInfo streamInfo)
        {
            StreamInfo = streamInfo;
        }

        public StreamInfo StreamInfo { get; }
    }
}