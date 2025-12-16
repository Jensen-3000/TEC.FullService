using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Identity;

namespace TEC.FullService.Api.Persistence;

internal sealed class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
}
