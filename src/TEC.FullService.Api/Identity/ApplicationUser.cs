using Microsoft.AspNetCore.Identity;

namespace TEC.FullService.Api.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser() => Id = Guid.CreateVersion7();
}
