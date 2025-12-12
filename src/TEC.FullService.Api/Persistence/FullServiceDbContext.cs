using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Persistence.Extensions;

namespace TEC.FullService.Api.Persistence;

internal sealed class FullServiceDbContext(DbContextOptions<FullServiceDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FullServiceDbContext).Assembly);
        modelBuilder.ApplySoftDeleteFilters();
    }
}
