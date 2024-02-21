
using BusinessLogic.Interface;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

namespace HalloDocWeb.Controllers
{
    public class RequestPagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPatientService _patientService;


        public RequestPagesController(ApplicationDbContext context, IAuthService authService, IPatientService patientService)
        {
            _context = context;
            _authService = authService;
            _patientService = patientService;
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
            Guid id = Guid.NewGuid();
            Aspnetuser user = new()
            {
                Id = id.ToString(),
                Username = userDetails.FirstName,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
                Email = userDetails.Email
            };

            User user1 = new()
            {
                Aspnetuserid = id.ToString(),
                Firstname = userDetails.PatientFirstName,
                Lastname = userDetails.PatientLastName,
                Email = userDetails.PatientEmail,
                Mobile = userDetails.PatientPhone,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zip = userDetails.ZipCode,
                Strmonth = DateTime.Now.Month.ToString(),
                Intyear = DateTime.Now.Year,
                Intdate = DateTime.Now.Day,
                Createdby = userDetails.FirstName,
                Createddate = DateTime.Now,
                Modifiedby = userDetails.FirstName + userDetails.LastName,
                Modifieddate = DateTime.Now
            };
            Request request = new()
            {
                Requesttypeid = 1,
                //Userid = user.Userid;
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status = 4,
                Createddate = DateTime.Now,
                Isurgentemailsent = true
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            if (userDetails.File != null && userDetails.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.File.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDetails.File.CopyTo(stream);
                }

                Requestwisefile requestwisefile = new()
                {
                    Filename = fileName,
                    Requestid = request.Requestid,
                    Createddate = DateTime.Now
                };

                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            };


            _context.Aspnetusers.Add(user);
            _context.Users.Add(user1);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
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
            Guid id = Guid.NewGuid();
            Aspnetuser user = new()
            {
                Id = id.ToString(),
                Username = userDetails.FirstName,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
                Email = userDetails.Email
            };
            _context.Aspnetusers.Add(user);
            _context.SaveChanges();

            Request request = new()
            {
                Requesttypeid = 1,
                //Userid = user.Userid;
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status = 4,
                Createddate = DateTime.Now,
                Isurgentemailsent = true
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            Region region = new()
            {
                Name = userDetails.City,
            };
            _context.Regions.Add(region);
            _context.SaveChanges();

            Concierge concierge = new()
            {
                Conciergename = userDetails.FirstName + userDetails.LastName,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zipcode = userDetails.ZipCode,
                Createddate = DateTime.Now,
                Regionid = region.Regionid
            };
            _context.Concierges.Add(concierge);
            _context.SaveChanges();

            if (userDetails.File != null && userDetails.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.File.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDetails.File.CopyTo(stream);
                }

                Requestwisefile requestwisefile = new()
                {
                    Filename = fileName,
                    Requestid = request.Requestid,
                    Createddate = DateTime.Now
                };

                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            };
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        //Business Request
        public IActionResult BusinessRequest()
        {
            return View();
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

        [HttpPost]
        public IActionResult BusinessRequest(BusinessSubmitRequest userDetails)
        {
            Guid g = Guid.NewGuid();

            //Aspnetuser user = new()
            //{
            //    Id = g.ToString(),
            //    Username = userDetails.FirstName,
            //    Createddate = DateTime.Now,
            //    Modifieddate = DateTime.Now,
            //    Email = userDetails.Email
            //};
            //_context.Aspnetusers.Add(user);
            //_context.SaveChanges();

            Business business = new()
            {
                Name = userDetails.FirstName + " " + userDetails.LastName,
                Createddate = DateTime.Now

            };
            _context.Businesses.Add(business);
            _context.SaveChanges();


            User user1 = new()
            {
                Aspnetuserid = g.ToString(),
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Mobile = userDetails.PatientPhone,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zip = userDetails.ZipCode,
                //ZipCode = userDetails.Zipcode,
                Createddate = DateTime.Now,
                Createdby = "admin",
            };

            Request request = new()
            {
                Requesttypeid = 1,
                //Userid = user.Userid;
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status = 4,
                Createddate = DateTime.Now,
                Isurgentemailsent = true
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            Requestbusiness requestbusiness = new()
            {
                //requestbusiness.Businessid = business.Id;
                Requestid = request.Requestid,
                Businessid = business.Businessid,

            };

            Requeststatuslog requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 4,
                Createddate = DateTime.Now
            };


            Requesttype requesttype = new()
            {
                Name = userDetails.FirstName + " " + userDetails.LastName
            };


            if (userDetails.File != null && userDetails.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.File.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDetails.File.CopyTo(stream);
                }

                Requestwisefile requestwisefile = new()
                {
                    Filename = fileName,
                    Requestid = request.Requestid,
                    Createddate = DateTime.Now
                };

                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            };

            _context.Requesttypes.Add(requesttype);
            _context.Users.Add(user1);
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.Requestbusinesses.Add(requestbusiness);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}