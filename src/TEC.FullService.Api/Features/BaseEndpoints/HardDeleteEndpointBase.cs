using FastEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for hard deletes.
/// Ignores soft-delete functionality.
/// </summary>
internal abstract class HardDeleteEndpointBase<TRequest, TEntity>(
    FullServiceDbContext db)
    : Endpoint<TRequest>
    where TRequest : notnull
    where TEntity : class
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract Task<TEntity?> FindEntity(TRequest req, CancellationToken ct);

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        // Find
        var entity = await FindEntity(req, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Hard delete
        Db.Set<TEntity>().Remove(entity);
        await Db.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
