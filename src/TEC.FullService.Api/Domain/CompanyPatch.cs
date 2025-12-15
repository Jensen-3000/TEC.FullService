namespace TEC.FullService.Api.Domain;

public sealed record CompanyPatch(
    string? Name = null,
    string? CvrNumber = null,
    string? ContactEmail = null,
    string? PhoneNumber = null,
    string? Address = null);
