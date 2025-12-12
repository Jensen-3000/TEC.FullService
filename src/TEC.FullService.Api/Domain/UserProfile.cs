using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Api.Identity;

namespace TEC.FullService.Api.Domain;

public sealed class UserProfile : SoftDeletableEntity
{
    public Guid Id { get; set; }
    public Guid IdentityUserId { get; set; }

    public ApplicationUser? IdentityUser { get; set; }

    public string DisplayName { get; set; } = "";
    public Guid CompanyId { get; set; }
}
