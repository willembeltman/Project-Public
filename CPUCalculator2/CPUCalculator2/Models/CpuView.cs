using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CPUCalculator2.Models;

public class CpuView
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    public string PassmarkUrl { get; set; }
    public double SingleScore { get; set; }
    public double MultiScore { get; set; }
    public double? MultiScoreOC { get; set; }

    public long TweakersId { get; set; }
    public string? TweakersLink { get; set; }
    public string? TweakersThumbnail { get; set; }
    public double? TweakersPrijs { get; set; }

    public DateTime? TweakersPrijsDate { get; set; }
    public DateTime? PassmarkUpdateDate { get; set; }
    public DateTime? PassmarkInsertDate { get; set; }

    public bool Disabled { get; set; }

    public double GetX()
    {
        return Overclocked_SingleScore;
        //return Overclocked_MultiScore;
    }
    public double GetY()
    {
        return Overclocked_MultiScore;
        //return Overclocked_SingleScore;
    }
    public double GetSize()
    {
        return Overclocked_SingleScoreBedrag + Overclocked_MultiScoreBedrag;
        //return (Overclocked_SingleScoreBedrag + Overclocked_MultiScoreBedrag) / 2;
    }

    [NotMapped]
    public double Overclocked_MultiScore => MultiScoreOC ?? MultiScore;
    [NotMapped]
    public double Overclocked_SingleScore
    {
        get
        {
            if (MultiScore == 0) return -1;
            return SingleScore * Overclocked_MultiScore / MultiScore;
        }
    }

    [NotMapped]
    public double SingleScoreBedrag { get { return Convert.ToDouble(SingleScore) / (TweakersPrijs ?? double.MaxValue) * 100; } }
    [NotMapped]
    public double MultiScoreBedrag { get { return Convert.ToDouble(MultiScore) / (TweakersPrijs ?? double.MaxValue) * 100; } }
    [NotMapped]
    public double Overclocked_MultiScoreBedrag { get { return Convert.ToDouble(Overclocked_MultiScore) / (TweakersPrijs ?? double.MaxValue) * 100; } }
    [NotMapped]
    public double Overclocked_SingleScoreBedrag { get { return Convert.ToDouble(Overclocked_SingleScore) / (TweakersPrijs ?? double.MaxValue) * 100; } }


    public byte R(double newindex)
    {
        double index = newindex * 5;
        if (index >= 0 && index <= 1)
        {
            return 255;
        }
        if (index > 1 && index <= 2)
        {
            return Convert.ToByte((1 - (index - 1)) * 255);
        }
        if (index > 2 && index <= 3)
        {
            return 0;
        }
        if (index > 3 && index <= 4)
        {
            return 0;
        }
        if (index > 4 && index <= 5)
        {
            return Convert.ToByte((index - 4) * 255);
        }
        return 0;
    }

    public byte G(double newindex)
    {
        double index = newindex * 5;
        if (index >= 0 && index <= 1)
        {
            return Convert.ToByte(index * 255);
        }
        if (index > 1 && index <= 2)
        {
            return 255;
        }
        if (index > 2 && index <= 3)
        {
            return 255;
        }
        if (index > 3 && index <= 4)
        {
            return Convert.ToByte((1 - (index - 3)) * 255);
        }
        if (index > 4 && index <= 5)
        {
            return 0;
        }
        return 0;

    }

    public byte B(double newindex)
    {
        double index = newindex * 5;
        if (index >= 0 && index <= 1)
        {
            return 0;
        }
        if (index > 1 && index <= 2)
        {
            return 0;
        }
        if (index > 2 && index <= 3)
        {
            return Convert.ToByte((index - 2) * 255);
        }
        if (index > 3 && index <= 4)
        {
            return 255;
        }
        if (index > 4 && index <= 5)
        {
            return 255;
        }
        return 0;

    }

    public override string ToString()
    {
        return $"{Name} {TweakersPrijs ?? 0:F2} ({SingleScore:F0} / {MultiScore:F0} {Overclocked_SingleScore:F0} / {Overclocked_MultiScore:F0})";
    }
}