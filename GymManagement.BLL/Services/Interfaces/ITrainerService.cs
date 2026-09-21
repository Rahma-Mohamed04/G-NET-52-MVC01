using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync();
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct);
        Task<Result<TrainerDetailsViewModel>> GetTrainerByIdAsync(int id, CancellationToken ct);
        Task<Result<CreateTrainerViewModel>> GetTrainerToUpdateAsync(int id, CancellationToken ct);
        Task<Result> UpdateTrainerAsync(int id, CreateTrainerViewModel model, CancellationToken ct);
        Task<Result> DeleteTrainerAsync(int id, CancellationToken ct);
    }
}
