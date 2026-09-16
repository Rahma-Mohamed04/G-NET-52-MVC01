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

            if (result) return RedirectToAction(nameof(Index));

            return View(model);
        }

        private async Task DropDownList()
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainersForDropDomnList(), "Id", "Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategoriesForDropDomnList(), "Id", "CategoryName");
        }
    }
}
