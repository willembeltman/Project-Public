using gAPI.CodeGen.Backend.Models.Config;
using gAPI.Core.Helpers;
using UwvLlm.Infrastructure.Data.Entities;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "UwvLlm");
var config = new BackendConfig(
    DbContextType: typeof(ApplicationDbContext),

    Shared_DtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Dtos"),
    Shared_DtosNamespace: "UwvLlm.Shared.Dtos",
    Shared_StateDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Dtos"),
    Shared_StateDtosNamespace: "UwvLlm.Shared.Dtos",
    Shared_CrudInterfacesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\CrudInterfaces"),
    Shared_CrudInterfacesNamespace: "UwvLlm.Shared.CrudInterfaces",

    Core_CrudUseCasesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Infrastructure.Data\UseCases"),
    Core_CrudUseCasesNamespace: "UwvLlm.Infrastructure.Data.UseCases",
    Core_CrudServicesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Infrastructure.Data\CrudServices"),
    Core_CrudServicesNamespace: "UwvLlm.Infrastructure.Data.CrudServices",
    Core_CrudMappingsDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Infrastructure.Data\Mappings"),
    Core_CrudMappingsNamespace: "UwvLlm.Infrastructure.Data.Mappings",

    Extensions_Directory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Api\Extensions"),
    Extensions_Namespace: "UwvLlm.Api.Extensions"
    );

var generator = new gAPI.CodeGen.Backend.BackendGenerator(config);
generator.Run();