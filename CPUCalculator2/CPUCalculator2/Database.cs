using CPUCalculator2.Data;
using Newtonsoft.Json;

namespace CPUCalculator2
{
    public class Database : IDisposable
    {
        public Database()
        {
            Cpus = [];
            DownloadedCpus = [];
            ReadDatabase();
        }

        public Cpu[] Cpus { get; private set; }
        public DownloadedCpu[] DownloadedCpus { get; private set; }

        public void AddCpu(Cpu cpu)
        {
            lock (Cpus)
            {
                var newArray = new Cpu[Cpus.Length + 1];
                Cpus.CopyTo(newArray, 0);
                newArray[Cpus.Length] = cpu;
                Cpus = newArray;
            }
        }
        public void AddDownloadedCpu(DownloadedCpu cpu)
        {
            lock (DownloadedCpus)
            {
                var newArray = new DownloadedCpu[DownloadedCpus.Length + 1];
                DownloadedCpus.CopyTo(newArray, 0);
                newArray[DownloadedCpus.Length] = cpu;
                DownloadedCpus = newArray;
            }
        }

        private void ReadDatabase()
        {
            ReadCpus();
            ReadDownloadedCpus();
        }
        private void ReadCpus()
        {
            lock (Cpus)
            {
                if (!File.Exists("Cpus.json"))
                {
                    Cpus = [];
                    return;
                }
                var cpusjson = File.ReadAllText("Cpus.json");
                var cpus = JsonConvert.DeserializeObject<Cpu[]>(cpusjson);
                if (cpus == null)
                    Cpus = [];
                else
                    Cpus = cpus!;
            }
        }
        private void ReadDownloadedCpus()
        {
            lock (DownloadedCpus)
            {
                if (!File.Exists("DownloadedCpus.json"))
                {
                    DownloadedCpus = [];
                    return;
                }
                var cpusjson = File.ReadAllText("DownloadedCpus.json");
                var cpus = JsonConvert.DeserializeObject<DownloadedCpu[]>(cpusjson);
                if (cpus == null)
                    DownloadedCpus = [];
                else
                    DownloadedCpus = cpus!;
            }
        }

        public void SaveChanges()
        {
            SaveCpus();
            SaveDownloadedCpus();
        }

        private void SaveCpus()
        {
            lock (Cpus)
            {
                var json = JsonConvert.SerializeObject(Cpus);
                File.WriteAllText("Cpus.json", json);
            }
        }
        private void SaveDownloadedCpus()
        {
            lock (DownloadedCpus)
            {
                var json = JsonConvert.SerializeObject(DownloadedCpus);
                File.WriteAllText("DownloadedCpus.json", json);
            }
        }

        public void Dispose()
        {
            SaveChanges();
        }
    }
}
