using System.Reflection;

namespace CodeGenerator.Entities.Models
{
    public class DbSetInfo
    {
        public DbSetInfo(DbContextInfo dbContextInfo, PropertyInfo propertyInfo)
        {
            DbContext = dbContextInfo;

            Name = propertyInfo.Name;
            Entity = new EntityInfo(this, propertyInfo.PropertyType);
        }

        public DbContextInfo DbContext { get; }
        public string Name { get; }
        public EntityInfo Entity { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
