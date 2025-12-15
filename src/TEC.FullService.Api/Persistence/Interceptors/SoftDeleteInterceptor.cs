using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TEC.FullService.Api.Common;
using TEC.FullService.Api.Domain.Common;

namespace TEC.FullService.Api.Persistence.Interceptors;

public sealed class SoftDeleteInterceptor(TimeProvider timeProvider, ICurrentUserIdProvider currentUser)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplySoftDelete(DbContext? context)
    {
        if (context is null)
            return;

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var userId = currentUser.UserId;

        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    StampDeleted(entry, now, userId);
                    break;

                case EntityState.Added:
                case EntityState.Modified:
                {
                    var isDeleted = entry.Property(x => x.IsDeleted).CurrentValue;
                    if (isDeleted)
                        StampDeleted(entry, now, userId);

                    break;
                }
            }
        }
    }

    private static void StampDeleted(
        EntityEntry<ISoftDeletable> entry,
        DateTime deletedAt,
        Guid? deletedBy)
    {
        entry.Property(x => x.IsDeleted).CurrentValue = true;

        if (entry.Property(x => x.DeletedAt).CurrentValue is null)
            entry.Property(x => x.DeletedAt).CurrentValue = deletedAt;

        if (entry.Property(x => x.DeletedBy).CurrentValue is null && deletedBy is not null)
            entry.Property(x => x.DeletedBy).CurrentValue = deletedBy;
    }
}
