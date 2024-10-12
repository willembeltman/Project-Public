using LanCloud.Models;
using Newtonsoft.Json;
using System.IO;

namespace LanCloud.Services
{
    public class ApplicationConfigService
    {
        public ApplicationConfigService(string currentDirectory)
        {
            CurrentDirectory = currentDirectory;
        }

        public string CurrentDirectory { get; }

        public ApplicationConfig Load()
        {
            var fullname = Path.Combine(CurrentDirectory, "LanCloud.config");
            if (!File.Exists(fullname))
            {
                var config = new ApplicationConfig();
                var json = JsonConvert.SerializeObject(config);
                using (var writer = new StreamWriter(fullname))
                {
                    writer.Write(json);
                }
            }

            using (var reader = new StreamReader(fullname))
            {
                var json = reader.ReadToEnd();
                var config = JsonConvert.DeserializeObject<ApplicationConfig>(json);

                if ((config.Servers == null || config.Servers.Length == 0) && 
                    (config.Shares == null || config.Shares.Length == 0))
                    throw new System.Exception("Nothing is configured, please setup LanCloud.config file.");

                return config;
            }

        }

        public void Load(ApplicationConfig config)
        {
            var fullname = Path.Combine(CurrentDirectory, "LanCloud.config");
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
