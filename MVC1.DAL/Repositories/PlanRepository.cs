using Microsoft.EntityFrameworkCore;
using MVC1.DAL.Context;
using MVC1.DAL.Models;
using MVC1.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC1.DAL.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext context;
        public PlanRepository(GymDbContext _context)
        {
            context = _context;
        }
        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            context.Plans.Add(plan);
            return await context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            context.Plans.Remove(plan);
            return await context.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAcync(bool tracking, CancellationToken ct = default)
        {
            return tracking ? await context.Plans.ToListAsync(ct) : await context.Plans.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int Id, CancellationToken ct = default)
        { 
            return await context.Plans.FindAsync(Id, ct);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            context.Plans.Update(plan);
            return await context.SaveChangesAsync(ct);
        }
    }
}
