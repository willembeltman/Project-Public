namespace LanCloud.Domain.Share
{
    internal class FindFileStripesRequest
    {
        public FindFileStripesRequest() { } 
        public FindFileStripesRequest(string extention, string hash, long length, byte[] indexes)
        {
            Extention = extention;
            Hash = hash;
            Length = length;
            Indexes = indexes;
        }

        public string Extention { get; set; }
        public string Hash { get; set; }
        public long Length { get; set; }
        public byte[] Indexes { get; set; }
    }
}