using Microsoft.EntityFrameworkCore;
using _07_Patienten.Infrastructure.Data;
using _07_Patienten.Domain.Interfaces;
using _07_Patienten.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// EF Core Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository Registration
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IExaminationRepository, ExaminationRepository>();

var app = builder.Build();

// Automated Database Setup & Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patients}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
