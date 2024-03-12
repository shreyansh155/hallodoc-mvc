
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using DataAccess.ViewModels;
using System.Diagnostics;
using BusinessLogic.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;


namespace HalloDocWeb.Controllers
{
    [CustomAuthorize((int)AllowRole.Patient)]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPatientService _patientService;
        private readonly INotyfService _notyf;

        public PatientController(ApplicationDbContext context, IAuthService authService, IPatientService patientService, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
            _authService = authService;
            _patientService = patientService;
        }

        public IActionResult PatientRequest()
        {
            return View();
        }


        public IActionResult PatientForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientForgotPassword(PatientForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                if (_authService.PatientForgotPassword(model))
                {
                    var user = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == model.Email);
                    return RedirectToAction("ResetPassword", user);
                }
            }
            return View(model);
        }

        public IActionResult ResetPassword(PatientResetPassword model)
        {
            return View(model);
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public IActionResult Resetpassword(PatientResetPassword patientResetPassword)
        {
            if (ModelState.IsValid)
            {
                _authService.PatientResetPassword(patientResetPassword);
                _notyf.Success("Password Reset Successfully", 3);
                return RedirectToAction("PatientLogin");
            }
            _notyf.Error("Please provide correct credentials",3);
            return View();
        }

        public IActionResult SubmitInfoAboutMe()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitInfoAboutMe(PatientSubmitRequest userDetails)
        {
            if (ModelState.IsValid)
            {
                _patientService.PatientRequest(userDetails);
            }
            return View();
        }

        public IActionResult SomeoneElseInfo()
        {
            return View();
        }

        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);

            if (user != null)
            {
                var patientDetails = _patientService.Profile((int)userId);
                return View("Profile", patientDetails);
            }
            return RedirectToAction("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(PatientProfile profile)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (ModelState.IsValid)
            {
                _patientService.ProfileUpdate(profile, (int)userId);
            }
            return View();
        }


        public IActionResult CreateNewAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewAccount(CreateNewAccount newAccount)
        {
            if (ModelState.IsValid)
            {
                _authService.CreateNewAccount(newAccount);
                _notyf.Success("Account Created Successfully", 3);
                return RedirectToAction("PatientLogin");
            }
            return View();
        }


        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return View("Error");
            }

            User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);

            if (user != null)
            {
                var dashboardVM = _patientService.PatientDashboard((int)userId);
                return View("Dashboard", dashboardVM);
            }

            return View("Error");
        }


        //View Document Page
        public IActionResult ViewDocument(int requestId)
        {
            int? userid = HttpContext.Session.GetInt32("userId");
            var document=_patientService.ViewDocument(requestId,(int)userid);
            return View(document);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewDocument(ViewDocument document)
        {
            if (document != null)
            {
                _patientService.ViewDocument(document);
            }
            return ViewDocument(document.RequestId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("hallodoc");
            return RedirectToAction("PatientLogin", "Home");
        }
    }
}