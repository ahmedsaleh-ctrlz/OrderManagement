using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entites;
using OrderManagement.Infrastructure.Persistence.EntitesConfiguration;

namespace OrderManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        }

        public DbSet<User> Users { get; set; }

    }
}
