namespace TEC.FullService.Shared.DTOs.Companies.Admin;

// Request
public sealed class GetCompanyDetailsRequest
{
    public Guid Id { get; set; }
}

// Response
public sealed record CompanyDetailsResponse(
    Guid Id,
    string Name,
    string CvrNumber,
    string ContactEmail,
    string? PhoneNumber,
    string? Address,
    DateTime CreatedAt
);
