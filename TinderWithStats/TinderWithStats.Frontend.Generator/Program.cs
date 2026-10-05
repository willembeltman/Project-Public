using gAPI.CodeGen.Frontend;
using gAPI.CodeGen.Frontend.Models.Configs;
using gAPI.Core.Helpers;

var root = EnvironmentPathHelper.GetRoot(Environment.ProcessPath!, "TinderWithStats");
var config = new FrontendConfig(
    Assemblies: [
        typeof(gAPI.Core.Interfaces.IAccountService).Assembly,
        typeof(gAPI.Core.Client.Razor.FormFile).Assembly,
        typeof(TinderWithStats.Shared.Dtos.State).Assembly,
        typeof(TinderWithStats.Frontend.Razor.Layout.MainLayout).Assembly
    ],

    BaseNamespaces: ["TinderWithStats"],

    RootDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Razor"),
    RootNamespace: "TinderWithStats.Frontend.Razor",
    BlazorMauiDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Maui"),
    BlazorMauiNamespace: "TinderWithStats.Frontend.Razor.Blazor.Maui",
    BlazorWebassemblyDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Webassembly"),
    BlazorWebassemblyServiceNamespace: "TinderWithStats.Frontend.Razor",

    LayoutDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Razor\Layout"),
    LayoutNamespace: "TinderWithStats.Frontend.Razor.Layout",

    PagesDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Razor\Pages"),
    PagesNamespace: "TinderWithStats.Frontend.Razor.Pages",

    ComponentsDirectory: EnvironmentPathHelper.GetDirectory(root, @"TinderWithStats.Frontend.Razor\Components"),
    ComponentsNamespace: "TinderWithStats.Frontend.Razor.Components",

    UseAutoComponents: false,

    GenerateIsPage: true,
    GenerateComponents: true,

    OverwritePages: true,
    OverwriteComponents: true,
    OverwriteImports: false

    );

var generator = new FrontendGenerator(config);
generator.Run();
