using gAPI.Core.Server;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Api.Core.Infrastructure.Data;
using gAPI.Core.Server.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace UwvLlm.Llm.Extensions;

public static class AddDatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, ServerConfig config)
    {
        return AddDatabase(services, config.DefaultConnectionString, config.UseMemoryDatabase);
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString, bool useMemoryDatabase = false)
    {
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
                    sql => sql.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    )
                );
            });
            services.AddDbContextFactory<AuthenticationDbContext<User>>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    sql => sql.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    )
                );
            });
        }
        return services;
    }
}
