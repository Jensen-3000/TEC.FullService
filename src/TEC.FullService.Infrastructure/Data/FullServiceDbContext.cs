using Microsoft.EntityFrameworkCore;

namespace TEC.FullService.Infrastructure.Data;

public sealed class FullServiceDbContext(DbContextOptions<FullServiceDbContext> options)
    : DbContext(options)
{

};
