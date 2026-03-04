using Microsoft.EntityFrameworkCore;
using _05_NewsApplication.Application.UseCases;
using _05_NewsApplication.Domain.Interfaces;
using _05_NewsApplication.Infrastructure.Database;
using _05_NewsApplication.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("database")));

// Repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

// Use Cases
builder.Services.AddScoped<CreateArticleUseCase>();
builder.Services.AddScoped<GetArticlesUseCase>();
builder.Services.AddScoped<GetAuthorsUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
