namespace Server;

internal static class AppConstants
{
    internal const string LocalUrl = "https://localhost:7072";

    internal static class Auth
    {
        internal const string CookieAuthScheme = "CookieScheme";
        internal const string GithubAuthScheme = "GithubScheme";
        internal static readonly string[] UserInfo = ["login", "avatar_url"];
    }

    internal static class Authz
    {
        internal const string AppPolicy = "AppPolicy";
    }
}