using TEC.FullService.Shared.Common;

namespace TEC.FullService.Api.Domain;

/// <summary>
/// Repræsenterer en brugers tilmelding til et specifikt kursus.
/// Fungerer som den centrale "transaktions"-entitet i systemet.
/// </summary>
public class Enrollment
{
    public int Id { get; set; }
    public decimal PricePaid { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime CourseStartDate { get; set; }
    public DateTime CourseEndDate { get; set; }
    public EnrollmentStatus Status { get; set; }
    public FundApplicationStatus FundApplicationStatus { get; set; } = FundApplicationStatus.PendingHandling;
    public string? FundApplicationNotes { get; set; }

    // Fremmednøgler
    public Guid UserId { get; set; }
    public int CourseId { get; set; }
    public int? FundId { get; set; } // Nullable
    public Guid? ApprovedByUserId { get; set; } // Nullable

    // Navigation properties
    public UserProfile User { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public CompetenceFund? Fund { get; set; }
    public UserProfile? ApprovedByUser { get; set; }
    public Certificate? Certificate { get; set; }
}
