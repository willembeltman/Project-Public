using LanCloud.Models.Configs;

namespace LanCloud.Domain.Share
{
    public class LocalSharePart
    {
        public LocalSharePart(LocalShare localShare, LocalSharePartConfig part)
        {
            Share = localShare;
            Part = part;
        }
        public LocalShare Share { get; }
        public LocalSharePartConfig Part { get; }
    }
}