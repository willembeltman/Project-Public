namespace LanCloud.Domain.IO
{
    public interface IFileBit
    {
        string Extention { get; }
        byte[] Indexes { get; }
        bool IsTemp { get; }
        string Hash { get; }
        long Length { get; }
    }
}