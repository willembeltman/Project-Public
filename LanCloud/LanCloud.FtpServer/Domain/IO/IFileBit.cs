namespace LanCloud.Domain.IO
{
    public interface IFileBit
    {
        string Extention { get; }
        int[] Indexes { get; }
        bool IsTemp { get; }
        string Hash { get; }
        long Length { get; }
    }
}