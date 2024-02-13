using System.Reflection;
using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Extended
{
    public class ExDbSet
    {
        public ExDbSet(DbSetInfo a)
        {
            Name = a.Name;
            Entity = new ExEntity(a.Entity);
        }

        public string Name { get; }
        public ExEntity Entity { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
