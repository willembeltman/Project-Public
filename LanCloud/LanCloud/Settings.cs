using System.Net;

namespace LanCloud
{
    public class Settings
    {
        public Settings(App app)
        {
            App = app;
        }
        App App { get; }

        public int Port { get; private set; } = 9999;
        public IPAddress IPAddress { get; private set; } = IPAddress.Any;
    }
}
