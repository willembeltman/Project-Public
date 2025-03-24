using CPUCalculator2.Data;
using CPUCalculator2.Models;

namespace CPUCalculator2.Services;

public class GpuService
{
    TweakersDownloader TweakersDownloader = new TweakersDownloader();

    public IEnumerable<Gpu> CompileNewList(IEnumerable<PassmarkScore> gpus)
    {
        foreach (var gpu in gpus)
        {
            var product = TweakersDownloader.GetTweakersProduct(gpu.Name);
            if (product == null) continue;

            yield return new Gpu(gpu, product);
        }
    }
}
