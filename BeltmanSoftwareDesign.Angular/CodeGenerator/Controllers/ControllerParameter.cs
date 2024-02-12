using CodeGenerator.Models;

namespace CodeGenerator.Controllers
{
    public class ControllerParameter
    {
        public ControllerParameter(string? name, string typeName, string typescriptType, bool nulleble, bool isList, bool import, Models.ModelNamespace? importNamespace)
        {
            Name = name;
            TypeName = typeName;
            TsType = typescriptType;
            Nulleble = nulleble;
            IsList = isList;
            Import = import;
            ImportNamespace = importNamespace;
        }

        public string? Name { get; }
        public string TypeName { get; }
        public string TsType { get; }
        public bool Nulleble { get; }
        public bool IsList { get; }
        public bool Import { get; }
        public ModelNamespace? ImportNamespace { get; }
    }
}