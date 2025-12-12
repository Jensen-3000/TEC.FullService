using TEC.FullService.Shared.Common.Paging;

namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

public enum CertificateSortBy
{
    Id,
    IssueDate,
    ExpiryDate,
    Status
}

// Request
public sealed class ListCertificatesRequest : IHasListQuery, ISortRequest<CertificateSortBy>
{
    public ListQuery Query { get; set; } = new();
    public CertificateSortBy? SortBy { get; set; }
    public bool? Desc { get; set; }
}

// Response
public sealed record CertificateListResponse(
    Guid Id,
    string CertificateName,
    DateTime IssueDate,
    DateTime ExpiryDate,
    string Status,
    string UserName,
    string? CompanyName,
    string CourseName
);
