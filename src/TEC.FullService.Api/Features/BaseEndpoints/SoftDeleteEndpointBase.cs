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

    /// <summary>
    /// Optional validation hook before performing a soft delete.
    /// Override and add errors via <see cref="Endpoint{TRequest}.AddError(string, string)"/>.
    /// Return false to stop execution and send a 400 error response.
    /// </summary>
    protected virtual Task<bool> ValidateDeleteAsync(TRequest req, TEntity entity, CancellationToken ct)
        => Task.FromResult(true);

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        // Find
        var entity = await FindEntity(req, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Validate
        if (!await ValidateDeleteAsync(req, entity, ct))
        {
            await Send.ErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        // Soft delete
        entity.Delete();
        await Db.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
