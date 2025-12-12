using System.Linq.Expressions;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TEC.FullService.Api.Domain.Common;
using TEC.FullService.Shared.Common.Paging;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;

/// <summary>
/// Base for list-endpoints for soft-deleted entities.
/// </summary>
internal abstract class ListDeletedEndpointBase<TRequest, TEntity, TResponse>(
    FullServiceDbContext db)
    : Endpoint<TRequest, PagedResponse<TResponse>>
    where TRequest : class, IHasListQuery
    where TEntity : class, ISoftDeletable
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract IQueryable<TEntity> Query(TRequest req, IQueryable<TEntity> query);
    protected abstract IQueryable<TEntity> ApplySearch(IQueryable<TEntity> q, string search);
    protected abstract Expression<Func<TEntity, TResponse>> Projection { get; }

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        // Query
        var query = Db.Set<TEntity>()
            .IgnoreQueryFilters()
            .Where(e => e.IsDeleted);

        query = Query(req, query);

        // Search
        var search = req.Query.Filter.Search;
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = ApplySearch(query, search);
        }

        // Paging
        var page = req.Query.Paging.Page;
        var pageSize = req.Query.Paging.PageSize;

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(Projection)
            .ToListAsync(ct);

        // Response
        var response = new PagedResponse<TResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        };

        await Send.OkAsync(response, ct);
    }
}
