using FastEndpoints;
using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for restore of soft-deleted entities.
/// </summary>
internal abstract class RestoreEndpointBase<TEntity>(FullServiceDbContext db)
    : EndpointWithoutRequest
    where TEntity : class, ISoftDeletable
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract Task<TEntity?> FindEntity(Guid id, CancellationToken ct);

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Find
        var id = Route<Guid>("id");
        var entity = await FindEntity(id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Restore
        entity.Restore();
        await Db.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
