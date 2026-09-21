using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var PhoneExist = await unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone, ct);
            var EmailExist = await unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email, ct);

            if (PhoneExist) return Result.Validation("Trainer with this phone number already exists");
            if (EmailExist) return Result.Validation("Trainer with this email already exists");

            var trainer = mapper.Map<Trainer>(model);

            unitOfWork.GetRepository<Trainer>().AddAsync(trainer, ct);

            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Trainer");
        }

        public async Task<Result> DeleteTrainerAsync(int id, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var hasSessions = await unitOfWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == id && s.StartDate > DateTime.Now, ct);
            if (hasSessions) return Result.Fail("Cannot delete a trainer with scheduled sessions");

            unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer, ct);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Trainer");
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync()
        {
            var result = await unitOfWork.GetRepository<Trainer>().GetAllAcync();
            var trainers = mapper.Map<IEnumerable<TrainerViewModel>>(result);
            return trainers;
        }

        public async Task<Result<TrainerDetailsViewModel>> GetTrainerByIdAsync(int id, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            
            if (trainer == null) 
                return Result<TrainerDetailsViewModel>.NotFound("Trainer Not Found");

            var model = mapper.Map<TrainerDetailsViewModel>(trainer);
            return Result<TrainerDetailsViewModel>.Ok(model);
        }

        public async Task<Result<CreateTrainerViewModel>> GetTrainerToUpdateAsync(int id, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            
            if (trainer == null) 
                return Result<CreateTrainerViewModel>.NotFound("Trainer Not Found");

            var model = mapper.Map<CreateTrainerViewModel>(trainer);
            return Result<CreateTrainerViewModel>.Ok(model);
        }

        public async Task<Result> UpdateTrainerAsync(int id, CreateTrainerViewModel model, CancellationToken ct)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer == null)
                return Result.NotFound("Trainer Not Found");

            var PhoneExist = await unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != id, ct);
            var EmailExist = await unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != id, ct);

            if (PhoneExist) return Result.Validation("Trainer with this phone number already exists");
            if (EmailExist) return Result.Validation("Trainer with this email already exists");

            mapper.Map(model, trainer);

            unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer, ct);
            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Trainer");
        }
    }
}
