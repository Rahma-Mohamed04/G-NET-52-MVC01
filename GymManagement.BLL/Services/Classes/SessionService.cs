using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork _unitOfWork, IMapper mapper)
        {
            unitOfWork = _unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate
                || model.StartDate <= DateTime.Now
                || model.Capacity < 1
                || model.Capacity > 25) return false;

            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return false;

            var category = await unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return false;

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return false;

            var session = mapper.Map<Session>(model);

            unitOfWork.GetRepository<Session>().AddAsync(session);
            var result = await unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory(ct);
            if (sessions == null || !sessions.Any()) return null!;

            var mappedSession = sessions.Select(s => new SessionViewModel()
            {
                Id = s.Id,
                TrainerName = s.Trainer.Name,
                CategoryName = s.Category.CategoryName,
                Capacity = s.Capacity,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Description = s.Description
            });

            foreach(var session in mappedSession )
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mappedSession;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDomnList(CancellationToken ct = default)
        {
            var result = await unitOfWork.GetRepository<Category>().GetAllAcync();
            return mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDomnList(CancellationToken ct = default)
        {
            var result = await unitOfWork.GetRepository<Trainer>().GetAllAcync();
            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }
    }
}
