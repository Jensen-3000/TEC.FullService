using TEC.FullService.Api.Identity;
using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Domain;

public sealed class CompetenceFundApplication
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateOnly RegistrationDate { get; set; }
    public FundApplicationStatus FundApplicationStatus { get; set; }

    // Foreign keys
    public Guid UserId { get; set; }
    public Guid CompetenceFundId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid CourseId { get; set; }

    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public CompetenceFund CompetenceFund { get; set; } = null!;
    public Company? Company { get; set; }
    public Course Course { get; set; } = null!;
}
