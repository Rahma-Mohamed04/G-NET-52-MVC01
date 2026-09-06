using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC1.DAL.Context;
using MVC1.DAL.Repositories.Interfaces;

namespace MVC1.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanRepository planRepo;
        public PlansController(IPlanRepository _planRepo)
        {
            planRepo = _planRepo;
        }
        public async Task<IActionResult> Index()
        {
            var plans = await planRepo.GetAllAcync();
            return View(plans);
        }
        public async Task<IActionResult> Details(int Id)
        {
            var plan = await planRepo.GetByIdAsync(Id);
            return View(plan);
        }
    }
}
