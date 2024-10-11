//using LanCloud.Interfaces;
//using System.Net;
//using System.Threading;
//using System.Threading.Tasks;

//namespace LanCloud.Settings
//{
//    public class SettingsService : IStarteble, IInitializeble
//    {
//        public event SettingsChangedEventHandler SettingsChanged;
//        AutoResetEvent SettingsChangedAutoReset = new AutoResetEvent(false);

//        public SettingsService(App app)
//        {
//            App = app;
//        }

//        App App { get; }
//        SettingsEntity SettingsEntity { get; set; }

//        public int Port
//        {
//            get => SettingsEntity.Port;
//            set
//            {
//                SettingsEntity.Port = value;
//                SettingsChangedAutoReset.Set();
//                SettingsChanged?.Invoke(SettingsEntity);
//            }
//        }
//        public IPAddress IPAddress
//        {
//            get => SettingsEntity.IPAddress;
//            set
//            {
//                SettingsEntity.IPAddress = value;
//                SettingsChangedAutoReset.Set();
//                SettingsChanged?.Invoke(SettingsEntity);
//            }
//        }

//        public async Task InitializeAsync()
//        {
//            await Task.Run(() =>
//            {
//                SettingsEntity = new SettingsEntity();
//            });
//        }

//        public async Task StartAsync()
//        {
//            while (!App.KillSwitch)
//            {
//                if (SettingsChangedAutoReset.WaitOne(1000))
//                {
//                    await SaveSettings();
//                }
//            }
//        }

//        private async Task SaveSettings()
//        {
//            // TODO
//        }
//    }
//}
