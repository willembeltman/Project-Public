using CPUCalculator2.Data;
using CPUCalculator2.Services;
using System.ComponentModel.DataAnnotations;

namespace CPUCalculator2;

public class Program
{
    public static void Main(string[] args)
    {
        using var db = new Database();

        //var list = 

        //SyncService syncService = new SyncService(db);
        //syncService.SyncGpus();

        //var cpus = db.Cpus
        //    .Where(a => a.SingleScore > 4000 && a.TweakersPrice < 200)
        //    .OrderByDescending(a => a.Overclocked_SingleScoreBedrag)
        //    .ToArray();

        //var passmarkDownloader = new PassmarkDownloader();
        //var cpuService = new CpuService();
        //using var stream = File.OpenWrite("Cpus.csv");
        //using var writer = new StreamWriter(stream);
        //{
        //    writer.WriteLine(Cpu.CreateCsvHeader());
        //    foreach (var cpu in db.Cpus)
        //    {
        //        writer.WriteLine(cpu.CreateCsvRow());
        //        Console.WriteLine(cpu);
        //    }
        //}
    }
}