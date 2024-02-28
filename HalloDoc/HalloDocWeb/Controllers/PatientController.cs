
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using DataAccess.ViewModels;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;


namespace HalloDocWeb.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPatientService _patientService;


        public PatientController(ApplicationDbContext context, IAuthService authService, IPatientService patientService)
        {
            _context = context;
            _authService = authService;
            _patientService = patientService;
        }
        //Generate PasswordHash
        public static string GenerateSHA256(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hashEngine = SHA256.Create())
            {
                var hashedBytes = hashEngine.ComputeHash(bytes, 0, bytes.Length);
                var sb = new StringBuilder();
                foreach (var b in hashedBytes)
                {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }
                return sb.ToString();
            }
        }

        public IActionResult PatientRequest()
        {
            return View();
        }
        public IActionResult PatientSubmitRequest()
        {
            return View();
        }
        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientLogin(PatientLogin patientLogin)
        {
            if (ModelState.IsValid)
            {
                if (_authService.PatientLogin(patientLogin))
                {
                    User user = _context.Users.FirstOrDefault(Au => Au.Email == patientLogin.Email);
                    HttpContext.Session.SetInt32("userId", user.Userid);
                    return RedirectToAction("Dashboard");
                }
            }
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
                return RedirectToAction("PatientLogin");
            }
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
               var patientDetails= _patientService.Profile((int)userId);
                return View("Profile",patientDetails);
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
            User user = _context.Users.FirstOrDefault(u => u.Userid == userid);
            Request request = _context.Requests.FirstOrDefault(r => r.Requestid == requestId);
            List<Requestwisefile> fileList = _context.Requestwisefiles.Where(reqFile => reqFile.Requestid == requestId).ToList();

            ViewDocument document = new()
            {
                requestwisefiles = fileList,
                RequestId = requestId,

                ConfirmationNumber = request.Confirmationnumber,
                UserName = user.Firstname + " " + user.Lastname,
            };
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


        public IActionResult ReviewAgreement()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}