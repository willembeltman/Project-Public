
using CodeGenerator.Helpers;

namespace CodeGenerator.Models
{

    public class Model
    {
        public Model(ModelNamespace modelsNamespace, Type type)
        {
            ModelsNamespace = modelsNamespace;

            FullName = type.FullName;
            var filter = "+<>c";
            if (FullName.EndsWith(filter))
                FullName = FullName.Substring(0, FullName.Length - filter.Length);

            Name = FullName.Split(new char[] { '.' }).Last();

            //Name = FullName.Substring(modelNamespace.Name.Length + 1, FullName.Length - modelNamespace.Name.Length - 1);
            NameLower = NameHelper.LowerCaseFirstLetter(Name);
            Properties = type.GetProperties()
                .Where(a => 
                    !string.IsNullOrEmpty(a.Name) && 
                    !string.IsNullOrEmpty(a.PropertyType?.FullName))
                .Select(a => new ModelProperty(this, a))
                .ToArray();
        }

        public ModelNamespace ModelsNamespace { get; }
        public string? FullName { get; }
        public string Name { get; }
        public string NameLower { get; }

        public ModelProperty[] Properties { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}