using CodeGenerator.Entities.Models;

namespace CodeGenerator.Templates
{
    internal class DtoTemplate
    {
        private DbSetInfo DbSet { get; }
        public EntityInfo Entity { get; }
        public string NamespaceName { get; }

        public DtoTemplate(DbSetInfo dbSet, string namespaceName = "BeltmanSoftwareDesign.Shared.Jsons")
        {
            DbSet = dbSet;
            Entity = dbSet.Entity;
            NamespaceName = namespaceName;
        }



        public string GetContent()
        {

            //
            //        public long? CountryId { get; set; }
            //        public string? CountryName { get; set; }
            //
            //        public string Name { get; set; } = "";
            //        public string? Address { get; set; }
            //        public string? Postalcode { get; set; }
            //        public string? Place { get; set; }
            //        public string? PhoneNumber { get; set; }
            //        public string Email { get; set; } = "";
            //        public string? Website { get; set; }
            //        public string? BtwNumber { get; set; }
            //        public string? KvkNumber { get; set; }
            //        public string? Iban { get; set; }
            //

            var namespaces = Entity.Properties
                .Where(a => a.Type.Entity == null)
                .GroupBy(a => a.Type.CsNamespace)
                .Select(a => a.Key)
                .Where(a => a != null && a != NamespaceName)
                .ToArray();

            var rtn = "";

            foreach (var ns in namespaces)
            {
                rtn += $"using {ns};" + Environment.NewLine;
            }

            if (namespaces.Any())
            {
                rtn += Environment.NewLine;
            }

            rtn += $"namespace {NamespaceName};" + Environment.NewLine;
            rtn += $"{{" + Environment.NewLine;
            rtn += $"    public class {Entity.Name}" + Environment.NewLine;
            rtn += $"    {{" + Environment.NewLine;
            foreach (var item in Entity.Properties)
            {
                if (item.Type.Entity == null)
                {
                    rtn += $"        public {item.Type.CsSimpleName} {item.Name} {{ get; set; }}" + Environment.NewLine;
                    if (item.Type.IsEnum)
                    {

                    }
                    else
                    {
                        //rtn += $"        public {item.Type.CsSimpleName} {item.Name} {{ get; set; }}" + Environment.NewLine;
                    }
                }
                else
                {
                    var nameproperty = item.Type.Entity.Properties.FirstOrDefault(a => a.IsName);
                    if (nameproperty != null)
                    {
                        rtn += $"        public {nameproperty.Type.CsSimpleName} {item.Name}Name {{ get; set; }}" + Environment.NewLine;
                    }

                }
            }
            rtn += $"    }}" + Environment.NewLine;
            rtn += $"}}" + Environment.NewLine;

            return rtn;
        }
    }
}