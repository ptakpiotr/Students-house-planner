using Server.Helpers;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddCookie(AppConstants.Auth.CookieAuthScheme)
    .AddOAuth(AppConstants.Auth.GithubAuthScheme, opts =>
    {
        opts.SignInScheme = AppConstants.Auth.CookieAuthScheme;
        opts.ClientId = builder.Configuration.GetValue<string>("Auth:ClientId") ?? string.Empty;
        opts.ClientSecret = builder.Configuration.GetValue<string>("Auth:ClientSecret") ?? string.Empty;
        opts.TokenEndpoint = builder.Configuration.GetValue<string>("Auth:TokenEndpoint") ?? string.Empty;
        opts.AuthorizationEndpoint = builder.Configuration.GetValue<string>("Auth:AuthorizationEndpoint") ?? string.Empty;
        opts.SaveTokens = true;
        opts.CallbackPath = "/cb/github";
        opts.AccessDeniedPath = "/api/login";

        opts.Events.OnCreatingTicket = async (ctx) =>
        {
            string? token = ctx.AccessToken;
            HttpRequestMessage msg = new()
            {
                RequestUri = new Uri(builder.Configuration.GetValue<string>("Auth:UserEndpoint") ?? string.Empty)
            };
            msg.Headers.Add("Authorization", $"Bearer {token}");
            var resp = await ctx.Backchannel.SendAsync(msg);
            string val = await resp.Content.ReadAsStringAsync();

            Dictionary<string, object>? userInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(val);

            ClaimsIdentity? appUser = ctx.Principal?.Identities.FirstOrDefault();

            appUser.ApplyClaimsToExistingUser(userInfo);
        };
    });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(AppConstants.Authz.AppPolicy, pb =>
    {
        pb.RequireAuthenticatedUser().AddAuthenticationSchemes([AppConstants.Auth.GithubAuthScheme]);
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/api", (HttpContext ctx) =>
{
    return Results.Ok(ctx.User);
});

app.MapGet("/api/logout", () =>
{
    return Results.SignOut(new()
    {
        RedirectUri = "/"
    }, [AppConstants.Auth.CookieAuthScheme]);
});

app.MapGet("/api/login", () =>
{
    return Results.Challenge(new()
    {
        RedirectUri = "/"
    }, [AppConstants.Auth.GithubAuthScheme]);
});

app.MapReverseProxy();

app.Run();