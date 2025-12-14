using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TEC.FullService.Api.Domain;

namespace TEC.FullService.Api.Persistence.Interceptors;

/// <summary>
/// Enforces the shared-key invariant for UserProfile:
/// UserProfile.Id must equal UserProfile.IdentityUserId (== ApplicationUser.Id).
/// </summary>
public sealed class UserProfileSharedKeyInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        Apply(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void Apply(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries<UserProfile>())
        {
            if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
                continue;

            var profile = entry.Entity;

            if (profile.Id == Guid.Empty && profile.IdentityUserId != Guid.Empty)
                profile.Id = profile.IdentityUserId;
            else if (profile.Id != Guid.Empty && profile.IdentityUserId == Guid.Empty)
                profile.IdentityUserId = profile.Id;
            else if (profile.Id == Guid.Empty && profile.IdentityUserId == Guid.Empty)
                throw new InvalidOperationException(
                    "UserProfile requires Id/IdentityUserId to be set (shared key: UserProfile.Id == ApplicationUser.Id).");
            else if (profile.Id != profile.IdentityUserId)
                throw new InvalidOperationException(
                    "UserProfile.Id must equal IdentityUserId (shared key).");
        }
    }
}
