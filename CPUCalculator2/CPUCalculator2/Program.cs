using CPUCalculator2.Data;
using CPUCalculator2.Services;

namespace CPUCalculator2;

public class Program
{
    public static void Main(string[] args)
    {
        var cpuService = new CpuService();
        using (var stream = File.OpenWrite("output.csv")) 
        using (var writer = new StreamWriter(stream)) 
        {
            writer.WriteLine(Cpu.CreateCsvHeader());
            foreach (var cpu in cpuService.CompileNewList())
            {
                writer.WriteLine(cpu.CreateCsvRow());
                Console.WriteLine(cpu);
            }
        }
    }
}