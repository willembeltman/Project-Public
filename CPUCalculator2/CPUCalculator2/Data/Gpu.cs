namespace CPUCalculator2.Data;

public class Gpu
{
    public Gpu()
    {
        PassmarkScore = new PassmarkScore();
        TweakersProduct = new TweakersProduct();
    }
    public Gpu(PassmarkScore score, TweakersProduct product)
    {
        PassmarkScore = score;
        TweakersProduct = product;
    }

    private PassmarkScore PassmarkScore { get; }
    private TweakersProduct TweakersProduct { get; }

    public long TweakersId
    {
        get => TweakersProduct.Id;
        set => TweakersProduct.Id = value;
    }
    public string? TweakersName
    {
        get => TweakersProduct.Name;
        set => TweakersProduct.Name = value;
    }
    public string? TweakersLink
    {
        get => TweakersProduct.Link;
        set => TweakersProduct.Link = value;
    }
    public double TweakersPrice
    {
        get => TweakersProduct.Price;
        set => TweakersProduct.Price = value;
    }
    public string? TweakersThumbnail
    {
        get => TweakersProduct.Thumbnail;
        set => TweakersProduct.Name = value;
    }

    public string Name
    {
        get => PassmarkScore.Name ?? "";
        set => PassmarkScore.Name = value;
    }
    public double Score
    {
        get => PassmarkScore.Score;
        set => PassmarkScore.Score = value;
    }
    public string PassmarkLink
    {
        get => PassmarkScore.Link ?? "";
        set => PassmarkScore.Link = value;
    }

    public override string ToString()
    {
        return $"{Name} {TweakersPrice:F2} ({Score:F0})";
    }

}