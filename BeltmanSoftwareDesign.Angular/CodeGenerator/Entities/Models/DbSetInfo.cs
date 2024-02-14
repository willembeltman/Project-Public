using System.Reflection;

namespace CodeGenerator.Entities.Models
{
    public class DbSetInfo
    {
        public DbSetInfo(DbContextInfo dbContextInfo, System.Reflection.PropertyInfo propertyInfo)
        {
            Parent = dbContextInfo;

            Name = propertyInfo.Name;
            Entity = new EntityInfo(this, propertyInfo.PropertyType);
        }

        public DbContextInfo Parent { get; }

        public string Name { get; }
        public EntityInfo Entity { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
