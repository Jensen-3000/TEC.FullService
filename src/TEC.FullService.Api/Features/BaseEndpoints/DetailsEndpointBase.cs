using System.Linq.Expressions;
using FastEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for details endpoints.
/// </summary>
internal abstract class DetailsEndpointBase<TRequest, TEntity, TResponse>(
    FullServiceDbContext db)
    : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TEntity : class
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract IQueryable<TEntity> Query(TRequest req);
    protected abstract Expression<Func<TEntity, TResponse>> Projection { get; }

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        // Query
        var projected = await Query(req)
            .Select(Projection)
            .FirstOrDefaultAsync(ct);

        // Check if not found
        if (projected is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(projected, ct);
    }
}
