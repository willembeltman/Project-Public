using System.Net;

namespace LanCloud.Settings
{
    public class SettingsEntity
    {
        public int Port { get; set; } = 9999;
        public IPAddress IPAddress { get; set; } = IPAddress.Any;
    }
}
