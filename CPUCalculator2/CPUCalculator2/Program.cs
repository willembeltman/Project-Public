using CPUCalculator2.Data;
using CPUCalculator2.Services;

namespace CPUCalculator2;

public class Program
{
    public static void Main(string[] args)
    {
        //var passmarkDownloader = new PassmarkDownloader();
        //var cpuService = new CpuService();
        //using (var db = new Database())
        //using (var stream = File.OpenWrite("output.csv"))
        //using (var writer = new StreamWriter(stream))
        //{
        //    writer.WriteLine(Cpu.CreateCsvHeader());
        //    var passmarkcpus = passmarkDownloader.GetPassmarkCpus().ToArray();
        //    var cpus = cpuService.CompileNewList(passmarkcpus);
        //    foreach (var cpu in cpus)
        //    {
        //        db.AddCpu(cpu);
        //        writer.WriteLine(cpu.CreateCsvRow());
        //        Console.WriteLine(cpu);
        //    }
        //}


        using var db = new Database();

        var cpus = db.Cpus
            .Where(a => a.SingleScore > 4000 && a.TweakersPrice < 200)
            .OrderByDescending(a => a.Overclocked_SingleScoreBedrag)
            .ToArray();

    }
}