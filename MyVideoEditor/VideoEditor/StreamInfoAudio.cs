namespace VideoEditor
{
    public class StreamInfoAudio
    {
        public StreamInfoAudio(File file, StreamInfo info)
        {
            File = file;
            Index = info.Index;
        }

        public File File { get; }
        public int Index { get; }
    }
}