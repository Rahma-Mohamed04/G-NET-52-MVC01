using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Context.Configurations
{
    internal class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(X => X.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(X => X.UpdatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Member { Id = 1, Name = "Ahmed Ali", Phone = "01012345678", Email = "ahmed.ali@email.com", DateOfBirth = new DateOnly(1995, 5, 12), Photo = "~/images/about p1.jpg", Gender = Gender.Male },
                new Member { Id = 2, Name = "Fatma Omar", Phone = "01123456789", Email = "fatma.omar@email.com", DateOfBirth = new DateOnly(1998, 8, 20), Photo = "~/images/about p3.jpg", Gender = Gender.Felmale },
                new Member { Id = 3, Name = "Mahmoud Hassan", Phone = "01234567890", Email = "mahmoud.hassan@email.com", DateOfBirth = new DateOnly(1990, 2, 15), Photo = null, Gender = Gender.Male },
                new Member { Id = 4, Name = "Salma Khaled", Phone = "01598765432", Email = "salma.khaled@email.com", DateOfBirth = new DateOnly(2000, 11, 5), Photo = "~/images/workout 2.jpg", Gender = Gender.Felmale },
                new Member { Id = 5, Name = "Youssef Ibrahim", Phone = "01098765431", Email = "youssef.ibrahim@email.com", DateOfBirth = new DateOnly(1992, 7, 30), Photo = "~/images/card-header.jpg", Gender = Gender.Male }
            );
            base.Configure(builder);
        }

    }
}
