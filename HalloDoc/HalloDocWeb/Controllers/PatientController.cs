
using DataAccess.DataModels;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DataAccess.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace HalloDocWeb.Controllers
{
    public class PatientController : Controller
    {

        private readonly ApplicationDbContext _context;



        //Function tp Generate Password hash
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


        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CreateNewAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateNewAccount(CreateNewAccount newAccount)
        {
            Guid id = Guid.NewGuid();



            if (newAccount.Password == newAccount.ConfirmPassword)
            {
                var hashedPassword = GenerateSHA256(newAccount.Password);
                Aspnetuser aspnetuser = new()
                {
                    Id = id.ToString(),
                    Passwordhash = hashedPassword,
                    Createddate = DateTime.Now,
                    Username = newAccount.UserName,
                    Email = newAccount.Email,

                };
                _context.Aspnetusers.Add(aspnetuser);
                _context.SaveChanges();
                User user = new()
                {
                    Aspnetuserid = aspnetuser.Id,
                    Email = newAccount.Email,
                    Firstname = newAccount.UserName,
                    Createddate = DateTime.Now,
                    Createdby = newAccount.UserName,

                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("PatientLogin");
            }
            return View();
        }

        public IActionResult PatientLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientLogin(PatientLogin patient)
        {

            if (ModelState.IsValid)
            {
                var obj = _context.Aspnetusers.ToList();

                string hashPassword = GenerateSHA256(patient.Password);
                //match the email and pw with database entry
                foreach (var item in obj)
                {
                    if (item.Email == patient.Email && item.Passwordhash == hashPassword)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PatientForgotPassword(PatientForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == model.Email);
                if (obj == null)
                {
                    return View(model);
                }
                else
                {
                    var user = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == model.Email);
                    PatientResetPassword patientResetPassword = new()
                    {
                        Email = model.Email
                    };
                    return RedirectToAction("ResetPassword", patientResetPassword);
                }
            };
            return View(model);
        }
        public IActionResult ResetPassword(PatientResetPassword patientResetPassword)
        {
            //PatientResetPassword patientResetPassword = new PatientResetPassword();
            //patientResetPassword.Email = Email;
            //return View(patientResetPassword);
            return View(patientResetPassword);
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public IActionResult Resetpassword(PatientResetPassword patientResetPassword)
        {
            if (ModelState.IsValid)
            {
                if (patientResetPassword.Password == patientResetPassword.ConfirmPassword)
                {
                    Aspnetuser user1 = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == patientResetPassword.Email);
                    string hashPassword = GenerateSHA256(patientResetPassword.Password);
                    user1.Passwordhash = hashPassword;
                    _context.Aspnetusers.Update(user1);
                    _context.SaveChanges();
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
            PatientDashboard patientDashboard = new();

            List<Request> obj = _context.Requests.Where(req => req.Userid == user.Userid).ToList();
            List<int> fileCount = new();
            for (int i = 0; i < obj.Count; i++)
            {
                int count = _context.Requestwisefiles.Count(rf => rf.Requestid == obj[i].Requestid);
                fileCount.Add(count);
            }

            patientDashboard.requests = obj;

            patientDashboard.File = fileCount;

            patientDashboard.Name = user.Firstname;
            return View(patientDashboard);
        }




        //View Document Page
        public IActionResult ViewDocument(int id)
        {
            IEnumerable<Requestwisefile> fileList = _context.Requestwisefiles.Where(reqFile => reqFile.Requestid == id);

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