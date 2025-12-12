using FastEndpoints;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for create endpoints.
/// </summary>
internal abstract class CreateEndpointBase<TRequest, TEntity, TResponse>(
    FullServiceDbContext db)
    : Endpoint<TRequest, TResponse>
    where TRequest : notnull
    where TEntity : class
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract TEntity CreateEntity(TRequest req);
    protected abstract TResponse MapToResponse(TEntity entity);

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        // Create
        var entity = CreateEntity(req);

        // Save
        Db.Add(entity);
        await Db.SaveChangesAsync(ct);

        var response = MapToResponse(entity);

        await Send.ResponseAsync(
            response,
            statusCode: 201,
            cancellation: ct
        );
    }
}
