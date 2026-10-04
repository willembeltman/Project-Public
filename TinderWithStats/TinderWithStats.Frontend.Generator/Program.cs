using gAPI.CodeGen.Frontend;
using gAPI.CodeGen.Frontend.Models.Configs;
using gAPI.Core.Helpers;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "TinderWithStats");
var config = new FrontendConfig(
    Assemblies: [
        typeof(gAPI.Core.Interfaces.IAccountService).Assembly,
        typeof(gAPI.Core.Client.Razor.FormFile).Assembly,
        typeof(TinderWithStats.Shared.Dtos.State).Assembly,
        typeof(TinderWithStats.Frontend.Layout.MainLayout).Assembly
    ],

    BaseNamespaces: ["TinderWithStats"],

    RootDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend"),
    RootNamespace: "TinderWithStats.Frontend",
    BlazorMauiDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Blazor.Maui"),
    BlazorMauiNamespace: "TinderWithStats.Frontend.Blazor.Maui",
    BlazorWebassemblyDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend"),
    BlazorWebassemblyServiceNamespace: "TinderWithStats.Frontend",

    LayoutDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend\Layout"),
    LayoutNamespace: "TinderWithStats.Frontend.Layout",

    PagesDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend\Pages"),
    PagesNamespace: "TinderWithStats.Frontend.Pages",

    ComponentsDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend\Components"),
    ComponentsNamespace: "TinderWithStats.Frontend.Components",

    UseAutoComponents: true,

    GenerateIsPage: false,
    GenerateComponents: false,

    OverwritePages: true,
    OverwriteComponents: true,
    OverwriteImports: true

    );

var generator = new FrontendGenerator(config);
generator.Run();
