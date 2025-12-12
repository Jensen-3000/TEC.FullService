using System.Linq.Expressions;
using TEC.FullService.Api.Domain;
using TEC.FullService.Api.Features.Companies.Admin;

namespace TEC.FullService.Api.Features.Companies.Mappings;

/// <summary>
/// Expression-based mappings for Company projections.
/// These projections are compiled and can be used directly in EF Core queries.
/// </summary>
internal static class CompanyMappings
{
    /// <summary>
    /// Projection for list response - lightweight for paginated lists.
    /// </summary>
    internal static readonly Expression<Func<Company, CompanyListResponse>> ToListResponse =
        c => new CompanyListResponse(
            c.Id,
            c.Name,
            c.CvrNumber,
            c.ContactEmail,
            c.PhoneNumber,
            c.Address,
            c.CreatedAt);

    /// <summary>
    /// Projection for detailed response - includes all company information.
    /// </summary>
    internal static readonly Expression<Func<Company, CompanyDetailsResponse>> ToDetailsResponse =
        c => new CompanyDetailsResponse(
            c.Id,
            c.Name,
            c.CvrNumber,
            c.ContactEmail,
            c.PhoneNumber,
            c.Address,
            c.CreatedAt);
}
