using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities
{
    public static class EntityHelper
    {
        private static bool FindInParents(EntityInfo ent, EntityInfo[] entitiesToFind, EntityInfo[] ConstrainedProperties)
        {
            var constrainedProperties2 = ent.Properties
                .Where(a => ConstrainedProperties.Any(b => a.Type.Entity == b))
                .ToArray();
            if (constrainedProperties2.Any()) return true;

            var parents = ent.Properties
                .Where(a => a.Type.Entity != null && !a.Type.IsList)
                .ToArray();
            foreach (var parent in parents)
            {
                var found = FindInParents(parent.Type.Entity, entitiesToFind, ConstrainedProperties);
                if (found) return true;
            }
            return false;
        }
    }
}
