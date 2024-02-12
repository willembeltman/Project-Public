namespace BeltmanSoftwareDesign.Shared.Jsons
{
    public class WorkorderAttachment
    {
        public long id { get; set; }

        public string FileMimeType { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileMD5 { get; set; }

        public string FileUrl { get; set; }
    }
}