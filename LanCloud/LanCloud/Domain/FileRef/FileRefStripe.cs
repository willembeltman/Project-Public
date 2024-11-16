namespace LanCloud.Models
{
    public class FileRefStripe
    {
        public FileRefStripe(byte[] indexes)
        {
            Indexes = indexes;
        }

        public byte[] Indexes { get; }
    }
}
