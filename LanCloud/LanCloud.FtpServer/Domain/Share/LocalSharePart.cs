using LanCloud.Domain.Collections;
using LanCloud.Models.Configs;

namespace LanCloud.Domain.Share
{
    public class LocalSharePart
    {
        public LocalSharePart(LocalSharePartCollection localSharePartCollection, LocalSharePartConfig part)
        {
            LocalSharePartCollection = localSharePartCollection;
            Part = part;
        }

        public LocalSharePartCollection LocalSharePartCollection { get; }
        public LocalSharePartConfig Part { get; }

        public LocalShare Share => LocalSharePartCollection.Share;
    }
}