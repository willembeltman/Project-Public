using System.Reflection;

namespace CodeGenerator.Entities.Models
{
    public class DbSetInfo
    {
        public DbSetInfo(DbContextInfo dbContextInfo, PropertyInfo propertyInfo)
        {
            _DbContextInfo = dbContextInfo;

            Name = propertyInfo.Name;
            Entity = new EntityInfo(this, propertyInfo.PropertyType);
        }

        public DbContextInfo _DbContextInfo { get; }
        public string Name { get; }
        public EntityInfo Entity { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
