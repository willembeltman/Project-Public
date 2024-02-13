using BeltmanSoftwareDesign.Data;
using CodeGenerator.Entities;
using CodeGenerator.Entities.Extended;
using CodeGenerator.Entities.Models;
using CodeGenerator.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class AllesGenerator
    {
        public void Run(string[] args)
        {
            var dbcontexttype = typeof(ApplicationDbContext);
            var dbcontext = new DbContextInfo(dbcontexttype);
            var extendeddbcontext = new ExDbContext(dbcontext);

            var user = extendeddbcontext.DbSetInfos.First(a => a.Entity.Name == "User");
            var userstate = new StateProperty(user, "UserId", "this.stateService.getState().user.id");

            var company = extendeddbcontext.DbSetInfos.First(a => a.Entity.Name == "Company");
            var companystate = new StateProperty(company, "CompanyId", "this.stateService.getState().currentcompany.id");

            var dto_outputdir = @"";
            var dto_namespace = @"";
            foreach (var entity in extendeddbcontext.DbSetInfos)
            {
                var template = new DtoTemplate(entity);
                var content = template.GetContent();
            }

            var list = extendeddbcontext.DbSetInfos
                .Where(a => a.Entity.Properties.Any(a => a.Name == "CompanyId"))
                .ToArray();
        }
    }

    public class StateProperty
    {

        public StateProperty(ExDbSet dbSet, string foreignKeyName, string tsGetCommand)
        {
            ForeignKeyName = foreignKeyName;
            TsGetCommand = tsGetCommand;
            DbSet = dbSet;
            Entity = dbSet.Entity;
        }

        public string ForeignKeyName { get; }
        public string TsGetCommand { get; }
        public ExDbSet DbSet { get; }
        public ExEntity Entity { get; }

        public override string ToString()
        {
            return ForeignKeyName;
        }
    }
}
