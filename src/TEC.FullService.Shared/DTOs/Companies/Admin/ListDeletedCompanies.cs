using TEC.FullService.Shared.Common.Paging;

namespace TEC.FullService.Shared.DTOs.Companies.Admin;

// Request
public sealed class ListDeletedCompaniesRequest : IHasListQuery
{
    public ListQuery Query { get; set; } = new();
}
