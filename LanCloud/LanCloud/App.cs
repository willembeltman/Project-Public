using System;
using System.Net.Sockets;
using System.Threading;

namespace LanCloud
{
    public class App 
    {
        public App()
        {
            Settings = new Settings(this);
            Server = new Server(this);
            FileLoader = new FileLoader();
            Form = new MainForm(this);
        }

        public Settings Settings { get; }
        public Server Server { get; }
        public FileLoader FileLoader { get; }
        public MainForm Form { get; }

        public bool KillSwitch { get; set; }

        Thread ServerThread { get; set; }
        Thread FileLoaderThread { get; set; }

        public void Start()
        {
            // Start de server
            ServerThread = new Thread(new ThreadStart(Server.Start));
            ServerThread.Start();

            // Start de file loader
            FileLoaderThread = new Thread(new ThreadStart(FileLoader.Start));
            FileLoaderThread.Start();

            // Gebruik main thread om de applicatie in te laden
            Form.ShowDialog();

            // Als het form wordt afgesloten, komt hij uit de showdialog dus kunnen we de rest ook afsluiten.
            KillSwitch = true;
            ServerThread.Join();
            FileLoaderThread.Join();
        }

        public void SettingsChanged()
        {
            Form.SettingsChanged();
            Server.SettingsChanged();
        }

        public void WriteLine(string msg)
        {
            Console.WriteLine($"{DateTime.Now} {msg}");
        }
    }
}
