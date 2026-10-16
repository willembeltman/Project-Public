using gAPI.CodeGen.Backend.Models.Config;
using gAPI.Core.Helpers;
using Rekenkamer.Infrastructure.Data.Entities;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "Rekenkamer");
var config = new BackendConfig(
    DbContextType: typeof(ApplicationDbContext),

    Shared_DtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Shared\Dtos"),
    Shared_DtosNamespace: "Rekenkamer.Shared.Dtos",
    Shared_StateDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Shared\Dtos"),
    Shared_StateDtosNamespace: "Rekenkamer.Shared.Dtos",
    Shared_CrudInterfacesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Shared\Interfaces"),
    Shared_CrudInterfacesNamespace: "Rekenkamer.Shared.Interfaces",
    Shared_CrudInterfacesEnd: "Api",

    Core_CrudUseCasesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Infrastructure.Data\UseCases"),
    Core_CrudUseCasesNamespace: "Rekenkamer.Infrastructure.Data.UseCases",
    Core_CrudMappingsDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Infrastructure.Data\Mappings"),
    Core_CrudMappingsNamespace: "Rekenkamer.Infrastructure.Data.Mappings",
    Core_CrudServicesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Infrastructure.Data\Services"),
    Core_CrudServicesNamespace: "Rekenkamer.Infrastructure.Data.Services",
    Core_CrudServicesEnd: "Api",

    Extensions_Directory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Infrastructure.Data\Extensions"),
    Extensions_Namespace: "Rekenkamer.Infrastructure.Data.Extensions",
    OverwriteServices: true,
    OverwriteServiceInterfaces: true,
    OverwriteMappers: true,
    OverwriteUseCases: false
    );

var generator = new gAPI.CodeGen.Backend.BackendGenerator(config);
generator.Run();