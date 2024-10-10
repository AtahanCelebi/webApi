using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;
using System;

namespace ProductsAPI.Data
{
    public class MainDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        // Risks Tablosu
        public DbSet<RiskEntity> Risks { get; set; }

        // Kullanıcı ve Risk arasındaki ilişkiyi yapılandırma
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppUser ile RiskEntity arasında birebir ilişki
            modelBuilder.Entity<AppUser>()
                .HasOne(a => a.Risk)
                .WithOne(b => b.AppUser)
                .HasForeignKey<RiskEntity>(b => b.UserId);
        }
    }
}
