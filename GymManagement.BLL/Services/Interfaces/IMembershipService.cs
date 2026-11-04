using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<IEnumerable<MembershipViewModel>> GetAllMembershipAsync(CancellationToken ct = default!);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlanSelectListViewsAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMemberSelectListViewsAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default);
        Task<Result> DeleteActiveMembership(int memberId, CancellationToken ct = default!);
    }
}
