namespace LLMClient
{
    public static class StringHelper
    {
        public static string EncodeMessagePreserveNewlines(string input)
        {
            // Vervang CRLF en CR door \n (escape)
            return input.Replace("\r\n", "\\n").Replace("\r", "\\n").Replace("\n", "\\n");
        }
    }
}

