using System.Reflection;
using System.Xml.Linq;

namespace CodeGenerator.Entities.Models
{
    public class EntityPropertyInfo
    {
        public EntityPropertyInfo(EntityInfo entityInfo, PropertyInfo propertyInfo)
        {
            Entity = entityInfo;
            Name = propertyInfo.Name;
            IsKey = propertyInfo.CustomAttributes
                .Any(a => a.AttributeType.Name == "KeyAttribute");
            IsName = propertyInfo.CustomAttributes
                .Any(a => a.AttributeType.Name == "NameAttribute");
            _TypeToLoad = propertyInfo.PropertyType;
        }


        public EntityInfo Entity { get; }
        public string Name { get; }
        public bool IsKey { get; }
        public bool IsName { get; }


        private Type _TypeToLoad { get; }
        private EntityPropertyTypeInfo _Type { get; set; }
        public EntityPropertyTypeInfo Type
        {
            get
            {
                if (_Type == null)
                    _Type = new EntityPropertyTypeInfo(Entity.DbSet.DbContext, _TypeToLoad);
                return _Type;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
