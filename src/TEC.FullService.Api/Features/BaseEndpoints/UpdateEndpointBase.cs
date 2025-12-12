using FastEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for full-update endpoints.
/// </summary>
internal abstract class UpdateEndpointBase<TRequest, TEntity, TResponse>(
    FullServiceDbContext db)
    : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TEntity : class
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract Task<TEntity?> FindEntity(TRequest req, CancellationToken ct);
    protected abstract void UpdateEntity(TEntity entity, TRequest req);
    protected abstract TResponse MapToResponse(TEntity entity);

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        // Find
        var entity = await FindEntity(req, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Update
        UpdateEntity(entity, req);
        await Db.SaveChangesAsync(ct);

        var response = MapToResponse(entity);
        
        await Send.OkAsync(response, ct);
    }
}
