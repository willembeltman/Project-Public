using Bsd.Infrastructure.Data.Entities;
using gAPI.CodeGen.Backend.Models.Config;
using gAPI.Core.Helpers;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "BeltmanSoftwareDesign");
var config = new BackendConfig(
    DbContextType: typeof(ApplicationDbContext),

    Shared_DtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Public.Shared\Dtos"),
    Shared_DtosNamespace: "Bsd.Public.Shared.Dtos",
    Shared_StateDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Public.Shared\Dtos"),
    Shared_StateDtosNamespace: "Bsd.Public.Shared.Dtos",
    Shared_CrudInterfacesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Public.Shared\Interfaces"),
    Shared_CrudInterfacesNamespace: "Bsd.Public.Shared.Interfaces",
    Shared_CrudInterfacesEnd: "Api",

    Core_CrudUseCasesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Infrastructure.Database\UseCases"),
    Core_CrudUseCasesNamespace: "Bsd.Infrastructure.Database.UseCases",
    Core_CrudMappingsDirectory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Infrastructure.Database\Mappings"),
    Core_CrudMappingsNamespace: "Bsd.Infrastructure.Database.Mappings",
    Core_CrudServicesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Infrastructure.Database\Services"),
    Core_CrudServicesNamespace: "Bsd.Infrastructure.Database.Services",
    Core_CrudServicesEnd: "Api",

    Extensions_Directory: EnvironmentPathHelper.GetDirectory(root, @"Bsd.Public.Api\Extensions"),
    Extensions_Namespace: "Bsd.Public.Api.Extensions",
    OverwriteServices: true,
    OverwriteServiceInterfaces: true,
    OverwriteMappers: true,
    OverwriteUseCases: false
    );

var generator = new gAPI.CodeGen.Backend.BackendGenerator(config);
generator.Run();