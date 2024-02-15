
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataAccess.ViewModels;

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
        public IActionResult PatientLogin(PatientLogin patient)
        {

            if(ModelState.IsValid)
            {
                    Aspnetuser aspnetuser = new()
                    {
                        Email = patient.Email,
                        Passwordhash=patient.Password,
                    };

                var obj = _context.Aspnetusers.ToList();
             
                foreach (var item in obj)
                {
                    if(item.Email == patient.Email && item.Passwordhash == patient.Password)
                    {
                        User user = _context.Users.FirstOrDefault(u => u.Aspnetuserid == item.Id);
                        return RedirectToAction("Dashboard", user);
                    }
                }
            }
           return View(patient);
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
        
        
        public IActionResult Dashboard(User user)
        {
            PatientDashboard patientDashboard = new();
            
            List<Request> obj = _context.Requests.Where(req => req.Userid == user.Userid).ToList();
            List<int> fileCount = new List<int>();
            for (int i =0;i<obj.Count;i++)
            {
                int count = _context.Requestwisefiles.Count(rf => rf.Requestid == obj[i].Requestid);
                fileCount.Add(count);
            }

            patientDashboard.requests = obj;
            //patientDashboard.User = user;
            patientDashboard.File = fileCount;

            patientDashboard.Name = user.Firstname;
            return View(patientDashboard);
        }
            
            //User user = _context.Users.FirstOrDefault(u => u.Userid == id);
            //IQueryable<Requestwisefile> obj2 = _context.Requestwisefiles.Where(req => req.Requestid == obj.Requestid);

            



//View Document Page
public IActionResult ViewDocument(int id)
        {
            IEnumerable < Requestwisefile > fileList = _context.Requestwisefiles.Where(reqFile => reqFile.Requestid == id);

            return View(fileList);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}