using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC1.DAL.Models;

namespace MVC1.DAL.Context.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(X => X.Name)
                   .HasColumnType("varchar(50)");

            builder.Property(X => X.Description)
                   .HasMaxLength(200);

            builder.Property(X => X.Price)
                   .HasPrecision(10, 2);

            builder.Property(X => X.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(X => X.UpdatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("PlanDurationCheck", "DurationDays Between 1 and 365");
            });

            builder.HasData(
                new() { Id = 1, Name = "Basic Plan", Description = "Access to gym equipment during working hours.", DurationDays = 30, Price = 500m, IsActive = true },
                new() { Id = 2, Name = "Standard Plan", Description = "Full gym access with basic fitness guidance.", DurationDays = 90, Price = 1200m, IsActive = true },
                new() { Id = 3, Name = "Premium Plan", Description = "Full gym access with personal training sessions.", DurationDays = 180, Price = 2200m, IsActive = false },
                new() { Id = 4, Name = "Annual Plan", Description = "Unlimited gym access for one year with premium benefits.", DurationDays = 365, Price = 4000m, IsActive = true },
                new() { Id = 5, Name = "Student Plan", Description = "Affordable gym membership for students.", DurationDays = 30, Price = 350m, IsActive = false },
                new() { Id = 6, Name = "Weekend Plan", Description = "Gym access on weekends only.", DurationDays = 30, Price = 300m, IsActive = true }
            );
        }
    }
}
