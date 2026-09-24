using GymManagement.DAL.Context;
using GymManagement.DAL.DataSeeding;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace GymManagement
{
    public static class ProgramExtension
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();

            var dbContext = Scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = Scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync(); 

            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath);
            await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager);
        }
    }
}
