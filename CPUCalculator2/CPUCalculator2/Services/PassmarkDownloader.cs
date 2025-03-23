using CPUCalculator2.Data;
using CPUCalculator2.Helpers;
using System.Xml.Linq;

namespace CPUCalculator2.Services;

public class PassmarkDownloader
{
    public IEnumerable<PassmarkCpu> GetCpus()
    {
        var passmarkDownloader = new PassmarkDownloader();
        var tweakersdownloader = new TweakersDownloader();
        var cpus =
            passmarkDownloader.GetSingleScores().Concat(
                passmarkDownloader.GetHighMultiScores().Concat(
                    passmarkDownloader.GetOverclockedScores())
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

    public IEnumerable<PassmarkSimpleCpu> GetHighMultiScores()
        => Get("https://www.cpubenchmark.net/high_end_cpus.html", ScoreTypeEnum.Multi);
    public IEnumerable<PassmarkSimpleCpu> GetOverclockedScores()
        => Get("https://www.cpubenchmark.net/overclocked_cpus.html", ScoreTypeEnum.Overclocked);
    public IEnumerable<PassmarkSimpleCpu> GetSingleScores()
        => Get("https://www.cpubenchmark.net/singlethread.html", ScoreTypeEnum.Single);
    private static IEnumerable<PassmarkSimpleCpu> Get(string url, ScoreTypeEnum scoreType)
    {
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
                    var item = new PassmarkSimpleCpu()
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
