using gAPI.Core.Server.Config;
using gAPI.Core.Server.Dashboard;
using gAPI.Core.Server.Extensions;
using gAPI.Generated;
using System.Globalization;
using TinderWithStats.Backend.Extensions;


internal class Program
{
    private static void Main(string[] args)
    {
        var invariantCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = invariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = invariantCulture;

        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration.CreateServerConfig();

        builder.Services.AddAutoWssServer(config);
        builder.Services.AddAutoAuthServer(config);
        builder.Services.AddStorage(config);
        builder.Services.AddCrudMappings();
        builder.Services.AddCrudUseCases();


        //// Dashboard
        //var nameGetter = new ApplicationNameGetter("TinderWithStats.Backend");
        //builder.Services.AddSingleton(nameGetter);
        //var consoleBuffer = new ConsoleBuffer();
        //builder.Services.AddSingleton(consoleBuffer);
        //if (CanRenderConsole())
        //    builder.Services.AddHostedService<DashboardConsoleOutput>();
        //else
        //    builder.Services.AddHostedService<NormalConsoleOutput>();
        //builder.Services.AddSingleton<IDashboardName>(sp => config);
        //builder.Services.AddSingleton<IDashboardSnapshotGetter, DashboardSnapshotGetter>();

        var app = builder.Build();

        app.MapAutoWssServer();
        app.MapAutoAuthServer();

        app.UseHttpsRedirection();
        app.Run();
    }
    private static bool CanRenderConsole()
    {
        try
        {
            return !Console.IsOutputRedirected && Console.WindowWidth >= 10;
        }
        catch (IOException)
        {
            return false;
        }
    }
}