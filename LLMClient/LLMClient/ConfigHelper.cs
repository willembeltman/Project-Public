using Newtonsoft.Json;

namespace LLMClient
{
    public static class ConfigHelper
    {
        private const string ConfigFilename = "config.json";

        public static void Save(this Config config)
        {
            try
            {
                var configJson = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(ConfigFilename, configJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kon config niet opslaan: " + ex.Message);
            }
        }

        public static Config LoadConfig()
        {
            var config = new Config();
            if (File.Exists(ConfigFilename))
            {
                try
                {
                    var configJson = File.ReadAllText(ConfigFilename);
                    config = JsonConvert.DeserializeObject<Config>(configJson) ?? new Config();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kon config niet lezen: " + ex.Message);
                    config = new Config();
                }
            }
            return config;
        }
    }
}

