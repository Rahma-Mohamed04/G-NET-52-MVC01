using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDomnList(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDomnList(CancellationToken ct = default);
        Task<Result<SessionViewModel?>> GetSessionByIdAsync(int sessionId ,CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionToUpdate(int sessionId ,CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model ,CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);

    }
}
