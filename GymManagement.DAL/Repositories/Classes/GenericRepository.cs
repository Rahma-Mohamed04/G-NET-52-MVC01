using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext context;
        private readonly DbSet<TEntity> set;
        public GenericRepository(GymDbContext _context)
        {
            context = _context;
            set = context.Set<TEntity>();
        }
        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            set.Add(entity);
            return await context.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            set.Remove(entity);
            return await context.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAcync(bool tracking, CancellationToken ct = default)
        {
            return tracking ? await set.ToListAsync(ct) : await set.AsNoTracking().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int Id, CancellationToken ct = default)
        {
            return await set.FindAsync(Id, ct);
        }

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            set.Update(entity);
            return await context.SaveChangesAsync(ct);
        }
    }
}
