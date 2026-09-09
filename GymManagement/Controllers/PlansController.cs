using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagement.DAL.Context;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DAL.Models;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> planRepo;
        public PlansController(IGenericRepository<Plan> _planRepo)
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
