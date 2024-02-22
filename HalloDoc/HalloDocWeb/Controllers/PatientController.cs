
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using DataAccess.ViewModels;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using BusinessLogic.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                    HttpContext.Session.SetInt32("userId",user.Userid);
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
                    return RedirectToAction("ResetPassword", model);
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

        public IActionResult SomeoneElseInfo()
        {
            return View();
        }

        //public IActionResult ViewDocument(User user) 
        //{
        //    _patientService.GetMedicalHistory(user);

        //    //List<int> fileCount = new();
        //    //for (int i = 0; i < obj.Count; i++)
        //    //{
        //    //    int count = _context.Requestwisefiles.Count(rf => rf.Requestid == obj[i].Requestid);
        //    //    fileCount.Add(count);
        //    //}
        //    return View();
        //}

        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);

            if (user != null)
            {
                string dobDate = user.Intyear + "-" + user.Strmonth + "-" + user.Intdate;

                PatientProfile model = new()
                {
                    UserId = user.Userid,
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Type = "Mobile",
                    Phone = user.Mobile,
                    Email = user.Email,
                    Street = user.Street,
                    City = user.City,
                    State = user.State,
                };

                return View("Profile", model);
            }
            return RedirectToAction("Error");
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
                PatientDashboard dashboardVM = new PatientDashboard();
                dashboardVM.UserId = user.Userid;
                dashboardVM.UserName = user.Firstname + " " + user.Lastname;
                dashboardVM.Requests = _context.Requests.Where(req => req.Userid == user.Userid).ToList();
                List<int> fileCounts = new List<int>();
                foreach (var request in dashboardVM.Requests)
                {
                    int count = _context.Requestwisefiles.Count(reqFile => reqFile.Requestid == request.Requestid);
                    fileCounts.Add(count);
                }
                dashboardVM.DocumentCount = fileCounts;
                return View("Dashboard", dashboardVM);
            }

            return View("Error");
        }




        //View Document Page
        public IActionResult ViewDocument(int requestId)
        {
            IEnumerable<Requestwisefile> fileList = _context.Requestwisefiles.Where(reqFile => reqFile.Requestid == requestId);

            return View(fileList);
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