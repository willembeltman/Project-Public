namespace BeltmanSoftwareDesign.Business.Helpers
{
    public static class HashGeneratorHelper
    {
        static Random random = new Random();
        static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string GenerateCode(int length)
        {
            char[] code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
        }
    }
}
