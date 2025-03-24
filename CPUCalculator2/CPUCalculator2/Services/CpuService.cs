using CPUCalculator2.Data;

namespace CPUCalculator2.Services;

public class CpuService
{
    TweakersDownloader TweakersDownloader = new TweakersDownloader();

    public IEnumerable<Cpu> CompileNewList(IEnumerable<PassmarkCpu> cpus)
    {
        foreach (var cpu in cpus)
        {
            var product = TweakersDownloader.GetTweakersProduct(cpu.Name);
            if (product == null) continue;

            yield return new Cpu(cpu, product);
        }
    }
}
