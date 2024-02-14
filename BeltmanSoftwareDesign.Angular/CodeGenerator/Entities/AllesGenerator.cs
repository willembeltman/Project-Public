using BeltmanSoftwareDesign.Data;
using CodeGenerator.Entities.Models;
using CodeGenerator.Entities.Templates;

namespace CodeGenerator.Entities
{
    public class AllesGenerator
    {
        static string angular_app_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Angular\src\app";

        static string csharp_shared_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Shared";
        static string csharp_shared_namespace = @"BeltmanSoftwareDesign.Shared";

        static string csharp_api_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Api";
        static string csharp_api_namespace = @"BeltmanSoftwareDesign.Api";

        static string csharp_business_directory = @"D:\Project-Public\BeltmanSoftwareDesign.Angular\BeltmanSoftwareDesign.Business";
        static string csharp_business_namespace = @"BeltmanSoftwareDesign.Business";

        public void Run(string[] args)
        {
            var dbcontexttype = typeof(ApplicationDbContext);
            var dbcontext = new DbContextInfo(dbcontexttype);
            //var extendeddbcontext = new ExDbContext(dbcontext);

            var user = dbcontext.DbSetInfos.First(a => a.Entity.Name == "User");
            var userconstrain = new ConstrainedProperty(user, "UserId", "this.stateService.getState().user.id");

            var company = dbcontext.DbSetInfos.First(a => a.Entity.Name == "Company");
            var companyconstrain = new ConstrainedProperty(company, "CompanyId", "this.stateService.getState().currentcompany.id");

            var constrainedProperties = new []
            {
                userconstrain, companyconstrain
            };

            var list = dbcontext.DbSetInfos
                .Where(a => a.Entity.Properties.Any(a => constrainedProperties.Any(b => a.Name == b.ForeignKeyName)))
                .ToArray();



            // C# Jsons
            foreach (var entity in dbcontext.DbSetInfos)
            {
                var template = new CsJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, constrainedProperties);
                var content = template.GetContent();
                var filename = template.GetFullName();
                WriteToFile(filename, content);
            }

            // C# RequestJsons

            // C# ResponseJsons

            // TS Interfaces
            //foreach (var entity in dbcontext.DbSetInfos)
            //{
            //    var template = new TsInterfaceTemplate(entity);
            //    var content = template.GetContent();
            //    var filename = template.GetFullName(angular_app_directory, TsFolder, entity.Name);
            //    WriteToFile(filename, content);
            //}

            // TS RequestJsons

            // TS ResponseJsons

            // C# Services

            // C# Controllers

            // TS Services

            // List for components
            var list2 = dbcontext.DbSetInfos
                .Where(a => a.Entity.Properties.Any(a => a.Name == "CompanyId"))
                .ToArray();

            // List TS components

            // Details TS components

            // Edit TS components

            // Delete TS components

        }

        private void WriteToFile(string filename, string content)
        {
            Console.WriteLine(filename);
            Console.WriteLine(content);
            Console.WriteLine();
        }
    }

    public class ConstrainedProperty
    {

        public ConstrainedProperty(DbSetInfo dbSet, string foreignKeyName, string tsGetCommand)
        {
            ForeignKeyName = foreignKeyName;
            TsGetCommand = tsGetCommand;
            DbSet = dbSet;
            Entity = dbSet.Entity;
        }

        public string ForeignKeyName { get; }
        public string TsGetCommand { get; }
        public DbSetInfo DbSet { get; }
        public EntityInfo Entity { get; }

        public override string ToString()
        {
            return ForeignKeyName;
        }
    }
}
