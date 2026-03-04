using Microsoft.EntityFrameworkCore;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Infrastructure.Database {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Configure Headline ValueObject as an owned type constraint
            modelBuilder.Entity<Article>(entity => {
                entity.OwnsOne(e => e.Headline, hd => {
                    hd.Property(h => h.Value).HasColumnName("Headline").IsRequired().HasMaxLength(200);
                });
            });
        }
    }
}
