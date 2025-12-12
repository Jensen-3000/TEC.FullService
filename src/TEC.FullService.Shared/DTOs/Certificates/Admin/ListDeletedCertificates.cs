using TEC.FullService.Shared.Common.Paging;

namespace TEC.FullService.Shared.DTOs.Certificates.Admin;

// Request
public sealed class ListDeletedCertificatesRequest : IHasListQuery
{
    public ListQuery Query { get; set; } = new();
}
