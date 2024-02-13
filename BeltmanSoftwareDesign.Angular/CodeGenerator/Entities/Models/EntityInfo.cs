using CodeGenerator.Entities.Extended;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Xml.Linq;

namespace CodeGenerator.Entities.Models
{
    public class EntityInfo
    {
        public EntityInfo(DbSetInfo dbSet, Type entityType)
        {
            _DbSetInfo = dbSet;
            Type = entityType.GenericTypeArguments.First();
            Name = Type.Name;
            Namespace = Type.Namespace;
            var properties = Type
                .GetProperties();
            EntityPropertyInfos = properties
                .Where(a => a.IsPubliclyReadable())
                .Select(a => new EntityPropertyInfo(this, a))
                .ToArray();
        }

        public DbSetInfo _DbSetInfo { get; }
        public string Name { get; }
        public string? Namespace { get; }
        public Type Type { get; }
        public EntityPropertyInfo[] EntityPropertyInfos { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
