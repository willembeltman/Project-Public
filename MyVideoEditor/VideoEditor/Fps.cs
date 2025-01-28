namespace VideoEditor;

public class Fps
{
    public Fps()
    {
        Base = 25;
        Divider = 1;
    }

    public Fps(long @base, long divider)
    {
        Base = @base;
        Divider = divider;
    }

    public long Base { get; set; }
    public long Divider { get; set; }

    public double Value
    {
        get
        {
            return Convert.ToDouble(Base) / Divider;
        }
    }

    public static bool TryParse(string? value, out Fps? result)
    {
        result = null;

        if (value == null) return false;

        var list = value.Split(['/'], StringSplitOptions.RemoveEmptyEntries);

        if (list.Length != 2) return false;
        if (!long.TryParse(list[0], out var @base)) return false;
        if (!long.TryParse(list[1], out var divider)) return false;

        result = new Fps(@base, divider);
        return true;
    }

    public override string ToString()
    {
        return $"{Value}";
    }
}

