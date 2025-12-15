using System.Linq.Expressions;
using FastEndpoints;
using TEC.FullService.Api.Common;
using TEC.FullService.Shared.Common.Paging;
using TEC.FullService.Api.Persistence;

namespace TEC.FullService.Api.Features.BaseEndpoints;
/// <summary>
/// Base for list-endpoints with sorting and search.
/// </summary>
internal abstract class ListEndpointBase<TRequest, TEntity, TResponse, TSortBy>(
    FullServiceDbContext db)
    : Endpoint<TRequest, PagedResponse<TResponse>>
    where TRequest : class, IHasListQuery, ISortRequest<TSortBy>
    where TEntity : class
    where TSortBy : struct, Enum
{
    protected readonly FullServiceDbContext Db = db;

    protected abstract IQueryable<TEntity> Query(TRequest req, IQueryable<TEntity> query);
    protected virtual IQueryable<TEntity> ApplySearch(IQueryable<TEntity> query, string search) => query;
    protected abstract Expression<Func<TEntity, TResponse>> Projection { get; }

    // Defaults to first enum value / "Id"
    protected virtual TSortBy DefaultSort => default;

    // Defaults to false = ascending
    protected virtual bool DefaultDesc => false;

    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        IQueryable<TEntity> query = Db.Set<TEntity>();

        // Query
        query = Query(req, query);

        // Search
        var search = req.Query.Filter.Search;
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = ApplySearch(query, search);
        }

        // Sort
        var sortBy = req.SortBy ?? DefaultSort;
        var desc = req.Desc ?? DefaultDesc;

        query = query.ApplyEnumSort(sortBy, desc);

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
