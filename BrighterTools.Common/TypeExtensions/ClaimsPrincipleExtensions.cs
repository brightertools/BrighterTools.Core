using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using App.Data;
using App.Security.Auth;
using App.Security.MultiTenancy;

namespace App.TypeExtensions;

public static class ClaimsPrincipleExtensions
{
    public static AuthenticatedUser? ApplicationUser(this ClaimsPrincipal user)
    {
        var authenticatedUser = new AuthenticatedUser();

        if (user == null || (!user?.Identity?.IsAuthenticated ?? false))
        {
            return null;
        }

        int.TryParse(user?.FindFirstValue("id"), out int userId);
        authenticatedUser.Id = userId;

        Guid.TryParse(user?.FindFirstValue("guid"), out Guid userGuid);
        authenticatedUser.Guid = userGuid;

        var currentTenantJson = user?.FindFirstValue("currentTenant");
        authenticatedUser.CurrentTenant = JsonSerializer.Deserialize<TenantMetadata>(currentTenantJson!)!;

        var email = user?.FindFirstValue("email");
        authenticatedUser.Email = email ?? "";

        var firstName = user?.FindFirstValue("firstName");
        var lastName = user?.FindFirstValue("lastName");
        authenticatedUser.FullName = $"{firstName} {lastName}".Trim();

        var currentLoginProvider = user?.FindFirstValue("currentLoginProvider")!;
        authenticatedUser.CurrentLoginProvider = (LoginProviderType)Enum.Parse(typeof(LoginProviderType), currentLoginProvider);

        return authenticatedUser;
    }
}