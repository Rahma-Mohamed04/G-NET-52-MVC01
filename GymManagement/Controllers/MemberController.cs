using AutoMapper.Execution;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;
        private readonly IAttachmentService attachmentService;

        public MemberController(IMemberService _memberService, IAttachmentService attachmentService)
        {
            memberService = _memberService;
            this.attachmentService = attachmentService;
        }
        public async Task<IActionResult> Index()
        {
            var members = await memberService.GetAllMembersAsync();
            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(nameof(Create), model);
            var result = await memberService.CreateMemberAsync(model, ct);

            if (result) TempData["SuccessMessage"] = "Member Created Succesfully";
            else TempData["ErrorMessage"] = "Member Falied to Create !";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberDetailsByIdAsync(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var record = await memberService.GetMemberHealthRecordAsync(id, ct);
            if (record == null)
            {
                TempData["ErrorMessage"] = "No Health Record Found !";
                return RedirectToAction(nameof(Index)); 
            }
            return View(record);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await memberService.GetMemberToUpdateAsync(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Fount !";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await memberService.UpdateMemberAsync(id, model);

            if (result) TempData["SuccessMessage"] = "Member Updated Succesfully";
            else TempData["ErrorMessage"] = "Member Falied to Update !";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await memberService.GetMemberDetailsByIdAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await memberService.DeleteMemberAsync(id, ct);

            if (result) TempData["SuccessMessage"] = "Member Deleted Succesfully";
            else TempData["ErrorMessage"] = "Member Falied to Delete !";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await memberService.GetMemberDetailsByIdAsync(id);
            if (member is null || string.IsNullOrWhiteSpace(member.Photo))
                return NotFound();

            var result = attachmentService.GetFile(member.Photo, "MembersPhoto");
            if (result == null) return NotFound();

            return File(result.Value.stream, result.Value.contentType);
        }
    }
}
