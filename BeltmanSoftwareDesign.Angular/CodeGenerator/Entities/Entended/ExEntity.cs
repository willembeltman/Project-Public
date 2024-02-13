using CodeGenerator.Entities.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CodeGenerator.Entities.Extended
{
    public class ExEntity
    {
        public ExEntity(EntityInfo entity)
        {
            Name = entity.Name;
            Namespace = entity.Type.Namespace;
            Properties = entity.EntityPropertyInfos
                .Select(a => new ExEntityProperty(a))
                .ToArray();
        }

        public string Name { get; }
        public string Namespace { get; }
        public ExEntityProperty[] Properties { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
