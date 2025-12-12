namespace TEC.FullService.Shared.DTOs.Companies.Admin;

// Request
public sealed class GetCompanyDetailsRequest
{
    public int Id { get; set; }
}

// Response
public sealed record CompanyDetailsResponse(
    int Id,
    string Name,
    string CvrNumber,
    string ContactEmail,
    string? PhoneNumber,
    string? Address,
    bool IsActive,
    DateTime CreatedAt
);
