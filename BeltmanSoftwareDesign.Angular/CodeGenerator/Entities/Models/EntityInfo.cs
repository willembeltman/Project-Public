using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace CodeGenerator.Entities.Models
{
    public class EntityInfo
    {
        public EntityInfo(DbSetInfo dbSet, Type entityType)
        {
            Parent = dbSet;
            var Type = entityType.GenericTypeArguments.First();
            Name = Type.Name;
            FullName = Type.FullName;
            Namespace = Type.Namespace;

            Properties = Type
                .GetProperties()
                .Where(a => a.IsPubliclyReadable())
                .Select(a => new PropertyInfo(this, a))
                .ToArray();
        }

        public DbSetInfo Parent { get; }

        public string Name { get; }
        public string FullName { get; }
        public string Namespace { get; }

        public PropertyInfo[] Properties { get; }


        public override string ToString()
        {
            return Name;
        }
    }
}
