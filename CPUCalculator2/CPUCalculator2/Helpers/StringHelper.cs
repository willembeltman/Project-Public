namespace CPUCalculator2.Helpers;

public static class StringHelper
{
    public static string FixName(string name)
    {
        return name
            .Replace(" Quad-Core", "")
            .Replace(" Six-Core", "")
            .Replace(" Eight-Core", "")
            .Replace(" APU", "");
        //.Replace(" PRO", "");
    }
    public static string NumberFormat(string value)
    {
        string id_allowed = "1234567890.";
        string rtn = "";
        foreach (char c in value)
        {
            if (id_allowed.Contains(c)) rtn += c;
        }
        return rtn;
    }
}
