using System.Reflection;

namespace CodeGenerator.Entities.Models
{
    public class EntityPropertyInfo
    {
        public EntityPropertyInfo(EntityInfo entityInfo, PropertyInfo propertyInfo)
        {
            _EntityInfo = entityInfo;
            Name = propertyInfo.Name;
            PropertyInfo = propertyInfo;
            Type = propertyInfo.PropertyType;
        }

        public EntityInfo _EntityInfo { get; }
        public string Name { get; }
        public PropertyInfo PropertyInfo { get; }
        public Type Type { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
