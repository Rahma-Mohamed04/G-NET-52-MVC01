using Microsoft.EntityFrameworkCore;
using MVC1.DAL.Context.Configurations;
using MVC1.DAL.Models;

namespace MVC1.DAL.Context
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymSystem;Trusted_Connection=true;TrustServerCertificate=true");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Plan>(new PlanConfiguration());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
