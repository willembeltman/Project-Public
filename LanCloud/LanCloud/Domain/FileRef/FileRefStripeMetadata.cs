using System.Linq;

namespace LanCloud.Domain.FileRef
{
    public class FileRefStripeMetadata
    {
        public FileRefStripeMetadata(byte[] indexes)
        {
            Indexes = indexes;
        }

        public byte[] Indexes { get; }

        public string GetUniqueIdentifier()
        {
            return string.Join("_", Indexes.OrderBy(a => a));
        }
    }
}
