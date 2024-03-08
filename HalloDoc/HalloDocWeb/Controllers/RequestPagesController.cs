
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDocWeb.Controllers
{
    public class RequestPagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        private readonly IPatientService _patientService;


        public RequestPagesController(ApplicationDbContext context, IPatientService patientService,INotyfService notyf)
        {
            _notyf = notyf;
            _context = context;
            _patientService = patientService;
        }
        public IActionResult PatientSubmitRequest()
        {
            return View();
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
                _notyf.Success("Request Submitted Successfully", 3);
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
                _notyf.Success("Request Submitted Successfully", 3);
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
                _notyf.Success("Request Submitted Successfully", 3);
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
                _notyf.Success("Request Submitted Successfully", 3);
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