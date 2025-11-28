using System.ComponentModel.DataAnnotations;

namespace TEC.FullService.Infrastructure.Configuration;

public sealed class DatabaseSettings
{
    public const string SectionName = "ConnectionStrings";

    [Required]
    public string IdentityConnection { get; init; } = string.Empty;

    [Required]
    public string FullServiceConnection { get; init; } = string.Empty;
}
