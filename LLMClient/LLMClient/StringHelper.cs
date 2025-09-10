namespace LLMClient;

public static class StringHelper
{
    public static string EncodeMessagePreserveNewlines(this string input)
    {
        // Vervang CRLF en CR door \n (escape)
        return input.Replace("\r\n", "\\n").Replace("\r", "\\n").Replace("\n", "\\n");
    }
    public static string EscapeArgument(this string arg)
    {
        // Minimal escaping for spaces
        if (arg.Contains(" "))
        {
            return $"\"{arg}\"";
        }
        return arg;
    }
}

