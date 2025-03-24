namespace CPUCalculator2.Data;

public class Database : IDisposable
{
    public Table<Cpu> Cpus { get; set; } = new Table<Cpu>();
    public Table<Gpu> Gpus { get; set; } = new Table<Gpu>();
    public Table<DownloadedCpu> DownloadedCpus { get; set; } = new Table<DownloadedCpu>();

    public void SaveChanges()
    {
        Cpus.SaveChanges();
        Gpus.SaveChanges();
        DownloadedCpus.SaveChanges();
    }
    public void Reload()
    {
        Cpus.Reload();
        Gpus.SaveChanges();
        DownloadedCpus.Reload();
    }
    public void Dispose()
    {
        SaveChanges();
    }
}
