using CPUCalculator2.Data;
using CPUCalculator2.Helpers;
using Newtonsoft.Json;
using System.Web;

namespace CPUCalculator2.Services;

public class TweakersDownloader
{
    public TweakersProduct? GetTweakersProduct(PassmarkCpu passmarkCpu)
    {
        var words = passmarkCpu.Name.ToLower().Split(' ');
        var prices = GetFromTweakers(passmarkCpu.Name)
            .Where(item =>
            {
                if (item.Name == null) return false;
                if (item.Type != "product") return false;
                if (item.Price <= 0) return false;

                var itemwords = item.Name.ToLower().Split(" ");
                var check = words.All(word => itemwords.Any(itemword => word == itemword));
                if (!check) return false;

                return true;
            })
            .ToArray();

        var product = prices.FirstOrDefault();
        return product;
    }

    public IEnumerable<TweakersProduct> GetFromTweakers(string searchstring)
    {
        var str = "vanaf \\u20ac";
        var str2 = "vanaf €";
        var oldurl = "https://tweakers.net/xmlhttp/xmlHttp.php?application=sitewidesearch&type=search&action=pricewatch&keyword=" + HttpUtility.UrlEncode(searchstring) + "&output=json";
        var url = $"https://tweakers.net/ajax/zoeken/pricewatch/?keyword={HttpUtility.UrlEncode(searchstring)}&output=json&country=NL";
        var html = HttpClientHelper.GetWebpage(url);
        var start2 = html.IndexOf(str);
        if (start2 > 0)
        {
            dynamic res = JsonConvert.DeserializeObject(html);

            foreach (var ent in res.entities)
            {
                string prijsstr = ent.minPrice;
                if (prijsstr != null)
                {
                    var start = prijsstr.IndexOf(str2) + str2.Length;
                    var end = prijsstr.IndexOf("</a>");
                    var prijsstrtrim = prijsstr.Substring(start, end - start).Trim().Replace(".", "").Replace(",-", "");
                    var price = Convert.ToDouble(prijsstrtrim);

                    var product = new TweakersProduct()
                    {
                        Id = ent.id,
                        Name = ent.name,
                        Type = ent.type,
                        Price = price,
                        Link = ent.link,
                        Thumbnail = ent.thumbnail,
                    };
                    yield return product;
                }
            }
        }
    }

}
