using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Templates
{
    internal class TsInterfaceTemplate : ITemplate
    {
        public EntityInfo Entity { get; }
        public string Angular_app_directory { get; }

        public TsInterfaceTemplate(EntityInfo entity, string angular_app_directory) // = "BeltmanSoftwareDesign.Shared.Jsons")
        {
            Entity = entity;
            Angular_app_directory = angular_app_directory;
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

        public string GetFullName()
        {
            return Angular_app_directory + "\\Interfaces\\" + Entity.Name.ToLower() + ".ts";
        }
    }
}