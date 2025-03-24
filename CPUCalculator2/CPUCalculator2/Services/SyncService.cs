using CPUCalculator2.Data;

namespace CPUCalculator2.Services;

public class SyncService
{
    PassmarkDownloader PassmarkDownloader = new PassmarkDownloader();
    GpuService GpuService = new GpuService();
    CpuService CpuService = new CpuService();

    public SyncService(Database db)
    {
        this.db = db;
    }

    Database db { get; }

    public void Sync()
    {
        SyncGpus();
        SyncCpus();
    }

    public void SyncCpus()
    {
        var passmarkcpus = PassmarkDownloader.GetAllCpus().ToArray();
        var cpus = CpuService.CompileNewList(passmarkcpus);

        foreach (var cpu in cpus)
        {
            var search = db.Cpus.FirstOrDefault(a => a.Name == cpu.Name);
            if (search == null)
            {
                Console.WriteLine("Added cpu: " + cpu);
                db.Cpus.Add(cpu);
            }
            else
            {
                search.OverwriteWith(cpu);
            }
        }

        db.Cpus.SaveChanges();
    }

    public void SyncGpus()
    {
        var passmarkgpus = PassmarkDownloader.GetAllGpus().ToArray();
        var gpus = GpuService.CompileNewList(passmarkgpus);

        foreach (var gpu in gpus)
        {
            var search = db.Gpus.FirstOrDefault(a => a.Name == gpu.Name);
            if (search == null)
            {
                Console.WriteLine("Added gpu: " + gpu);
                db.Gpus.Add(gpu);
            }
        }

        db.Gpus.SaveChanges();
    }
}
