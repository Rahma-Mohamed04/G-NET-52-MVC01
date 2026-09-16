using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext context;
        private readonly Dictionary<string, object> repositories = new();

        public UnitOfWork(GymDbContext _context, ISessionRepository _SessionRepository)
        {
            context = _context;
            SessionRepository = _SessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            string typeName = typeof(TEntity).Name;

            if (repositories.TryGetValue(typeName, out var repo))
                return (IGenericRepository<TEntity>)repo;
            else
            {
                var newRepo = new GenericRepository<TEntity>(context);
                repositories[typeName] = newRepo;
                return newRepo;
            }
        }


        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await context.SaveChangesAsync();
    }
}
