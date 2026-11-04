using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End Date Must be Greater Than Start Date");
            if( model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must be in The Future");
            if ( model.Capacity < 1 || model.Capacity > 25) return Result.Validation("End Date Must be Greater Than Start Date"); ;

            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return Result.Validation("Cannot Create Session Because Category Trainer");

            var session = mapper.Map<Session>(model);

            unitOfWork.GetRepository<Session>().AddAsync(session);
            var result = await unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.Ok(): Result.Fail("Failed to Create Session"); 
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await unitOfWork.GetRepository<Session>().GetByIdAsync(id);
            
            if(session == null) return Result.NotFound("Session Not Found");
            if(session.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now) 
                return Result.Fail("Cannot Delete an Ongoing Session");
            
            unitOfWork.GetRepository<Session>().DeleteAsync(session);
            var result = await unitOfWork.SaveChangesAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed to Delete Session");
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
            var result = await unitOfWork.GetRepository<Category>().GetAllAsync();
            return mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<Result<SessionViewModel?>> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategory(sessionId);
            if (session is null)
                return Result<SessionViewModel>.NotFound("Session Not Found");
            else
            {
                var mappedSession = mapper.Map<Session, SessionViewModel>(session);
                mappedSession.AvailableSlots = mappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
                return Result<SessionViewModel>.Ok(mappedSession);
            }
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdate(int sessionId, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetByIdAsync(sessionId);
            if (session is null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result<UpdateSessionViewModel>.Fail("Cannot Update Session That Already Started");

            var bookingCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookingCount > 0)
                return Result<UpdateSessionViewModel>.Fail("Cannot Update Session That Already Booked !");

            var mappedSession = mapper.Map<Session, UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mappedSession);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDomnList(CancellationToken ct = default)
        {
            var result = await unitOfWork.GetRepository<Trainer>().GetAllAsync();
            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetByIdAsync(id, ct);
            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot Update Session That Already Started");
           
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be greater than start date");

            var bookingCount = await unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookingCount > 0)
                return Result.Fail("Cannot Update Session That Already Booked !");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future");
                

            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);

            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.Specialty != CategorySpecialty) return Result.Validation("Cannot Create Session Because Category Trainer");

            mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;

            unitOfWork.SessionRepository.UpdateAsync(session); 
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to update Session !");
        }
    }
}
