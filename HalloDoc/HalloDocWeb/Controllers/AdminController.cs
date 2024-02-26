using Microsoft.AspNetCore.Mvc;
using DataAccess.ViewModels;
using DataAccess.DataContext;
using BusinessLogic.Repository;
using BusinessLogic.Interface;
using DataAccess.DataModels;

namespace HalloDocWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminService _adminService;
        public AdminController(ApplicationDbContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }


        public IActionResult AdminLogin()
        {

            return View();
        }

        public IActionResult CreateAdminAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAdminAccount(CreateAdminAccount newAccount)
        {
            if (ModelState.IsValid)
            {
                _adminService.CreateAdminAccount(newAccount);
                return RedirectToAction("AdminLogin");
            }
            return View();
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(AdminLogin adminLogin)
        {
            if (ModelState.IsValid)
            {
                if (_adminService.AdminLogin(adminLogin))
                {
                    Admin admin = _context.Admins.FirstOrDefault(x => x.Email == adminLogin.Email);
                    HttpContext.Session.SetInt32("adminId", admin.Adminid);
                    return RedirectToAction("AdminDashboard");
                }
            }
            return View(adminLogin);
        }

        public IActionResult AdminDashboard()
        {
            int? adminId = HttpContext.Session.GetInt32("adminId");
            var data = _adminService.AdminDashboard();
            return View(data);
        }

        public IActionResult ViewCase()
        {
            return View();
        }
        public IActionResult ViewNotes()
        {
            return View();
        }

    }
}
