namespace TEC.FullService.Api.Domain;

/// <summary>
/// Repræsenterer et kursus i systemet. Data kan synkroniseres fra et eksternt API
/// eller oprettes manuelt af en administrator.
/// </summary>
public class Course
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string CourseCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Provider { get; set; } = null!;
    public decimal StandardPrice { get; set; }
    public decimal FullPrice { get; set; }
    public string? CourseType { get; set; }
    public decimal? DurationInDays { get; set; }
    public bool IsCertificateCourse { get; set; }
    public int DefaultValidityMonths { get; set; }
    public bool IsManuallyCreated { get; set; }

    // Navigation property for fremtidig relation til Enrollments
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
