using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager,
                                                       UserManager<ApplicationUser> userManager)
        {
            bool HasUsers = await userManager.Users.AnyAsync();
            bool HasRoles = await roleManager.Roles.AnyAsync();
            if (HasUsers && HasRoles) return;

            var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")
            };


            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    var roleResult = await roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine("Failed to Create Role !");
                    }
                }
            }

            if (!HasUsers)
            {
                var MainAdmin = new ApplicationUser()
                {
                    FirstName = "Hossam",
                    LastName = "Mostafa",
                    Email = "hossam1000@gmail.com",
                    UserName = "HossamMostafa",
                    PhoneNumber = "01000136598"
                };

                await userManager.CreateAsync(MainAdmin, "P@ssw0rd");   
                await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");   

                var Admin = new ApplicationUser()
                {
                    FirstName = "Esraa",
                    LastName = "Mostafa",
                    Email = "esraa@gmail.com",
                    UserName = "EsraaMostafa",
                    PhoneNumber = "01000136599"
                };

                await userManager.CreateAsync(Admin, "P@ssw0rd");
                await userManager.AddToRoleAsync(Admin, "Admin");
            }
        }


    }
}
