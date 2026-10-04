using TinderWithStats.Backend.Entities;
using gAPI.CodeGen.Backend.Models.Config;
using gAPI.Core.Helpers;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "TinderWithStats");
var config = new BackendConfig(
    DbContextType: typeof(ApplicationDbContext),

    Shared_DtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Shared\Dtos"),
    Shared_DtosNamespace: "TinderWithStats.Shared.Dtos",
    Shared_StateDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Shared\Dtos"),
    Shared_StateDtosNamespace: "TinderWithStats.Shared.Dtos",
    Shared_CrudInterfacesDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Shared\Interfaces"),
    Shared_CrudInterfacesNamespace: "TinderWithStats.Shared.Interfaces",
    Shared_CrudInterfacesEnd: "Api",

    Core_CrudUseCasesDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Backend\UseCases"),
    Core_CrudUseCasesNamespace: "TinderWithStats.Backend.UseCases",
    Core_CrudMappingsDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Backend\Mappings"),
    Core_CrudMappingsNamespace: "TinderWithStats.Backend.Mappings",
    Core_CrudServicesDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Backend\Services"),
    Core_CrudServicesNamespace: "TinderWithStats.Backend.Services",
    Core_CrudServicesEnd: "Api",

    Extensions_Directory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Api\Extensions"),
    Extensions_Namespace: "TinderWithStats.Api.Extensions",
    OverwriteServices: true,
    OverwriteServiceInterfaces: true,
    OverwriteMappers: true,
    OverwriteUseCases: false
    );

var generator = new gAPI.CodeGen.Backend.BackendGenerator(config);
generator.Run();