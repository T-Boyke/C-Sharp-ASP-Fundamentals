using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using _10_Filmdatenbank.Application.Interfaces;
using _10_Filmdatenbank.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddScoped<ITmdbService, TmdbService>();
builder.Services.AddHttpClient<ITvdbService, TvdbService>();
builder.Services.AddHttpClient<IRottenTomatoesService, RottenTomatoesService>();
builder.Services.AddHttpClient<IImdbService, ImdbService>();
builder.Services.AddHttpClient<IMetacriticService, MetacriticService>();
builder.Services.AddHttpClient<IWikidataService, WikidataService>();
builder.Services.AddHttpClient("TVDB", client =>
{
    client.BaseAddress = new Uri("https://api4.thetvdb.com/v4/");
});

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("_10_Filmdatenbank.Infrastructure")
              .EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
              .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
              .CommandTimeout(60)));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("InMemoryDbForTesting"));
}

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

builder.Services.AddCookiePolicy(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}
app.UseCookiePolicy();

var supportedCultures = new[] { "de", "en", "pt", "ru", "ar", "tr" };
var defaultCulture = builder.Environment.IsEnvironment("Testing") ? "en-US" : supportedCultures[0];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(defaultCulture)
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (context.Database.IsSqlServer())
    {
        await context.Database.MigrateAsync();
    }
    else
    {
        await context.Database.EnsureCreatedAsync();
    }
    
    await DbSeeder.SeedAsync(context);

    // Seed Admin/Member roles if needed
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!await roleManager.RoleExistsAsync("Member")) await roleManager.CreateAsync(new IdentityRole("Member"));

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    if (await userManager.FindByEmailAsync("admin@film.de") == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin@film.de",
            Email = "admin@film.de",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            CreatedAt = DateTime.UtcNow,
            IsDisabled = false
        };
        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    if (await userManager.FindByEmailAsync("user@film.de") == null)
    {
        var user = new ApplicationUser
        {
            UserName = "user@film.de",
            Email = "user@film.de",
            EmailConfirmed = true,
            FirstName = "Standard",
            LastName = "User",
            CreatedAt = DateTime.UtcNow,
            IsDisabled = false
        };
        await userManager.CreateAsync(user, "User123!");
        await userManager.AddToRoleAsync(user, "Member");
    }
}

app.Run();

public partial class Program { }

