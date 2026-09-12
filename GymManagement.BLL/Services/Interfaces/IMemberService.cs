using GymManagement.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        public Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        public Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);
        public Task<MemberViewModel> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);
        public Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        public Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int id, CancellationToken ct = default);
        public Task<bool> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        public Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default);
        
    }
}
