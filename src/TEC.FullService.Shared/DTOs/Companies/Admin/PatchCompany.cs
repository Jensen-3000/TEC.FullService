namespace TEC.FullService.Shared.DTOs.Companies.Admin;

// Request
public sealed class PatchCompanyRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? CvrNumber { get; set; }
    public string? ContactEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}

// Response
public sealed record CompanyPatchedResponse(
    int Id,
    string Name,
    string CvrNumber,
    string ContactEmail,
    bool IsActive
);
