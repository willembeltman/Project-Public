using LanCloud.Models.Configs;
using System.IO;

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
        public DirectoryInfo Root => Share.FileBits.Root;
        public int[] Indexes => Part.Indexes;
    }
}