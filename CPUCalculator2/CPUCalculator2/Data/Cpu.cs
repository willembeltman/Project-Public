namespace CPUCalculator2.Data;

public class Cpu
{
    public Cpu()
    {
        PassmarkCpu = new PassmarkCpu();
        TweakersProduct = new TweakersProduct();
    }
    public Cpu(PassmarkCpu cpu, TweakersProduct product)
    {
        PassmarkCpu = cpu;
        TweakersProduct = product;
    }

    private PassmarkCpu PassmarkCpu { get; }
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
        get => PassmarkCpu.Name;
        set => PassmarkCpu.Name = value;
    }
    public double SingleScore
    {
        get => PassmarkCpu.SingleScore;
        set => PassmarkCpu.SingleScore = value;
    }
    public double MultiScore
    {
        get => PassmarkCpu.MultiScore;
        set => PassmarkCpu.MultiScore = value;
    }
    public double? MultiScoreOC
    {
        get => PassmarkCpu.OcScore;
        set => PassmarkCpu.OcScore = value;
    }
    public string PassmarkLink
    {
        get => PassmarkCpu.Link;
        set => PassmarkCpu.Link = value;
    }

    public double Overclocked_MultiScore => MultiScoreOC ?? MultiScore;
    public double Overclocked_SingleScore
    {
        get
        {
            if (MultiScore == 0) return -1;
            return SingleScore * Overclocked_MultiScore / MultiScore;
        }
    }

    public double SingleScoreBedrag => Convert.ToDouble(SingleScore) / TweakersPrice * 100;
    public double MultiScoreBedrag => Convert.ToDouble(MultiScore) / TweakersPrice * 100;
    public double Overclocked_MultiScoreBedrag => Convert.ToDouble(Overclocked_MultiScore) / TweakersPrice * 100;
    public double Overclocked_SingleScoreBedrag => Convert.ToDouble(Overclocked_SingleScore) / TweakersPrice * 100;

    public override string ToString()
    {
        return $"{Name} {TweakersPrice:F2} ({SingleScore:F0} / {MultiScore:F0} OC {MultiScoreOC:F0})";
    }

    public static string CreateCsvHeader()
    {
        return
            $"\"TweakersId\";\"TweakersName\";\"TweakersLink\";\"TweakersPrice\";\"TweakersThumbnail\";" +
            $"\"Name\";\"SingleScore\";\"MultiScore\";\"MultiScoreOC\";\"PassmarkLink\";" +
            $"\"Overclocked_MultiScore\";\"Overclocked_SingleScore\";" +
            $"\"SingleScoreBedrag\";\"MultiScoreBedrag\";\"Overclocked_MultiScoreBedrag\";\"Overclocked_SingleScoreBedrag\"";
    }
    public string CreateCsvRow()
    {
        return
            $"{TweakersId};\"{TweakersName}\";\"{TweakersLink}\";{TweakersPrice};\"{TweakersThumbnail};\"" +
            $"\"{Name}\";{SingleScore};{MultiScore};{MultiScoreOC};\"{PassmarkLink}\";" +
            $"{Overclocked_MultiScore};{Overclocked_SingleScore};" +
            $"{SingleScoreBedrag};{MultiScoreBedrag};{Overclocked_MultiScoreBedrag};{Overclocked_SingleScoreBedrag}";
    }
}