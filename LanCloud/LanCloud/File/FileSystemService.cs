using LanCloud.Interfaces;
using System.Threading.Tasks;

namespace LanCloud.File
{
    public class FileSystemService : IInitializeble
    {
        public FileSystemService(App app)
        {
            App = app;
            DokanOperations = new DokanOperations(this);
        }

        public App App { get; }
        public DokanOperations DokanOperations { get; }

        public async Task InitializeAsync()
        {
        }
    }
}
