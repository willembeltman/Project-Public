using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Templates
{
    internal class CsJsonTemplate : ITemplate
    {
        public EntityInfo Entity { get; }
        public string NamespaceName { get; }
        public string Directory { get; }
        public ConstrainedProperty[] ConstrainedProperties { get; }

        public CsJsonTemplate(EntityInfo entity, string directory, string @namespace, ConstrainedProperty[] constrainedProperties)
        {
            Entity = entity;
            NamespaceName = $"{@namespace}.Jsons";
            Directory = directory + @"\Jsons";
            ConstrainedProperties = constrainedProperties;
        }

        public string GetContent()
        {
            var rtn = "";

            var namespaces = Entity.Properties
                .Where(a => a.Type.Entity == null)
                .GroupBy(a => a.Type.CsNamespace)
                .Select(a => a.Key)
                .Where(a => a != null && a != NamespaceName)
                .ToArray();

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
                }
                else
                {
                    if (item.Type.IsList)
                    {
                        var constrainedEntities = ConstrainedProperties.Select(a => a.Entity).ToArray();
                        if (!item.Type.HasProperty(constrainedEntities) &&
                            item.Type.HasPropertyInParents(constrainedEntities))
                        {
                            rtn += $"        public {item.Type.CsSimpleName}[] {item.Name} {{ get; set; }}" + Environment.NewLine;
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
            }
            rtn += $"    }}" + Environment.NewLine;
            rtn += $"}}" + Environment.NewLine;

            return rtn;
        }


        public string GetFullName()
        {
            return Directory + "\\" + Entity.Name + ".cs";
        }
    }
}