using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> memberRepo;

        public MemberService(IGenericRepository<Member> _memberRepo)
        {
            memberRepo = _memberRepo;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await memberRepo.GetAllAcync(ct: ct);
            if (!members.Any()) return new List<MemberViewModel>();
            
            List<MemberViewModel> membersVM = new List<MemberViewModel>();
            foreach (var member in members)
            {
                membersVM.Add(new()
                {
                    Id = member.Id,
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Gender = member.Gender.ToString(),
                    Photo = member.Photo,
                });
            }
            return membersVM;
        }
    }
}
