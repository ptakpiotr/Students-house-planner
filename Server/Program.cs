using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Jobs;
using Server.Models;
using Server.Services;
using System.Globalization;
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

            AppDbContext dbContext = ctx.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            AppUser? dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == userInfo["login"].ToString()).ConfigureAwait(false);

            if (dbUser is null)
            {
                await dbContext.Users.AddAsync(new() { Login = userInfo["login"].ToString() }).ConfigureAwait(false);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }

            ctx.Success();
        };
    });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(AppConstants.Authz.AppPolicy, pb =>
    {
        pb.RequireAuthenticatedUser().AddAuthenticationSchemes([AppConstants.Auth.GithubAuthScheme]);
    });
});

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("MainConn"));
});

builder.Services.AddScoped<IInitNextMonthService, InitNextMonthService>();
builder.Services.AddScoped<BackgroundJobExecutor>();

builder.Services.AddHangfire(opts =>
{
    opts.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("MainConn"));
});

builder.Services.AddHangfireServer();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//builder.Services.ConfigureHttpJsonOptions((opts) =>
//{
//    opts.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}

using (var scope = app.Services.CreateScope())
{
    var executor = scope.ServiceProvider.GetRequiredService<BackgroundJobExecutor>();
    executor.RegisterJobs();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

RouteGroupBuilder apiGroup = app.MapGroup("/api");

apiGroup.MapGet("/months", ([FromServices] AppDbContext ctx) =>
{
    var res = ctx.Months.Select(m => new
    {
        Code = $"{m.Year}_{m.Month}",
        Label = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(m.Month)
    }).ToList();

    return Results.Ok(res);
});

apiGroup.MapGet("/month/{key}", async ([FromRoute] string key, [FromServices] AppDbContext ctx) =>
{
    string[] splitValue = key.Split("_");
    int.TryParse(splitValue[0], out int year);
    int.TryParse(splitValue[1], out int month);

    var res = await ctx.Months.Include(x => x.ShoppingEntries).Include(x => x.BathroomEntries).Include(x => x.KitchenEntries).FirstOrDefaultAsync(m => m.Month == month && m.Year == year);
    var dt = JsonSerializer.Serialize(res, new JsonSerializerOptions() { ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles });
    return Results.Ok(JsonSerializer.Deserialize<dynamic>(dt));
});

apiGroup.MapPost("/shopping", async (ShoppingEntryCreate entry, [FromServices] AppDbContext ctx, HttpContext httpContext) =>
{
    string? userLogin = httpContext.User.Claims.FirstOrDefault(c => c.Type == "login")?.Value;

    AppUser? user = await ctx.Users.FirstOrDefaultAsync(u => u.Login == userLogin).ConfigureAwait(false);

    if (userLogin is null || user is null)
    {
        return Results.BadRequest();
    }

    ShoppingEntry shoppingEntry = new()
    {
        Person = user,
        PersonId = user.Id,
        Confirmed = false,
        Item = entry.Item,
        MonthId = entry.MonthId,
        Date = entry.Date
    };

    await ctx.ShoppingEntries.AddAsync(shoppingEntry).ConfigureAwait(false);
    await ctx.SaveChangesAsync().ConfigureAwait(false);

    return Results.NoContent();
}).RequireAuthorization(AppConstants.Authz.AppPolicy);

apiGroup.MapGet("/logout", () =>
{
    return Results.SignOut(new()
    {
        RedirectUri = "/"
    }, [AppConstants.Auth.CookieAuthScheme]);
});

apiGroup.MapGet("/login", () =>
{
    return Results.Challenge(new()
    {
        RedirectUri = "/"
    }, [AppConstants.Auth.GithubAuthScheme]);
});

app.MapHangfireDashboard();
app.MapReverseProxy();

app.Run();