using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Data
{
    public class MainDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {

        }

        public DbSet<RiskEntity> Risks { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasOne(a => a.Risk)
                .WithOne(b => b.AppUser)
                .HasForeignKey<RiskEntity>(b => b.UserId);
        }
    }
}