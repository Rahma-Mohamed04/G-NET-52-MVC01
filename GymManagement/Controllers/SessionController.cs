using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService _sessionService)
        {
            sessionService = _sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions = await sessionService.GetAllSessionsAsync(ct);
            return View(Sessions);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await DropDownList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            await DropDownList();

            if (!ModelState.IsValid) return View(model);

            var result = await sessionService.CreateSessionAsync(model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await sessionService.GetSessionByIdAsync(id, ct);
            if (result.success)
                return View(result.value);
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await sessionService.GetSessionToUpdate(id, ct);
            if (result.success)
            {
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainersForDropDomnList(), "Id", "Name");
                return View(result.value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainersForDropDomnList(), "Id", "Name");
                return View(model);
            }

            var result = await sessionService.UpdateSessionAsync(id, model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Session";
                ViewBag.Trainers = new SelectList(await sessionService.GetTrainersForDropDomnList(), "Id", "Name");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await sessionService.GetSessionByIdAsync(id, ct);
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
            var result = await sessionService.DeleteSessionAsync(id, ct);
            if (!result.success)
            {
                TempData["ErrorMessage"] = result.error;
            }
            else
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task DropDownList()
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainersForDropDomnList(), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategoriesForDropDomnList(), "Id", "CategoryName");
        }
    }
}
