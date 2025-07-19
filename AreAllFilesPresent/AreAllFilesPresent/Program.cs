using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreAllFilesPresent
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir1 = new DirectoryInfo(@"\\wjserver\E\Footage\");
            var dir2 = new DirectoryInfo(@"e:\Videoedit\");

            List<CompareFile> clear_list = new List<CompareFile>();
            List<CompareFile> more_list = new List<CompareFile>();

            GetFiles(clear_list, dir1);
            GetFiles(more_list, dir2);

            foreach (var item in more_list)
            {
                var found = clear_list.FirstOrDefault(a => a.ToString() == item.ToString());
                if (found != null)
                {
                    clear_list.Remove(found);
                }
            }

            foreach (var item in clear_list)
            {
                var fullname = $"{dir2}FOUND\\{item.Info.FullName.Substring(dir1.FullName.Length)}";
                var info = new FileInfo(fullname);
                if (!info.Directory.Exists)
                    info.Directory.Create();
                
                item.Info.CopyTo(fullname);
            }
        }

        private static void GetFiles(List<CompareFile> list, DirectoryInfo dir)
        {
            list.AddRange(dir.GetFiles().Select(a => new CompareFile(a)));
            foreach (var subdir in dir.GetDirectories())
            {
                GetFiles(list, subdir);
            }
        }
    }

    public class CompareFile : IComparable
    {
        public CompareFile(FileInfo info)
        {
            Info = info;
        }

        public FileInfo Info { get; }

        public int CompareTo(object obj)
        {
            return obj.ToString().CompareTo(this.ToString());
        }

        public string Name { get; set; }
        public override string ToString()
        {
            if (Name == null)
            {
                Name = $"{Info.Name} {Info.Length}b";
            }
            return Name;
        }
    }
}
