using LanCloud.Configs;
using LanCloud.Shared.Log;

namespace LanCloud.Domain.Share
{
    public class LocalShareStorage
    {
        public LocalShareStorage(LocalShareConfig shareConfig, ILogger logger)
        {
            ShareConfig = shareConfig;
            Logger = logger;
        }
        private LocalShareConfig ShareConfig { get; }
        private ILogger Logger { get; }

    }
}