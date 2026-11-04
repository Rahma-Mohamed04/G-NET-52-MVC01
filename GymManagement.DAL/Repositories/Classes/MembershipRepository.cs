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
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(GymDbContext context) : base(context) { }
        
        public async Task<List<Membership>> GetAllMembershipsWithMembersAndPlansAsync(Expression<Func<Membership, bool>>? expression = null, CancellationToken ct = default)
        {
            var result = set.Include(p => p.Plan).Include(p => p.Member).AsNoTracking();
            return expression != null ? await result.Where(expression).ToListAsync(ct) : await result.ToListAsync(ct);
        }
    }
}
