using CodeGenerator.Entities.Extended;

namespace CodeGenerator.Templates
{
    internal class DtoTemplate
    {
        private ExDbSet DbSet { get; }
        public ExEntity Entity { get; }
        public string NamespaceName { get; }

        public DtoTemplate(ExDbSet dbSet, string namespaceName = "BeltmanSoftwareDesign.Shared.Jsons")
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

            var rtn = "";

            rtn += $"namespace {NamespaceName};" + Environment.NewLine;
            rtn += $"{{" + Environment.NewLine;
            rtn += $"    public class {Entity.Name}" + Environment.NewLine;
            rtn += $"    {{" + Environment.NewLine;
            foreach (var item in Entity.Properties)
            {
                rtn += $"        public {item.TypeCsSimpleName} {item.Name} {{ get; set; }}" + Environment.NewLine;
            }
            rtn += $"    }}" + Environment.NewLine;
            rtn += $"}}" + Environment.NewLine;

            return rtn;
        }
    }
}