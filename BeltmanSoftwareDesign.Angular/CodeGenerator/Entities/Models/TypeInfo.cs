using CodeGenerator.Helpers;
using System.Reflection;

namespace CodeGenerator.Entities.Models
{
    public class TypeInfo
    {
        static string nulleblestring = "System.Nullable`1[[";
        static string nulleblestringend = "]]";

        static string isliststring = "System.Collections.Generic.List`1[[";
        static string isliststringend = "]]";

        static string isliststring2 = "System.Collections.Generic.ICollection`1[[";
        static string isliststringend2 = "]]";

        static string isarraystringend = "[]";

        public TypeInfo(PropertyInfo entityProperty, Type type)
        {
            Parent = entityProperty;

            var dbContext = entityProperty.Parent.Parent.Parent;

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
                    CsName = CsName.Substring(
                        CsNamespace.Length + 1,
                        CsName.Length - Entity.Namespace.Length - 1);
                    CsSimpleName = CsName;
                    TsName = CsName;

                    if (!IsList)
                        Key = entityProperty.Parent.Properties.First(a => a.Name == entityProperty.Name + "Id");
                }
            }
        }

        public PropertyInfo Parent { get; }

        public string? CsNamespace { get; }
        public string CsName { get; }
        public string CsFullName { get; }
        public string CsSimpleName { get; }
        public PropertyInfo? Key { get; }
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

        internal bool HasProperty(EntityInfo[] constrainedEntities)
        {
            return Entity.Properties
                .Any(a => constrainedEntities.Any(b => a.Type.Entity == b));
        }

        internal bool HasPropertyInParents(EntityInfo[] constrainedEntities)
        {
            return FindInParents(Entity, constrainedEntities);
        }
        private bool FindInParents(EntityInfo entity, EntityInfo[] entitiesToFind)
        {
            var constrainedProperties2 = entity.Properties
                .Where(a => entitiesToFind.Any(b => a.Type.Entity == b))
                .ToArray();
            if (constrainedProperties2.Any()) return true;

            var parents = entity.Properties
                .Where(a => a.Type.Entity != null && !a.Type.IsList)
                .ToArray();
            foreach (var parent in parents)
            {
                var found = FindInParents(parent.Type.Entity, entitiesToFind);
                if (found) return true;
            }
            return false;
        }
    }
}
