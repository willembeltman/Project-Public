using LanCloud.Models;
using LanCloud.Shared.Models;
using Newtonsoft.Json;
using System.IO;

namespace LanCloud.Services
{
    public class ConfigService
    {
        public ConfigService(string currentDirectory)
        {
            CurrentDirectory = currentDirectory;
        }

        public string CurrentDirectory { get; }

        public Config Load()
        {
            var fullname = Path.Combine(CurrentDirectory, "LanCloud.json");
            if (!File.Exists(fullname))
            {
                var config = new Config()
                {
                    Port = 8080,
                    Servers = new RemoteApplicationConfig[]
                    {
                        new RemoteApplicationConfig()
                        {
                            Hostname = "WJLAPTOP",
                            Port = 8080,
                            IsThisComputer = true
                        }
                    },
                    Shares = new LocalShareConfig[]
                    {
                        new LocalShareConfig()
                        {
                            FullName="D:\\Test"
                        }
                    }
                };
                var json = JsonConvert.SerializeObject(config);
                using (var writer = new StreamWriter(fullname))
                {
                    writer.Write(json);
                }
            }

            using (var reader = new StreamReader(fullname))
            {
                var json = reader.ReadToEnd();
                var config = JsonConvert.DeserializeObject<Config>(json);

                if ((config.Servers == null || config.Servers.Length == 0) &&
                    (config.Shares == null || config.Shares.Length == 0))
                    throw new System.Exception("Nothing is configured, please setup LanCloud.config file.");

                return config;
            }

        }

        public void Load(Config config)
        {
            var fullname = Path.Combine(CurrentDirectory, "LanCloud.json");
            if (File.Exists(fullname))
            {
                File.Delete(fullname);
            }

            var json = JsonConvert.SerializeObject(config);
            using (var writer = new StreamWriter(fullname))
            {
                writer.Write(json);
            }
        }
    }
}
