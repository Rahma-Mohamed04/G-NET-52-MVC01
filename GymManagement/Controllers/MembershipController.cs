using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class MembershipController(IMembershipService membershipService) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await membershipService.GetAllMembershipAsync(ct);
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await DropDownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                await DropDownAsync(ct);
                return View(model);
            }

            var result = await membershipService.CreateMembershipAsync(model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;
            await DropDownAsync(ct);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await membershipService.DeleteActiveMembership(id, ct);
            TempData[result.success ? "SucessMessage" : "ErrorMessage"] = result.success ? "Membership Cancelled" : result.error;
            return RedirectToAction(nameof(Index));
        }

        private async Task DropDownAsync(CancellationToken ct)
        {
            ViewBag.Plans = new SelectList(await membershipService.GetPlanSelectListViewsAsync(ct), "Id", "Name");
            ViewBag.Members = new SelectList(await membershipService.GetMemberSelectListViewsAsync(ct), "Id", "Name");
        }
    }
}
