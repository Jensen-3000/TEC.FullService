using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TEC.FullService.Infrastructure.Configuration;
using TEC.FullService.Infrastructure.Data;
using TEC.FullService.Infrastructure.Identity;

namespace TEC.FullService.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            var provider = services.BuildServiceProvider();
            var db = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            ConfigureDatabaseContexts(services, db);
            ConfigureIdentitySystem(services);

            return services;
        }
    }

    private static void ConfigureDatabaseContexts(IServiceCollection services, DatabaseSettings db)
    {
        services.AddDbContext<IdentityDbContext>(o =>
            o.UseSqlServer(db.IdentityConnection));

        services.AddDbContext<FullServiceDbContext>(o =>
            o.UseSqlServer(db.FullServiceConnection));
    }

    private static void ConfigureIdentitySystem(IServiceCollection services) =>
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();
}
