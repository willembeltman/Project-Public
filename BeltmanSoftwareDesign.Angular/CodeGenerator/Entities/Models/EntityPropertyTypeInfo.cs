using CodeGenerator.Helpers;
using System.Reflection;

namespace CodeGenerator.Entities.Models
{
    public class EntityPropertyTypeInfo
    {
        static string nulleblestring = "System.Nullable`1[[";
        static string nulleblestringend = "]]";

        static string isliststring = "System.Collections.Generic.List`1[[";
        static string isliststringend = "]]";

        static string isliststring2 = "System.Collections.Generic.ICollection`1[[";
        static string isliststringend2 = "]]";

        static string isarraystringend = "[]";

        public EntityPropertyTypeInfo(DbContextInfo dbContext, Type type)
        {
            if (type.FullName == null) throw new Exception();

            CsFullName = type.FullName;
            CsName = CsFullName;

            if (type.IsEnum || type.GenericTypeArguments.Any(a => a.IsEnum))
            {
                IsEnum = true;

                if (CsName.StartsWith(nulleblestring))
                {
                    IsNulleble = true;
                    var end = CsName.IndexOf(nulleblestringend);
                    var csv = CsName.Substring(nulleblestring.Length, end - nulleblestring.Length);
                    var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                    CsName = csvlines.First();
                }

                var full = CsName;
                var split = full.Split(new char[] { '.' });
                CsName = split.Last();
                CsSimpleName = CsName;
                TsName = CsName;
                CsNamespace = full.Substring(0, full.Length - CsName.Length - 1);
            }
            else
            {
                if (CsName.StartsWith(isliststring))
                {
                    IsList = true;
                    var end = CsName.IndexOf(isliststringend);
                    var csv = CsName.Substring(isliststring.Length, end - isliststring.Length);
                    var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                    CsName = csvlines.First();
                }
                if (CsName.StartsWith(isliststring2))
                {
                    IsList = true;
                    var end = CsName.IndexOf(isliststringend2);
                    var csv = CsName.Substring(isliststring2.Length, end - isliststring2.Length);
                    var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                    CsName = csvlines.First();
                }
                if (CsName.StartsWith(nulleblestring))
                {
                    IsNulleble = true;
                    var end = CsName.IndexOf(nulleblestringend);
                    var csv = CsName.Substring(nulleblestring.Length, end - nulleblestring.Length);
                    var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                    CsName = csvlines.First();
                }

                DbSet = dbContext.DbSetInfos
                    .FirstOrDefault(b => b.Entity.FullName == CsName);
                Entity = DbSet?.Entity;

                // Ken ik het type al?
                if (Entity == null)
                {
                    TsName = NameHelper.GetTsType(CsName) ?? throw new Exception("Gaat niet goed...");
                    CsSimpleName = NameHelper.GetSimpleCsType(CsName) ?? throw new Exception("Gaat niet goed...");
                }
                else
                {
                    if (Entity.Namespace == null) throw new Exception();

                    CsNamespace = Entity.Namespace;
                    CsName = CsFullName.Substring(
                        CsNamespace.Length + 1,
                        CsFullName.Length - Entity.Namespace.Length - 1);
                    CsSimpleName = CsName;
                    TsName = CsName;
                }
            }
        }

        public string? CsNamespace { get; }
        public string CsName { get; }
        public string CsFullName { get; }
        public string CsSimpleName { get; }
        public string TsName { get; }

        public bool IsNulleble { get; } = false;
        public bool IsList { get; } = false;
        public bool IsEnum { get; } = false;

        public DbSetInfo? DbSet { get; }
        public EntityInfo? Entity { get; }

        public override string ToString()
        {
            return CsName;
        }
    }
}
