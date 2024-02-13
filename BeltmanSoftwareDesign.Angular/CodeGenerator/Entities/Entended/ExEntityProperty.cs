using CodeGenerator.Entities.Models;
using CodeGenerator.Helpers;
using CodeGenerator.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace CodeGenerator.Entities.Extended
{
    public class ExEntityProperty
    {
        static string nulleblestring = "System.Nullable`1[[";
        static string nulleblestringend = "]]";

        static string isliststring = "System.Collections.Generic.List`1[[";
        static string isliststringend = "]]";

        static string isliststring2 = "System.Collections.Generic.ICollection`1[[";
        static string isliststringend2 = "]]";

        static string isarraystringend = "[]";

        public ExEntityProperty(EntityPropertyInfo entityProperty)
        {
            var dbcontext = entityProperty._EntityInfo._DbSetInfo._DbContextInfo;

            IsKey = entityProperty.PropertyInfo.CustomAttributes
                .Any(a => a.AttributeType.Name == "KeyAttribute");
            IsName = entityProperty.PropertyInfo.CustomAttributes
                .Any(a => a.AttributeType.Name == "NameAttribute");

            Name = entityProperty.Name;
            TypeCsFullName = entityProperty.Type.FullName;
            TypeCsName = entityProperty.Type.FullName;

            if (TypeCsName.StartsWith(nulleblestring))
            {
                IsNulleble = true;
                var end = TypeCsName.IndexOf(nulleblestringend);
                var csv = TypeCsName.Substring(nulleblestring.Length, end - nulleblestring.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                TypeCsName = csvlines.First();
            }

            if (TypeCsName.StartsWith(isliststring))
            {
                IsList = true;
                var end = TypeCsName.IndexOf(isliststringend);
                var csv = TypeCsName.Substring(isliststring.Length, end - isliststring.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                TypeCsName = csvlines.First();
            }

            if (TypeCsName.StartsWith(isliststring2))
            {
                IsList = true;
                var end = TypeCsName.IndexOf(isliststringend2);
                var csv = TypeCsName.Substring(isliststring2.Length, end - isliststring2.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                TypeCsName = csvlines.First();
            }

            DbSet = dbcontext.DbSetInfos
                .FirstOrDefault(b => b.Entity.Type.FullName == TypeCsName);
            Entity = DbSet?.Entity;


            if (Entity == null)
            {
                TypeTsName = NameHelper.GetTsType(TypeCsName);
                TypeCsSimpleName = NameHelper.GetSimpleCsType(TypeCsName);
                if (TypeCsSimpleName == null || TypeTsName == null)
                {
                    // Is een enum?
                }
            }
            else
            {
                TypeCsName = TypeCsFullName.Substring(
                    Entity.Namespace.Length + 1,
                    TypeCsFullName.Length - Entity.Namespace.Length - 1);
                TypeCsSimpleName = TypeCsName;
                TypeTsName = TypeCsName;
            }
        }

        public string Name { get; }
        public string TypeCsName { get; }
        public string TypeCsFullName { get; }
        public string TypeCsSimpleName { get; }
        public string TypeTsName { get; }

        public bool IsKey {  get; }
        public bool IsName { get; }
        public bool IsNulleble { get; }
        public bool IsList { get; }

        public DbSetInfo? DbSet { get; }
        public EntityInfo? Entity { get; }


        public override string ToString()
        {
            return Name;
        }
    }
}
