using FastEndpoints;
using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for soft deletes.
/// </summary>
internal abstract class SoftDeleteEndpointBase<TRequest, TEntity>(
    FullServiceDbContext db)
    : Endpoint<TRequest>
    where TRequest : notnull
    where TEntity : class, ISoftDeletable
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

        // Soft delete
        entity.Delete();
        await Db.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
