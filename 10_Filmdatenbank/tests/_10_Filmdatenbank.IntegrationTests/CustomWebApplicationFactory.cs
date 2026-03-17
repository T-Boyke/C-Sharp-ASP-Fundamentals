using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using _10_Filmdatenbank.Infrastructure.Persistence;
using System.Linq;

namespace _10_Filmdatenbank.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Also remove the DbContext itself if it was registered with a specific lifetime
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ApplicationDbContext));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated();

                    // Seed Test User
                    if (!db.Users.Any(u => u.Id == "test-user-id"))
                    {
                        db.Users.Add(new _10_Filmdatenbank.Domain.Entities.ApplicationUser
                        {
                            Id = "test-user-id",
                            UserName = "test@film.de",
                            Email = "test@film.de",
                            FirstName = "Test",
                            LastName = "User",
                            CreatedAt = System.DateTime.UtcNow
                        });
                        db.SaveChanges();
                    }
                }
            });
        }
    }
}
