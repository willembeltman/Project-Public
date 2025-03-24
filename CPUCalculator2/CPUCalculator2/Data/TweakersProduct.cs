
namespace CPUCalculator2.Data;

public class TweakersProduct
{
    public TweakersProduct()
    {
    }

    public TweakersProduct(
        long id,
        string? name,
        string? type,
        double price,
        string? link,
        string? thumbnail)
    {
        Id = id;
        Name = name;
        Type = type;
        Price = price;
        Link = link;
        Thumbnail = thumbnail;
    }

    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public double Price { get; set; }
    public string? Link { get; set; }
    public string? Thumbnail { get; set; }

    internal void OverwriteWith(TweakersProduct tweakersProduct)
    {
        Id = tweakersProduct.Id;
        Name = tweakersProduct.Name;
        Type = tweakersProduct.Type;
        Price = tweakersProduct.Price;
        Link = tweakersProduct.Link;
        Thumbnail = tweakersProduct.Thumbnail;
    }
}
