
using BusinessLogic.Interface;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace HalloDocWeb.Controllers
{
    public class RequestPagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPatientService _patientService;


        public RequestPagesController(ApplicationDbContext context, IAuthService authService, IPatientService patientService)
        {
            _context = context;
            _authService = authService;
            _patientService = patientService;
        }
        //GET
        public IActionResult PatientRequest()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientRequest(PatientSubmitRequest userDetails)
        {
            if (ModelState.IsValid)
            {
                _patientService.PatientRequest(userDetails);
                return RedirectToAction("Index", "Home");
            }
            return View(userDetails);
        }

        [HttpPost]
        public JsonResult PatientCheckEmail(string email)
        {
            bool emailExists = _context.Users.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        //GET
        public IActionResult FamilyFriendRequest()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FamilyFriendRequest(FamilyFriendSubmitRequest userDetails)
        {
            if (ModelState.IsValid)
            {
                _patientService.FamilyFriendRequest(userDetails);
                return RedirectToAction("Index", "Home");

            }
            return View(userDetails);
        }


        //GET
        public IActionResult ConciergeRequest()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConciergeRequest(ConciergeSubmitRequest userDetails)
        {
            if (ModelState.IsValid)
            {
                _patientService.ConciergeRequest(userDetails);
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index", "Home");
        }


        //Business Request
        public IActionResult BusinessRequest()
        {
            return View();
        }


        [HttpPost]
        public IActionResult BusinessRequest(BusinessSubmitRequest userDetails)
        {
            if (ModelState.IsValid)
            {
                _patientService.BusinessRequest(userDetails);
                return RedirectToAction("Index", "Home");
            }
            return View(userDetails);

        }
        public Task<bool> IsEmailExists(string email)
        {
            bool isExist = _context.Aspnetusers.Any(x => x.Email == email);
            if (isExist)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}