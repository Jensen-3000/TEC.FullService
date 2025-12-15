using System.Linq.Expressions;
using TEC.FullService.Api.Domain.Common;

namespace TEC.FullService.Api.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies a global query filter that hides soft-deleted rows
    /// (entities implementing <see cref="ISoftDeletable"/>).
    /// </summary>
    public static void ApplySoftDeleteFilters(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Skip owned/keyless types because query filters don't apply the same way.
            if (entityType.IsOwned() || entityType.IsKeyless)
                continue;

            // Only apply to soft-deletable entities.
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");

            // Uses EF.Property(...) so EF reads the mapped property (works even with explicit interface impl).
            var isDeleted = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                [typeof(bool)],
                parameter,
                Expression.Constant(nameof(ISoftDeletable.IsDeleted)));

            var compare = Expression.Equal(isDeleted, Expression.Constant(false));
            var lambda = Expression.Lambda(compare, parameter);

            // Registers the global filter for this entity type.
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}
