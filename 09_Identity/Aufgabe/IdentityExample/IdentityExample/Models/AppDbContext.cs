using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Models {
    public class AppDbContext : IdentityDbContext<IdentityUser> {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    }
}
