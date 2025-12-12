using TEC.FullService.Shared.Common.Paging;

namespace TEC.FullService.Shared.DTOs.Companies.Admin;

public enum CompanySortBy
{
    Id,
    Name,
    CvrNumber,
    ContactEmail,
    CreatedAt
}

// Request
public sealed class ListCompaniesRequest : IHasListQuery, ISortRequest<CompanySortBy>
{
    public ListQuery Query { get; set; } = new();

    public CompanySortBy? SortBy { get; set; }

    public bool? Desc { get; set; }

    public bool? ActiveOnly { get; set; }
}

// Response
public sealed record CompanyListResponse(
    int Id,
    string Name,
    string CvrNumber,
    string ContactEmail,
    string? PhoneNumber,
    string? Address,
    bool IsActive,
    DateTime CreatedAt
);
