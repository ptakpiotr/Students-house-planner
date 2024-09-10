using System.Security.Claims;

namespace Server.Helpers;

internal static class UserHelpers
{
    internal static void ApplyClaimsToExistingUser(this ClaimsIdentity? appUser, Dictionary<string, object>? userInfo)
    {
        ArgumentNullException.ThrowIfNull(appUser);
        ArgumentNullException.ThrowIfNull(userInfo);

        foreach (string i in AppConstants.Auth.UserInfo)
        {
            if (userInfo.ContainsKey(i))
            {
                appUser.AddClaim(new(i, userInfo[i]?.ToString()!));
            }
        }
    }
}