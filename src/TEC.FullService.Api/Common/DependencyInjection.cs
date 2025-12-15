using Microsoft.AspNetCore.Identity;
using TEC.FullService.Api.Configuration;
using TEC.FullService.Api.Identity;
using TEC.FullService.Api.Persistence;
using TEC.FullService.Api.Persistence.Interceptors;

namespace TEC.FullService.Api.Common;

internal static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiServices(IConfiguration config)
        {
            AddOptions(services, config);
            AddDbContexts(services, config);
            AddIdentity(services, config);
            AddCrossCutting(services);

            return services;
        }
    }

    private static void AddCrossCutting(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserIdProvider, HttpContextCurrentUserIdProvider>();

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();
    }

    private static void AddOptions(IServiceCollection services, IConfiguration config)
    {
        // Database settings
        services.AddOptions<DatabaseSettings>()
            .Bind(config.GetRequiredSection(nameof(DatabaseSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Identity settings
        services.AddOptions<IdentitySettings>()
            .Bind(config.GetRequiredSection(nameof(IdentitySettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddDbContexts(IServiceCollection services, IConfiguration config)
    {
        var db = config.GetRequiredSection(nameof(DatabaseSettings))
                       .Get<DatabaseSettings>()
                       ?? throw new InvalidOperationException("DatabaseSettings missing.");

        services.AddDbContext<ApplicationIdentityDbContext>(opt =>
            opt.UseSqlServer(db.IdentityConnectionString));

        services.AddDbContext<FullServiceDbContext>((sp, options) =>
        {
            options.UseSqlServer(db.FullServiceConnectionString);
            options.AddInterceptors(
                sp.GetRequiredService<AuditInterceptor>(),
                sp.GetRequiredService<SoftDeleteInterceptor>());
        });
    }

    private static void AddIdentity(IServiceCollection services, IConfiguration config)
    {
        var identity = config.GetRequiredSection(nameof(IdentitySettings))
                             .Get<IdentitySettings>()
                             ?? throw new InvalidOperationException("IdentitySettings missing.");

        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.User.RequireUniqueEmail = identity.RequireUniqueEmail;
            opt.Password.RequiredLength = identity.PasswordRequiredLength;
            opt.Password.RequireLowercase = identity.PasswordRequireLowercase;
            opt.Password.RequireUppercase = identity.PasswordRequireUppercase;
            opt.Password.RequireNonAlphanumeric = identity.PasswordRequireNonAlphanumeric;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
        .AddDefaultTokenProviders();
    }
}
