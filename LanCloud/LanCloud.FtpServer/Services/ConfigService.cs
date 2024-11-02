using LanCloud.Configs;
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

        public Config Load()
        {
            if (!File.Exists(Fullname))
            {
                Logger.Info("No config found, creating dummy config");
                var config = new Config()
                {
                    StartPort = 8080,
                    Servers = new RemoteApplicationConfig[]
                    {
                        new RemoteApplicationConfig()
                        {
                            Hostname = "WJPC2",
                            Port = 8080,
                            IsThisComputer = true
                        }
                    },
                    Shares = new LocalShareConfig[]
                    {
                        new LocalShareConfig()
                        {
                            FullName = "D:\\Test",
                            MaxSize = (new DriveInfo("D")).TotalFreeSpace,
                            Parts = new LocalSharePartConfig[]
                            {
                                new LocalSharePartConfig(1)
                            }
                        },
                        new LocalShareConfig()
                        {
                            FullName = "E:\\Test",
                            MaxSize = (new DriveInfo("E")).TotalFreeSpace,
                            Parts = new LocalSharePartConfig[]
                            {
                                new LocalSharePartConfig(2)
                            }
                        },
                        new LocalShareConfig()
                        {
                            FullName = "F:\\Test",
                            MaxSize = (new DriveInfo("F")).TotalFreeSpace,
                            Parts = new LocalSharePartConfig[]
                            {
                                new LocalSharePartConfig(new [] { 1, 2 })
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
                var config = JsonConvert.DeserializeObject<Config>(json);

                if ((config.Servers == null || config.Servers.Length == 0) &&
                    (config.Shares == null || config.Shares.Length == 0))
                    throw new System.Exception("Nothing is configured, please setup LanCloud.config file.");

                return config;
            }
        }

        public void Save(Config config)
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
