using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Extended
{
    public class ExDbContext
    {
        public ExDbContext(DbContextInfo dbContextInfo)
        {
            Name = dbContextInfo.Name;
            DbSetInfos = dbContextInfo.DbSetInfos
                .Select(a => new ExDbSet(a))
                .OrderBy(a => a.Entity.Name)
                .ToArray();
        }

        public string Name { get; set; }
        public ExDbSet[] DbSetInfos { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
