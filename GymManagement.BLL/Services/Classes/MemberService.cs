using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;

        public MemberService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var EmailExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            var PhoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

            if (EmailExist || PhoneExist) return false;

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            unitOfWork.GetRepository<Member>().AddAsync(member);
            var result = await unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member == null) return false;

            var HasActiveSession = await unitOfWork.GetRepository<Booking>().AnyAsync(x => x.MemberId == id && x.Session.StartDate > DateTime.Now, ct);
            if(HasActiveSession) return false;

            unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAcync(ct: ct);
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

        public async Task<MemberViewModel> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id);
            if (member == null) return null!;

            var model = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address?.BuildingNumber} - {member.Address?.Street} - {member.Address?.City}",

            };

            var membership = await unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(x => x.Id == id && x.EndDate > DateTime.Now, ct: ct);

            if (membership != null)
            {
                var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(membership.PlanId, ct);
                model.PlanName = plan.Name;
                model.MembershipStartDate = membership.CreatedAt.ToString();
                model.MembershipEndDate = membership.EndDate.ToString();
            }
            return model;
        }

        public async Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == memberId);
            if (record == null) return null!;

            return new HealthRecordViewModel()
            {
                Weight = record.Weight,
                Height = record.Height,
                BloodType = record.BloodType,
                Note = record.Note
            };
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member == null) return null!;
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Phone = member.Phone,
                Email = member.Email,
                City = member.Address.City,
                BuildingNumber = (int) member.Address.BuildingNumber,
                Street = member.Address.Street,
                Photo = member.Photo
            };
        }

        public async Task<bool> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);

            var EmailExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id, ct);
            var PhoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id, ct);

            if (EmailExist || PhoneExist) return false;

            member.Phone = model.Phone;
            member.Email = model.Email;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var result = await unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
