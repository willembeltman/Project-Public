namespace LanCloud.Servers.Wjp
{
    public interface IWjpApplication
    {
        void StatusChanged();
        int FileBitBufferSize { get; }
        int WjpBufferSize { get; }
    }
}