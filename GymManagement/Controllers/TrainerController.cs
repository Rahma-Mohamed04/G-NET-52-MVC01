using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this.trainerService = trainerService;
        }
        public async Task<IActionResult> Index()
        {
            var trainers = await trainerService.GetAllTrainersAsync();
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await trainerService.CreateTrainerAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;
            return View(model);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await trainerService.GetTrainerByIdAsync(id, ct);
            
            if(result.success)
            {
                return View(result.value);
            }

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await trainerService.GetTrainerToUpdateAsync(id, ct);

            if (result.success)
            {
                return View(result.value);
            }

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await trainerService.UpdateTrainerAsync(id, model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await trainerService.GetTrainerByIdAsync(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await trainerService.DeleteTrainerAsync(id, ct);
            if (result.success)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
        }
    }
}
