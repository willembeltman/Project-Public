using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Templates
{
    internal class TsInterfaceTemplate
    {
        private DbSetInfo DbSet { get; }
        public EntityInfo Entity { get; }
        public string NamespaceName { get; }

        public TsInterfaceTemplate(DbSetInfo dbSet, string namespaceName = "BeltmanSoftwareDesign.Shared.Jsons")
        {
            DbSet = dbSet;
            Entity = dbSet.Entity;
            NamespaceName = namespaceName;
        }

        public string GetContent()
        {
            var text = "";

            //var namespaces = Entity.Properties
            //    .Where(a => a.Type.Entity == null)
            //    .GroupBy(a => a.Type.CsNamespace)
            //    .Select(a => a.Key)
            //    .Where(a => a != null && a != NamespaceName)
            //    .ToArray();

            //foreach (var ns in namespaces)
            //{
            //    text += $"using {ns};" + Environment.NewLine;
            //}

            //if (namespaces.Any())
            //{
            //    text += Environment.NewLine;
            //}

            //text += $"namespace {NamespaceName};" + Environment.NewLine;
            //text += $"{{" + Environment.NewLine;
            //text += $"    public class {Entity.Name}" + Environment.NewLine;
            //text += $"    {{" + Environment.NewLine;
            //foreach (var item in Entity.Properties)
            //{
            //    if (item.Type.Entity == null)
            //    {
            //        text += $"        public {item.Type.CsSimpleName} {item.Name} {{ get; set; }}" + Environment.NewLine;
            //    }
            //    else
            //    {
            //        var nameproperty = item.Type.Entity.Properties.FirstOrDefault(a => a.IsName);
            //        if (nameproperty != null)
            //        {
            //            text += $"        public {nameproperty.Type.CsSimpleName} {item.Name}Name {{ get; set; }}" + Environment.NewLine;
            //        }
            //    }
            //}
            //text += $"    }}" + Environment.NewLine;
            //text += $"}}" + Environment.NewLine;

            return text;
        }

        public string GetFullName(string angular_app_directory, string TsFolder, string entityName)
        {
            return angular_app_directory + "\\" + TsFolder.Replace("/", "\\") + "\\" + entityName.ToLower() + ".ts";
        }
    }
}