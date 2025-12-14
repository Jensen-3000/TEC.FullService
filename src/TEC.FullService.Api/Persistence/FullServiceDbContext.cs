using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Persistence.Extensions;

namespace TEC.FullService.Api.Persistence;

internal sealed class FullServiceDbContext(DbContextOptions<FullServiceDbContext> options)
    : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Certificate> Certificates => Set<Certificate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FullServiceDbContext).Assembly);
        modelBuilder.ApplySoftDeleteFilters();
    }
}
