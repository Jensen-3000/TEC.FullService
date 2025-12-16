using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TEC.FullService.Api.Domain;

namespace TEC.FullService.Api.Persistence.Configurations;

internal sealed class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.CertificateName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(c => c.IssueDate)
            .IsRequired();

        builder.Property(c => c.ExpiryDate)
            .IsRequired();

        // Foreign keys
        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.CourseId)
            .IsRequired();

        builder.Property(c => c.EnrollmentId)
            .IsRequired(false);

        builder.Property(c => c.ReplacesId)
            .IsRequired(false);

        builder.Property(c => c.CompetenceFundId)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(c => c.EnrollmentId)
            .IsUnique()
            .HasFilter("[EnrollmentId] IS NOT NULL"); // Kun unik hvis ikke null

        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.CourseId);
        builder.HasIndex(c => c.Status);

        // Relationships

        // Certificate -> User (many-to-one)
        builder.HasOne(c => c.User)
            .WithMany(u => u.Certificates)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Certificate -> Course (many-to-one)
        builder.HasOne(c => c.Course)
            .WithMany()
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Certificate -> Certificate (self-referencing for renewals)
        builder.HasOne(c => c.ReplacedCertificate)
            .WithMany()
            .HasForeignKey(c => c.ReplacesId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // Certificate -> CompetenceFund (many-to-one, optional)
        builder.HasOne(c => c.CompetenceFund)
            .WithMany()
            .HasForeignKey(c => c.CompetenceFundId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
