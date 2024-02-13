using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace CodeGenerator.Entities.Models
{
    public class EntityInfo
    {
        public EntityInfo(DbSetInfo dbSet, Type entityType)
        {
            DbSet = dbSet;
            var Type = entityType.GenericTypeArguments.First();
            Name = Type.Name;
            FullName = Type.FullName;
            Namespace = Type.Namespace;

            Properties = Type
                .GetProperties()
                .Where(a => a.IsPubliclyReadable())
                .Select(a => new EntityPropertyInfo(this, a))
                .ToArray();
        }

        public DbSetInfo DbSet { get; }
        public string Name { get; }
        public string FullName { get; }
        public string Namespace { get; }

        public EntityPropertyInfo[] Properties { get; }


        public override string ToString()
        {
            return Name;
        }
    }
}
