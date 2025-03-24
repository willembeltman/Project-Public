using CPUCalculator2.Data;
using CPUCalculator2.Enums;
using CPUCalculator2.Helpers;
using CPUCalculator2.Models;
using System.Xml.Linq;

namespace CPUCalculator2.Services;

public class PassmarkDownloader
{
    public IEnumerable<PassmarkCpu> GetAllCpus()
    {
        var cpus =
            GetSingleScores().Concat(
                GetHighMultiScores().Concat(
                    GetOverclockedScores())
                )
            .GroupBy(a => a.Link)
            .ToArray();

        foreach (var cpu in cpus)
        {
            if (cpu.Key == null) continue;
            
            var link = cpu.Key;
            var single = cpu.FirstOrDefault(a => a.ScoreType == ScoreTypeEnum.Single);
            var multi = cpu.FirstOrDefault(a => a.ScoreType == ScoreTypeEnum.Multi);
            var overclocked = cpu.FirstOrDefault(a => a.ScoreType == ScoreTypeEnum.Overclocked);

            if (single == null) continue;
            if (multi == null) continue;
            if (single.Name == null) continue;

            var name = single.Name.Split("@").First();

            yield return new PassmarkCpu(
                name,
                single.Name,
                link,
                single.Score,
                multi.Score,
                overclocked?.Score
            );
        }
    }
    public IEnumerable<PassmarkScore> GetAllGpus()
    {
        var gpus =
            GetHighEndGpus();
        return gpus;
    }

    public IEnumerable<PassmarkScore> GetHighMultiScores()
        => Get("https://www.cpubenchmark.net/high_end_cpus.html", ScoreTypeEnum.Multi);
    public IEnumerable<PassmarkScore> GetOverclockedScores()
        => Get("https://www.cpubenchmark.net/overclocked_cpus.html", ScoreTypeEnum.Overclocked);
    public IEnumerable<PassmarkScore> GetSingleScores()
        => Get("https://www.cpubenchmark.net/singlethread.html", ScoreTypeEnum.Single);
    public IEnumerable<PassmarkScore> GetHighEndGpus()
        => Get("https://www.videocardbenchmark.net/high_end_gpus.html", ScoreTypeEnum.Gpu);

    private static IEnumerable<PassmarkScore> Get(string url, ScoreTypeEnum scoreType)
    {
        Console.WriteLine("Getting page: " + url);

        var pagehtml = HttpClientHelper.GetWebpage(url);
        var startline = "<ul class=\"chartlist\">";
        var endline = "</ul>";
        var currentpos = 0;

        while (true)
        {
            var start = pagehtml.IndexOf(startline, currentpos);
            if (start == -1) break;
            var end = pagehtml.IndexOf(endline, start) + endline.Length;
            if (end == -1) break;
            var html = pagehtml
                .Substring(start, end - start)
                .Replace("<br>", "")
                .Replace("<br/>", "")
                .Replace("<br />", "");
            var doc = XDocument.Parse(html);

            foreach (var li in doc.Descendants("li"))
            {
                var list = li.Descendants("a").First().Descendants("span");
                var hrefnode = li.Descendants("a").First().Attributes().First(b => b.Name == "href");

                var name = list.First().Value;

                var hrefstr = hrefnode.Value;
                var href = "https://www.cpubenchmark.net/" + hrefstr;

                var scorestr = list.Skip(2).First().Value;
                if (double.TryParse(StringHelper.NumberFormat(scorestr), out var score))
                {
                    var item = new PassmarkScore()
                    {
                        Name = name,
                        Link = href,
                        Score = score,
                        ScoreType = scoreType
                    };
                    yield return item;
                }
            }

            currentpos = end;
        }
    }
}
