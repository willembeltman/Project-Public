namespace LanCloud.Models.Dtos
{
    public class FileStripeDto
    {
        public FileStripeDto() { }
        public FileStripeDto(FileStripe fileStripe)
        {
            IsTemp = fileStripe.IsTemp;
            Length = fileStripe.Length;
            Hash = fileStripe.Hash;
            Extention = fileStripe.Extention;
            Indexes = fileStripe.Indexes;
        }

        public bool IsTemp { get; set; }
        public long Length { get; set; }
        public string Hash { get; set; }
        public string Extention { get; set; }
        public byte[] Indexes { get; set; }
    }
}