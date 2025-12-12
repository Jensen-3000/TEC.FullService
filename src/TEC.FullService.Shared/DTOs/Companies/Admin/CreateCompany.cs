namespace TEC.FullService.Shared.DTOs.Companies.Admin;

// Request
public sealed class CreateCompanyRequest
{
    public required string Name { get; set; }
    public required string CvrNumber { get; set; }
    public required string ContactEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}

// Response
public sealed record CompanyCreatedResponse(
    Guid Id,
    string Name,
    string CvrNumber,
    string ContactEmail
);
