namespace LanCloud.Domain.Share
{
    public interface IShareStripe
    {
        IShare Share { get; }
        byte[] Indexes { get; }
    }
}
