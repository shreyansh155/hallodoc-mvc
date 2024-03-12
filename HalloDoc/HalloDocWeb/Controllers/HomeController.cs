using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interface;
using BusinessLogic.Repository;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDocWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IAdminService _adminService;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IAuthService auth, IJwtService jwtService, IAdminService adminService, INotyfService notyf)
        {
            _notyf = notyf;
            _logger = logger;
            _context = context;
            _authService = auth;
            _jwtService = jwtService;
            _adminService = adminService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
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
                    SessionUser suser = new SessionUser
                    {
                        Email = user.Email,
                        RoleId = (int)AllowRole.Patient,
                        UserId = user.Userid,
                        UserName = user.Firstname + " " + user.Lastname
                    };
                    var token = _jwtService.GenerateJwtToken(suser);
                    //Response.Cookies["userId"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Delete("hallodoc");
                    Response.Cookies.Append("hallodoc", token);
                    _notyf.Success("Login Successful", 3);
                    return RedirectToAction("Dashboard", "Patient");
                }
            }
            return View();
        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AdminLogin(AdminLogin adminLogin)
        {
            if (ModelState.IsValid)
            {
                if (_adminService.AdminLogin(adminLogin))
                {
                    Admin admin = _context.Admins.FirstOrDefault(x => x.Email == adminLogin.Email);
                    HttpContext.Session.SetInt32("adminId", admin.Adminid);
                    SessionUser suser = new SessionUser
                    {
                        Email = admin.Email,
                        RoleId = (int)AllowRole.Admin,
                        UserId = admin.Adminid,
                        UserName = admin.Firstname + " " + admin.Lastname
                    };
                    var token = _jwtService.GenerateJwtToken(suser);
                    Response.Cookies.Append("hallodoc", token);
                    _notyf.Success("Login Successful", 3);
                    return RedirectToAction("AdminDashboard", "Admin");

                }

            }
            return View(adminLogin);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ReviewAgreement(int reqClientId)
        {
            ReviewAgreementModal modal = new()
            {
                ReqClientId = reqClientId,
            };
            return View(modal);
        }
        [HttpPost]
        public IActionResult ReviewAgreement(ReviewAgreementModal model)
        {
            _authService.ReviewAgreementModal(model.ReqClientId);
            return RedirectToAction("PatientLogin");
        }

        public IActionResult CancelAgreementModal(int requestClientId)
        {
            Requestclient? reqCli = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == requestClientId);
            CancelAgreementModal obj = new()
            {
                ReqClientId = requestClientId,
                PatientName = reqCli.Firstname+" "+reqCli.Lastname
            };

            return PartialView("_CancelAgreementModal", obj);
        }
        public IActionResult CancelAgreementSubmit(int ReqClientid, string Description)
        {
            _authService.CancelAgreementSubmit(ReqClientid, Description);
            return RedirectToAction("PatientLogin");
        }


    }
}
