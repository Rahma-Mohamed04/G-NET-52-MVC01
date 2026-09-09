using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;
        public MemberController(IMemberService _memberService)
        {
            memberService = _memberService;
        }
        public async Task<IActionResult> Index()
        {
            var members = await memberService.GetAllMembersAsync();
            return View(members);
        }
    }
}
