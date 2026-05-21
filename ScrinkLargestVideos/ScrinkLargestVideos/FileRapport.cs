using System.IO;

namespace ScrinkLargestVideos
{
    public class FileRapport
    {
        public FileRapport()
        {

        }
        public FileRapport(FileInfo file)
        {
            FullName = file.FullName;
            Length = file.Length;
            Name = file.Name;
            Extention = file.Extension;
            Rapport = FFProbe.GetRapport(FullName);
        }

        public string FullName { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        public FFProbeRapport Rapport { get; set; }
    }
}