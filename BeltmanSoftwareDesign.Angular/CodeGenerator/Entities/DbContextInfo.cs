using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Entities
{
    public class DbContextInfo
    {
        static string dbsetnamestart = "Microsoft.EntityFrameworkCore.DbSet`1[[";

        public DbContextInfo(Type dbContextType)
        {
            Name = dbContextType.Name;
            Type = dbContextType;
            DbSetInfos = dbContextType
                .GetProperties()
                .Where(a => a.PropertyType.FullName.StartsWith(dbsetnamestart, StringComparison.OrdinalIgnoreCase))
                .Select(a => new DbSetInfo(this, a))
                .ToArray();
        }

        public string Name { get; set; }
        public Type Type { get; }
        public DbSetInfo[] DbSetInfos { get; }

        public override string ToString()
        {
            return Name;
        }
    }

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

    public class EntityInfo
    {
        public EntityInfo(DbSetInfo dbSet, Type entityType)
        {
            _DbSetInfo = dbSet;
            Type = entityType.GenericTypeArguments.First();
                Name = Type.Name;
                EntityPropertyInfos = Type
                    .GetProperties()
                    .Where(a => a.IsPubliclyReadable())
                    .Select(a => new EntityPropertyInfo(this, a))
                    .ToArray();
        }

        public DbSetInfo _DbSetInfo { get; }
        public string Name { get; }
        public Type Type { get; }
        public EntityPropertyInfo[] EntityPropertyInfos { get; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class EntityPropertyInfo
    {
        public EntityPropertyInfo(EntityInfo entityInfo, PropertyInfo propertyInfo)
        {
            _EntityInfo = entityInfo;
            Name = propertyInfo.Name;
            Type = propertyInfo.PropertyType;
        }

        public EntityInfo _EntityInfo { get; }
        public string Name { get; }
        public Type Type { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
