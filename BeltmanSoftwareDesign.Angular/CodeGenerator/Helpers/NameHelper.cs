namespace CodeGenerator.Helpers
{
    public static class NameHelper
    {
        public static string? GetTsType(string propertytype)
        {
            switch (propertytype)
            {
                case "System.Int64":
                case "System.Int32":
                case "System.Double":
                    return "number";
                case "System.String":
                case "System.DateTime":
                    return "string";
                case "System.Boolean":
                    return "boolean";
            }
            return null;
        }
        public static string UpperCaseFirstLetter(string value)
        {
            return value.Substring(0, 1).ToUpper() + value.Substring(1, value.Length - 1);
        }
        public static string LowerCaseFirstLetter(string value)
        {
            return value.Substring(0, 1).ToLower() + value.Substring(1, value.Length - 1);
        }
    }
}
