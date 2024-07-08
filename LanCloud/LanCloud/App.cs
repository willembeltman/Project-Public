using DokanNet;
using LanCloud.Client;
using LanCloud.File;
using LanCloud.Forms;
using LanCloud.Logger;
using LanCloud.Server;
using LanCloud.Settings;
using System.Threading.Tasks;

namespace LanCloud
{
    public class App
    {
        public App()
        {
            Settings = new SettingsService(this);
            Servers = new ServerService(this);
            Clients = new ClientService(this);
            Logger = new LoggerService(this);
            FileSystem = new FileSystemService(this);
            Forms = new FormsService(this);
        }

        public SettingsService Settings { get; }
        public ServerService Servers { get; }
        public ClientService Clients { get; }
        public LoggerService Logger { get; }
        public FileSystemService FileSystem { get; }
        public FormsService Forms { get; }

        public bool KillSwitch { get; set; }

        public void Start()
        {
            Task.WaitAll(
                Settings.InitializeAsync(),
                FileSystem.InitializeAsync());

            using (var dokan = new Dokan(Logger))
            {
                var dokanInstanceBuilder = new DokanInstanceBuilder(dokan);
                using (var dokanInstance = dokanInstanceBuilder.Build(FileSystem.DokanOperations))
                {
                    Task.WaitAll(
                        Settings.StartAsync(),
                        Servers.StartAsync(),
                        Clients.StartAsync(),
                        Logger.StartAsync());
                }
            }
        }
    }
}
