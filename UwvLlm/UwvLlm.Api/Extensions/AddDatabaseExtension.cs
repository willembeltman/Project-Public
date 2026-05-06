using gAPI.Core.Server.Entities;
using Microsoft.EntityFrameworkCore;
using UwvLlm.Infrastructure.Data.Entities;

namespace UwvLlm.Core.Extensions;

public static class AddDatabaseExtension
{
    public static IServiceCollection AddDatabase(
    this IServiceCollection services,
    IConfiguration configuration,
    bool useMemoryDatabase = false)
    {
        var connectionString = configuration.GetConnectionString("uwvllm-db");

        // ✅ Factory voor ApplicationDbContext
        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            if (useMemoryDatabase)
            {
                options.UseInMemoryDatabase("InMemoryDb");
            }
            else
            {
                options.UseSqlServer(
                    connectionString,
                    sql => sql.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null));
            }
        });

        // ✅ Normale ApplicationDbContext
        services.AddScoped<ApplicationDbContext>(sp =>
            sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
              .CreateDbContext());

        // ✅ Normale AuthenticationDbContext<User>
        services.AddScoped<AuthenticationDbContext<User>>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        // ✅ Factory voor AuthenticationDbContext<User>
        services.AddScoped<IDbContextFactory<AuthenticationDbContext<User>>>(sp =>
        {
            var factory = sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();

            return new DelegatingDbContextFactory<AuthenticationDbContext<User>>(
                () => factory.CreateDbContext());
        });

        return services;
    }
}
public class DelegatingDbContextFactory<TContext> : IDbContextFactory<TContext>
    where TContext : DbContext
{
    private readonly Func<TContext> _factory;

    public DelegatingDbContextFactory(Func<TContext> factory)
    {
        _factory = factory;
    }

    public TContext CreateDbContext()
        => _factory();
}