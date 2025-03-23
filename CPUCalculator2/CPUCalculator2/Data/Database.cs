namespace CPUCalculator2.Data;

public class Database : IDisposable
{
    public Table<Cpu> Cpus { get; set; } = new Table<Cpu>();
    public Table<DownloadedCpu> DownloadedCpus { get; set; } = new Table<DownloadedCpu>();

    public void SaveChanges()
    {
        Cpus.SaveChanges();
        DownloadedCpus.SaveChanges();
    }
    public void Reload()
    {
        Cpus.Reload();
        DownloadedCpus.Reload();
    }
    public void Dispose()
    {
        SaveChanges();
    }
}
