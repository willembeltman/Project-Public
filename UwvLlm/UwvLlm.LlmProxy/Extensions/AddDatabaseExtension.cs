using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UwvLlm.Infrastructure.Data.Entities;

namespace UwvLlm.LlmProxy.Extensions;
public static class AddDatabaseExtension
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        bool useMemoryDatabase = false)
    {
        var connectionString = configuration.GetConnectionString("uwvllm-db");

        Console.WriteLine(connectionString);

        if (useMemoryDatabase)
        {
            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));

            services.AddDbContextFactory<AuthenticationDbContext<User>>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));
        }
        else
        {
            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sql => sql.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null));
            });

            services.AddDbContextFactory<AuthenticationDbContext<User>>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sql => sql.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null));
            });
        }

        return services;
    }
}