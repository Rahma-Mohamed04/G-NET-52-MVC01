using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC1.Context;

namespace MVC1.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext context;
        public PlansController()
        {
            context = new();
        }
        public async Task<IActionResult> Index()
        {
            var plans = await context.Plans.ToListAsync();
            return View(plans);
        }
        public async Task<IActionResult> Details(int Id)
        {
            var plan = await context.Plans.FindAsync(Id);
            return View(plan);
        }
    }
}
