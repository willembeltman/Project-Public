using CPUCalculator2.Data;

namespace CPUCalculator2.Services;

public class CpuService
{
    PassmarkDownloader PassmarkDownloader = new PassmarkDownloader();
    TweakersDownloader TweakersDownloader = new TweakersDownloader();

    public IEnumerable<Cpu> CompileNewList()
    {
        var cpus = PassmarkDownloader.GetCpus();
        foreach (var cpu in cpus)
        {
            var product = TweakersDownloader.GetTweakersProduct(cpu);
            if (product == null) continue;

            yield return new Cpu(cpu, product);
        }
    }
}
