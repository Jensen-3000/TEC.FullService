using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TEC.FullService.Api.Persistence.Interceptors;

/// <summary>
/// Ensures Guid primary keys are set using Guid v7 on insert.
/// </summary>
public sealed class GuidV7Interceptor : SaveChangesInterceptor
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

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State != EntityState.Added)
                continue;

            // Generic Guid primary key convention: property named "Id" of type Guid.
            var idProperty = entry.Metadata.FindProperty("Id");
            if (idProperty is null || idProperty.ClrType != typeof(Guid))
                continue;

            var current = (Guid)(entry.Property("Id").CurrentValue ?? Guid.Empty);
            if (current == Guid.Empty)
                entry.Property("Id").CurrentValue = Guid.CreateVersion7();
        }
    }
}
