using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Models {
    public static class EnsureIdentity {
        private const string adminRole = "Admin";
        private const string userRole = "User";

        private const string adminName = "Admin";
        private const string userName = "User";

        private const string password = "Secret123$";

        public static void Migrate(IApplicationBuilder app) {
            var ctx = app.ApplicationServices
                            .CreateScope()
                            .ServiceProvider
                            .GetRequiredService<AppDbContext>();

            if (ctx.Database.GetPendingMigrations().Any()) {
                ctx.Database.Migrate();
            }
        }
        public static async void SeedDefaultAccounts(IApplicationBuilder app) {
            var ctx = app.ApplicationServices
                            .CreateScope()
                            .ServiceProvider
                            .GetRequiredService<AppDbContext>();

            var roleManager = app.ApplicationServices
                                    .CreateScope()
                                    .ServiceProvider
                                    .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = app.ApplicationServices
                                    .CreateScope()
                                    .ServiceProvider
                                    .GetRequiredService<UserManager<IdentityUser>>();

            // Rollen anlegen
            if (!await roleManager.RoleExistsAsync(adminRole)) {
                var role = new IdentityRole(adminRole);
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync(userRole)) {
                var role = new IdentityRole(userRole);
                await roleManager.CreateAsync(role);
            }

            // User anlegen
            var admin = await userManager.FindByNameAsync(adminName);
            if (admin == null) {
                admin = new IdentityUser(adminName);
                await userManager.CreateAsync(admin, password);
                await userManager.AddToRoleAsync(admin, adminRole);
                await userManager.AddToRoleAsync(admin, userRole);
            }
            var user = await userManager.FindByNameAsync(userName);
            if (user == null) {
                user = new IdentityUser(userName);
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, userRole);
            }
        }
    }
}
