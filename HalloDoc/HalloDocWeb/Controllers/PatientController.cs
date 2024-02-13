
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HalloDocWeb.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientLogin(Aspnetuser User)
        {
            var obj = _context.Aspnetusers.ToList();

            foreach (var item in obj)
            {
                if(item.Username == User.Username && item.Passwordhash == User.Passwordhash)
                {
                    return View("PatientForgotPassword");
                }
            }
            return View();
        }

        public IActionResult PatientForgotPassword()
        {
            return View();
        }
        public IActionResult PatientSubmitRequest()
        {
            return View();
        }
        public IActionResult PatientRequest()
        {
            return View();
        }
        
        public IActionResult Dashboard()
        {
            IEnumerable<Request>  obj = _context.Requests;
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}