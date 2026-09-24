using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public class GymDataSeeding
    {
        public static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Failed to get File");

            var data = File.ReadAllText(filePath); // JSON => List
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }

        public static async Task SeedAsync(GymDbContext dbContext, string seedFolderPath, CancellationToken ct = default)
        {
            try
            {
                if (!await dbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");
                    
                    if (plans.Any())
                        dbContext.Plans.AddRange(plans);

                    if (dbContext.ChangeTracker.HasChanges())
                        await dbContext.SaveChangesAsync(ct);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
