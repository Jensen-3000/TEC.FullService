using System.Security.Claims;

namespace TEC.FullService.Api.Common;

public interface ICurrentUserIdProvider
{
    Guid? UserId { get; }
}

internal sealed class HttpContextCurrentUserIdProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserIdProvider
{
    public Guid? UserId => TryGetUserId(httpContextAccessor.HttpContext?.User);

    private static Guid? TryGetUserId(ClaimsPrincipal? user)
    {
        if (user?.Identity?.IsAuthenticated is not true)
            return null;

        var raw = user.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? user.FindFirstValue("sub");

        return Guid.TryParse(raw, out var id)
            ? id
            : null;
    }
}
