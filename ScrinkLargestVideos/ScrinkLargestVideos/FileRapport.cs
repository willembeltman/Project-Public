using System.ComponentModel.DataAnnotations;

namespace ScrinkLargestVideos;

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
        RapportJson = FFProbe.GetRapportJson(file.FullName);
    }

    [Key]
    public string FullName { get; set; } = string.Empty;
    public long Length { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Extention { get; set; } = string.Empty;
    public string RapportJson { get; set; } = string.Empty;

    public FFProbeRapport GetRapport() => FFProbe.Deserialize(RapportJson);
}