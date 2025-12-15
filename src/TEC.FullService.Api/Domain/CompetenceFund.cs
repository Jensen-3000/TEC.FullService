namespace TEC.FullService.Api.Domain;

/// <summary>
/// Repræsenterer en kompetencefond, som der kan søges tilskud fra.
/// Administratorer kan vedligeholde denne liste.
/// </summary>
public class CompetenceFund
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? WebsiteUrl { get; set; }
    //public bool IsActive { get; set; } = true;
}
