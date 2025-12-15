using System.Linq.Expressions;

namespace TEC.FullService.Api.Common;

public static class QueryableExtensions
{
    /// <summary>
    /// Sorts a queryable sequence by a sort enum.
    /// Enum name must match the model property you want to sort by.
    /// </summary>
    public static IQueryable<TEntity> ApplyEnumSort<TEntity, TSortBy>(
        this IQueryable<TEntity> query,
        TSortBy sortBy,
        bool desc)
        where TEntity : class
        where TSortBy : struct, Enum
    {
        // Enum name is used directly as the model property name.
        var propertyName = sortBy.ToString();

        // Build expression: entity => EF.Property<object>(entity, propertyName)
        var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
        var propertyAccess = Expression.Call(
            typeof(EF),
            nameof(EF.Property),
            [typeof(object)],
            entityParameter,
            Expression.Constant(propertyName));

        var keySelector = Expression.Lambda<Func<TEntity, object>>(propertyAccess, entityParameter);

        return desc
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);
    }
}
