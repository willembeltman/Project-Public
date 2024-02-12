using CodeGenerator.Helpers;
using System.Reflection;

namespace CodeGenerator.Models
{
    public class ModelProperty
    {
        public ModelProperty(Model model, PropertyInfo propertyInfo)
        {
            Model = model;

            Name = propertyInfo.Name;
            NameLower = NameHelper.LowerCaseFirstLetter(propertyInfo.Name);
            _Type = propertyInfo.PropertyType;
        }

        Model Model { get; }
        public string Name { get; }
        public string NameLower { get; }
        
        Type _Type { get; }
        TypeRapport _TypeRapport { get; set; }
        public TypeRapport Type
        {
            get
            {
                if (_TypeRapport == null)
                    _TypeRapport = new TypeRapport(_Type, Model.ModelsNamespace.ModelsNamespacesList);
                return _TypeRapport;
            }
        }

        //public string TypeFullName => Type.FullName;
        //public string TypeName => Type.Name;
        //public string TypeTsName => Type.TsName;
        //public bool TypeNulleble => Type.Nulleble;
        //public bool TypeList => Type.List;
        //public bool TypeImport => Type.Import;
        //public Model? TypeImportModel => Type.ImportModel;

        public override string ToString()
        {
            return Name;
        }
    }
}