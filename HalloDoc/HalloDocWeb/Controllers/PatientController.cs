
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataAccess.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using BusinessLogic.Repository;
using BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HalloDocWeb.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;
        /*private readonly IPatientService _patientService*/
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

        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientLogin(PatientLogin patientLogin )
        {
            if (ModelState.IsValid)
            {
                if (_authService.PatientLogin(patientLogin))
                {
                    User user=_context.Users.FirstOrDefault(Au => Au.Email == patientLogin.Email);
                    return RedirectToAction("Dashboard",user);
                }
            }
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

        public IActionResult SubmitInfoAboutMe()
        {
            return View();
        }

        public IActionResult SomeoneElseInfo()
        {
            return View();
        }
        public IActionResult Dashboard(User user)
        {
            _patientService.GetMedicalHistory(user);
       
            //List<int> fileCount = new();
            //for (int i = 0; i < obj.Count; i++)
            //{
            //    int count = _context.Requestwisefiles.Count(rf => rf.Requestid == obj[i].Requestid);
            //    fileCount.Add(count);
            //}
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

    }
}
//Function tp Generate Password hash



//        public PatientController(ApplicationDbContext context)
//        {
//            _context = context;
//        }



//        public IActionResult CreateNewAccount()
//        {
//            return View();
//        }



//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult CreateNewAccount(CreateNewAccount newAccount)
//        {

//            return RedirectToAction("PatientLogin");
//        }

//        public IActionResult PatientLogin()
//        {
//            return View();
//        }

//        public IActionResult PatientForgotPassword()
//        {

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult PatientForgotPassword(PatientForgotPassword model)
//        {
//            if (ModelState.IsValid)
//            {
//                //Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == model.Email);
//                //if (obj == null)
//                //{
//                //    return View(model);
//                //}
//                //else
//                //{
//                //    var user = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == model.Email);
//                //    PatientResetPassword patientResetPassword = new()
//                //    {
//                //        Email = model.Email
//                //    };
//                //    return RedirectToAction("ResetPassword", patientResetPassword);
//            }
//            return View(model);
//        }

//    //public IActionResult ResetPassword(PatientResetPassword patientResetPassword)
//    //{
//    //    //PatientResetPassword patientResetPassword = new PatientResetPassword();
//    //    //patientResetPassword.Email = Email;
//    //    //return View(patientResetPassword);
//    //    return View(patientResetPassword);
//    //}

//    [HttpPost, ActionName("ResetPassword")]
//    [ValidateAntiForgeryToken]
//    public IActionResult Resetpassword(PatientResetPassword patientResetPassword)
//    {
//        if (ModelState.IsValid)
//        {
//            //if (patientResetPassword.Password == patientResetPassword.ConfirmPassword)
//            //{
//            //    Aspnetuser user1 = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == patientResetPassword.Email);
//            //    string hashPassword = GenerateSHA256(patientResetPassword.Password);
//            //    user1.Passwordhash = hashPassword;
//            //    _context.Aspnetusers.Update(user1);
//            //    _context.SaveChanges();
//            //}
//        }
//        return View();
//    }


//    public IActionResult PatientSubmitRequest()
//    {
//        return View();
//    }
//    public IActionResult PatientRequest()
//    {
//        return View();
//    }

//    public IActionResult SubmitInfoAboutMe()
//    {
//        return View();
//    }

//    public IActionResult SomeoneElseInfo()
//    {
//        return View();
//    }


//public IActionResult Dashboard(User user)
//{
//    PatientDashboard patientDashboard = new();

//    List<Request> obj = _context.Requests.Where(req => req.Userid == user.Userid).ToList();
//    List<int> fileCount = new();
//    for (int i = 0; i < obj.Count; i++)
//    {
//        int count = _context.Requestwisefiles.Count(rf => rf.Requestid == obj[i].Requestid);
//        fileCount.Add(count);
//    }

//    patientDashboard.requests = obj;

//    patientDashboard.File = fileCount;

//    patientDashboard.Name = user.Firstname;
//    return View(patientDashboard);
//}




//    //View Document Page
//    public IActionResult ViewDocument(int id)
//    {
//        IEnumerable<Requestwisefile> fileList = _context.Requestwisefiles.Where(reqFile => reqFile.Requestid == id);

//        return View(fileList);
//    }



//    public IActionResult ReviewAgreement()
//    {
//        return View();
//    }

//    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    public IActionResult Error()
//    {
//        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//    }
//}