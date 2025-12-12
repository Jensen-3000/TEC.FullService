namespace TEC.FullService.Shared.DTOs.Companies.Admin;

// Request
public sealed class PatchCompanyRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? CvrNumber { get; set; }
    public string? ContactEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}

// Response
public sealed record CompanyPatchedResponse(
    Guid Id,
    string Name,
    string CvrNumber,
    string ContactEmail
);
