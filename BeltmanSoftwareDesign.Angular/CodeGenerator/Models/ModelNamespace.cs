using System.Reflection;

namespace CodeGenerator.Models
{
    public class ModelNamespace
    {
        public ModelNamespace(ModelNamespacesList modelsNamespacesList, Assembly assembly, string name, string tsFolder)
        {
            ModelsNamespacesList = modelsNamespacesList;
            Name = name;
            TsFolder = tsFolder;

            Models = assembly.GetTypes()
                .Where(a =>
                    a.IsVisible &&
                    a.Namespace == name && 
                    !a.CustomAttributes.Any(b => b.AttributeType.Name == "TsHiddenAttribute"))
                .Select(a => new Model(this, a))
                .ToArray();
        }

        public ModelNamespacesList ModelsNamespacesList { get; }
        public string Name { get; }
        public string TsFolder { get; }
        public Model[] Models { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}