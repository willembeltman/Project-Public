using System.Reflection;
using System.Xml.Linq;

namespace CodeGenerator.Entities.Models
{
    public class PropertyInfo
    {
        public PropertyInfo(EntityInfo entityInfo, System.Reflection.PropertyInfo propertyInfo)
        {
            Parent = entityInfo;
            Name = propertyInfo.Name;
            IsKey = propertyInfo.CustomAttributes
                .Any(a => a.AttributeType.Name == "KeyAttribute");
            IsName = propertyInfo.CustomAttributes
                .Any(a => a.AttributeType.Name == "NameAttribute");
            _TypeToLoad = propertyInfo.PropertyType;
        }


        public EntityInfo Parent { get; }
        public string Name { get; }
        public bool IsKey { get; }
        public bool IsName { get; }


        private Type _TypeToLoad { get; }
        private TypeInfo _Type { get; set; }
        public TypeInfo Type
        {
            get
            {
                if (_Type == null)
                    _Type = new TypeInfo(this, _TypeToLoad);
                return _Type;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
