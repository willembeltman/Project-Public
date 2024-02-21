using BeltmanSoftwareDesign.Data;
using CodeGenerator.Entities.Models;
using CodeGenerator.Entities.Templates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CodeGenerator
{
    public class GenerateFromEntities_NotUsed
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

            // Excluded entities, these will not be generated
            var excluded = new[]
            {
                "ClientBearer",
                "ClientDevice",
                "ClientDeviceProperty",
                "ClientIpAdress",
                "CompanyUser",
                "Transaction",
                "InvoiceTransaction"
            };

            // Properties available from the state, these will also not be generated
            var stateList = new[]
            {
                userconstrain, companyconstrain
            };

            // Alle entities die wij kunnen opvragen met een van de id's
            var mainEntitiesList = dbcontext.DbSetInfos
                .Select(a => a.Entity)
                .Where(a =>
                {
                    var hasStateProperty = a.Properties.Any(a => !a.Type.IsList && stateList.Any(b => b.Entity == a.Type.Entity));
                    var isNotStateProperty = !stateList.Any(b => b.Entity == a);
                    var isNotExcluded = !excluded.Any(b => b == a.Name);
                    return
                        hasStateProperty &&
                        isNotStateProperty &&
                        isNotExcluded;
                })
                .ToArray();

            var propertiesEntitiesList =
                mainEntitiesList
                    .SelectMany(a => a.Properties)
                    .Where(a => a.Type.Entity != null)
                    .Select(a => a.Type.Entity)
                    .OrderBy(a => a.Name)
                    .Where(a =>
                    {
                        var notInCrudList = !mainEntitiesList.Contains(a);
                        var noCrudListUnknownProperties = !a.Properties.Any(a => a.Type.Entity != null && !mainEntitiesList.Any(b => b == a.Type.Entity));
                        var isNotStateProperty = !stateList.Any(b => b.Entity == a);
                        var isNotExcluded = !excluded.Any(b => b == a.Name);
                        return
                            notInCrudList &&
                            noCrudListUnknownProperties &&
                            isNotStateProperty &&
                            isNotExcluded;
                    })
                    .GroupBy(a => a)
                    .Select(a => a.Key)
                    .ToArray();

            var readonlyEntitiesList =
                mainEntitiesList
                    .SelectMany(a => a.Properties)
                    .Where(a => a.Type.Entity != null)
                    .Select(a => a.Type.Entity)
                    .OrderBy(a => a.Name)
                    .Where(a =>
                    {
                        var notInCrudList = !mainEntitiesList.Contains(a);
                        var noCrudListUnknownProperties = a.Properties.Any(a => a.Type.Entity != null && !mainEntitiesList.Any(b => b == a.Type.Entity));
                        var isNotStateProperty = !stateList.Any(b => b.Entity == a);
                        var isNotExcluded = !excluded.Any(b => b == a.Name);
                        return
                            notInCrudList &&
                            noCrudListUnknownProperties &&
                            isNotStateProperty &&
                            isNotExcluded;
                    })
                    .GroupBy(a => a)
                    .Select(a => a.Key)
                    .ToArray();

            var attachmentEntitiesList = propertiesEntitiesList
                .Where(a => a.IsStorageFile)
                .ToArray();

            var extendedAttachmentEntitiesList = attachmentEntitiesList
                .Where(a => a.Properties.Where(a => a.Type.Entity != null).Count() > 1)
                .ToArray();
            if (extendedAttachmentEntitiesList.Any())
                throw new NotImplementedException("Dat ondersteun ik niet!");

            var uitgebreideKoppelList = propertiesEntitiesList
                .Where(a => !attachmentEntitiesList.Contains(a))
                .Where(a =>
                    a.Properties.Where(a => a.Type.Entity != null).Count() > 2 ||
                    a.Properties.Where(a => a.Type.Entity == null).Count() > 3)
                .ToArray();

            var simpeleKoppelList = propertiesEntitiesList
                .Where(a => !attachmentEntitiesList.Contains(a))
                .Where(a => !uitgebreideKoppelList.Contains(a))
                .Where(a => a.Properties.Where(a => a.Type.Entity != null).Count() == 2)
                .ToArray();

            var overgeblevenKoppelList = propertiesEntitiesList
                .Where(a => !attachmentEntitiesList.Contains(a))
                .Where(a => !uitgebreideKoppelList.Contains(a))
                .Where(a => !simpeleKoppelList.Contains(a))
                .ToArray();
            if (overgeblevenKoppelList.Any())
                throw new NotImplementedException("Dat ondersteun ik niet!");

            var dtos = mainEntitiesList
                .Concat(propertiesEntitiesList)
                .Concat(readonlyEntitiesList)
                .OrderBy(a => a.Name)
                .ToArray();

            // C# Jsons voor alles
            foreach (var entity in dtos)
            {
                var template = new CsJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList);
                var content = template.GetContent();
                var filename = template.GetFullName();
                WriteToFile(filename, content);
            }

            // C# RequestJsons
            foreach (var entity in dtos)
            {
                var isReadonly = readonlyEntitiesList.Contains(entity);

                // C# List
                {
                    var template = new CsListRequestJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Create
                if (!isReadonly)
                {
                    var template = new CsCreateRequestJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Read
                {
                    var template = new CsReadRequestJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Update
                if (!isReadonly)
                {
                    var template = new CsUpdateRequestJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Delete
                if (!isReadonly)
                {
                    var template = new CsDeleteRequestJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }
            }

            // C# ResponseJsons
            foreach (var entity in dtos)
            {
                var isReadonly = readonlyEntitiesList.Contains(entity);

                // C# List
                {
                    var template = new CsListResponseJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Create
                if (!isReadonly)
                {
                    var template = new CsCreateResponseJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Read
                {
                    var template = new CsReadResponseJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Update
                if (!isReadonly)
                {
                    var template = new CsUpdateResponseJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }

                // C# Delete
                if (!isReadonly)
                {
                    var template = new CsDeleteResponseJsonTemplate(entity, csharp_shared_directory, csharp_shared_namespace, stateList, uitgebreideKoppelList, simpeleKoppelList, attachmentEntitiesList);
                    var content = template.GetContent();
                    var filename = template.GetFullName();
                    WriteToFile(filename, content);
                }
            }

            // TS Json Interfaces
            foreach (var entity in dtos)
            {
                var template = new TsInterfaceTemplate(entity, angular_app_directory);
                var content = template.GetContent();
                var filename = template.GetFullName();
                WriteToFile(filename, content);
            }

            // TS RequestJsons Interfaces
            foreach (var entity in dtos)
            {
                var isReadonly = readonlyEntitiesList.Contains(entity);

                // C# List

                // C# Create

                // C# Read

                // C# Update

                // C# Delete
            }

            // TS ResponseJsons Interfaces
            foreach (var entity in dtos)
            {
                var isReadonly = readonlyEntitiesList.Contains(entity);

                // C# List

                // C# Create

                // C# Read

                // C# Update

                // C# Delete
            }

            // C# Services

            // C# Controllers

            // TS Services


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
