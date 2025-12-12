namespace TEC.FullService.Shared.Common.Paging;

public sealed class ListQuery
{
    public PagingOptions Paging { get; set; } = new();
    public FilterOptions Filter { get; set; } = new();
}
