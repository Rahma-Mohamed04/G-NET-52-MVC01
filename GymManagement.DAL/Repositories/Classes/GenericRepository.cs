using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly GymDbContext context;
        private readonly DbSet<TEntity> set;
        public GenericRepository(GymDbContext _context)
        {
            context = _context;
            set = context.Set<TEntity>();
        }
        public void AddAsync(TEntity entity, CancellationToken ct = default)
        {
            set.Add(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await set.AnyAsync(predicate, ct);
        }

        public void DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            set.Remove(entity);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? set : set.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAcync(bool tracking, CancellationToken ct = default)
        {
            return tracking ? await set.ToListAsync(ct) : await set.AsNoTracking().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int Id, CancellationToken ct = default)
        {
            return await set.FindAsync(Id, ct);
        }

        public void UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            set.Update(entity);
        }
    }
}
