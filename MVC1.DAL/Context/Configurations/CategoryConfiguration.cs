using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC1.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC1.DAL.Context.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(X => X.CategoryName)
                   .HasColumnType("varchar")
                   .HasMaxLength(20);

            builder.Property(X => X.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Category { Id = 1, CategoryName = "Cardio" },
                new Category { Id = 2, CategoryName = "Strength" },
                new Category { Id = 3, CategoryName = "Boxing" },
                new Category { Id = 4, CategoryName = "Yoga" },
                new Category { Id = 5, CategoryName = "CrossFit" }
            );
        }
    }
}
