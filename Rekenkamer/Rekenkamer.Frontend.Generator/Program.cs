using gAPI.CodeGen.Frontend;
using gAPI.CodeGen.Frontend.Models.Configs;
using gAPI.Core.Helpers;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "Rekenkamer");
var config = new FrontendConfig(
    Assemblies: [
        typeof(gAPI.Core.Interfaces.IAccountService).Assembly,
        typeof(gAPI.Core.Client.Razor.FormFile).Assembly,
        typeof(Rekenkamer.Shared.Dtos.State).Assembly,
        typeof(Rekenkamer.Frontend.Layout.MainLayout).Assembly
    ],

    BaseNamespaces: ["Rekenkamer"],

    RootDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Frontend"),
    RootNamespace: "Rekenkamer.Frontend",
    BlazorMauiDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.App"),
    BlazorMauiNamespace: "Rekenkamer.App",
    BlazorWebassemblyDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Frontend"),
    BlazorWebassemblyServiceNamespace: "Rekenkamer.Frontend",

    LayoutDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Frontend\Layout"),
    LayoutNamespace: "Rekenkamer.Frontend.Layout",

    PagesDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Frontend\Pages"),
    PagesNamespace: "Rekenkamer.Frontend.Pages",

    ComponentsDirectory: EnvironmentPathHelper.GetDirectory(root, @"Rekenkamer.Frontend\Components"),
    ComponentsNamespace: "Rekenkamer.Frontend.Components",

    UseAutoComponents: true,

    GenerateIsPage: false,
    GenerateComponents: false,
    GenerateNavigation: false,

    OverwritePages: true,
    OverwriteComponents: true,
    OverwriteImports: false

    );

var generator = new FrontendGenerator(config);
generator.Run();
