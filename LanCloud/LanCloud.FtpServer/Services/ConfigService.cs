using LanCloud.Models.Configs;
using LanCloud.Shared.Log;
using Newtonsoft.Json;
using System.IO;

namespace LanCloud.Services
{
    public class ConfigService
    {
        public ConfigService(string currentDirectory, ILogger logger)
        {
            Logger = logger;
            Fullname = Path.Combine(currentDirectory, "LanCloud.json");
        }

        public string Fullname { get; }
        public ILogger Logger { get; }

        public ApplicationConfig Load()
        {
            if (!File.Exists(Fullname))
            {
                Logger.Info("No config found, creating dummy config");
                var config = new ApplicationConfig()
                {
                    StartPort = 8080,
                    FileDatabaseDirectoryName = "E:\\Test\\Ref",
                    Servers = new RemoteApplicationConfig[]
                    {
                        new RemoteApplicationConfig()
                        {
                            HostName = "WJPC2",
                            Port = 8080,
                            IsThisComputer = true
                        }
                    },
                    Shares = new LocalShareConfig[]
                    {
                        new LocalShareConfig()
                        {
                            DirectoryName = "E:\\Test\\0",
                            Parts = new LocalSharePartConfig[]
                            {
                                new LocalSharePartConfig(0)
                            }
                        },
                        new LocalShareConfig()
                        {
                            DirectoryName = "E:\\Test\\1",
                            Parts = new LocalSharePartConfig[]
                            {
                                new LocalSharePartConfig(1)
                            }
                        },
                        new LocalShareConfig()
                        {
                            DirectoryName = "E:\\Test\\P",
                            Parts = new LocalSharePartConfig[]
                            {
                                new LocalSharePartConfig(new [] { 0, 1 })
                            }
                        }
                    },

                };
                Save(config);
                return config;
            }

            Logger.Info("Config found, reading config settings");
            using (var reader = new StreamReader(Fullname))
            {
                var json = reader.ReadToEnd();
                var config = JsonConvert.DeserializeObject<ApplicationConfig>(json);

                if ((config.Servers == null || config.Servers.Length == 0) &&
                    (config.Shares == null || config.Shares.Length == 0))
                    throw new System.Exception("Nothing is configured, please setup LanCloud.config file.");

                return config;
            }
        }

        public void Save(ApplicationConfig config)
        {
            Logger.Info("Saving the config");

            if (File.Exists(Fullname))
            {
                File.Delete(Fullname);
            }

            var json = JsonConvert.SerializeObject(config);
            using (var writer = new StreamWriter(Fullname))
            {
                writer.Write(json);
            }
        }
    }
}
