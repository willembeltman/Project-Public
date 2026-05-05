using gAPI.CodeGen.Backend.Models.Config;
using gAPI.Core.Helpers;
using UwvLlm.Api.Core.Infrastructure.Data;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "UwvLlm");
var config = new BackendConfig(
    DbContextType: typeof(ApplicationDbContext),

    Data_AuthenticationDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Infrastructure.Data\Authentication"),
    Data_AuthenticationNamespace: "UwvLlm.Infrastructure.Data.Authentication",

    Shared_DtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Dtos"),
    Shared_DtosNamespace: "UwvLlm.Shared.Dtos",
    Shared_StateDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Dtos"),
    Shared_StateDtosNamespace: "UwvLlm.Shared.Dtos",
    Shared_InterfacesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Interfaces"),
    Shared_InterfacesNamespace: "UwvLlm.Shared.Interfaces",
    Shared_RequestDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Dtos"),
    Shared_RequestDtosNamespace: "UwvLlm.Shared.Dtos",
    Shared_ResponseDtosDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Shared\Dtos"),
    Shared_ResponseDtosNamespace: "UwvLlm.Shared.Dtos",

    Core_CrudUseCasesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Api.Core\UseCases"),
    Core_CrudUseCasesNamespace: "UwvLlm.Api.Core.UseCases",
    Core_CrudServicesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Api.Core\CrudServices"),
    Core_CrudServicesNamespace: "UwvLlm.Api.Core.CrudServices",

    Core_CrudMappingsDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Api.Core\Mappings"),
    Core_CrudMappingsNamespace: "UwvLlm.Api.Core.Mappings",
    Core_ServicesDirectory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Api.Core\Services"),
    Core_ServicesNamespace: "UwvLlm.Api.Core.Services",
    Extensions_Directory: EnvironmentPathHelper.GetDirectory(root, @"UwvLlm.Api\Extensions"),
    Extensions_Namespace: "UwvLlm.Api.Extensions");

var generator = new gAPI.CodeGen.Backend.BackendGenerator(config);
generator.Run();