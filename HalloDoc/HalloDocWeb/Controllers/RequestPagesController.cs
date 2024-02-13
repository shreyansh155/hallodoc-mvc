
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

        public RequestPagesController(ApplicationDbContext context)
        {
            _context = context;
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
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Mobile = userDetails.Phone,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zip = userDetails.ZipCode,
                Strmonth = DateTime.Now.Month.ToString(),
                Intyear = DateTime.Now.Year,
                Intdate = DateTime.Now.Day,
                Createdby = userDetails.FirstName,
                Createddate = DateTime.Now,
                Modifiedby = userDetails.FirstName,
                Modifieddate = DateTime.Now
            };

            Request request = new()
            {
                Requesttypeid=2,
                Userid=user1.Userid,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status=1, //To be dynamically assigned when admin side is created
                Phonenumber = userDetails.Phone,
                Createddate = DateTime.Now,
                
            };

            //uploading files
            if (userDetails.FileUpload != null && userDetails.FileUpload.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.FileUpload.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDetails.FileUpload.CopyTo(stream);
                }
                Requestwisefile requestwisefile = new()
                {
                    Filename = fileName,
                    Requestid = request.Requestid,
                    Createddate = DateTime.Now
                };

                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }

            _context.Aspnetusers.Add(user);
            _context.Users.Add(user1);
            _context.Requests.Add(request);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
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
            Guid id = new();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConciergeRequest(ConciergeSubmitRequest userDetails)
        {
            Guid id = new();
            Aspnetuser user = new()
            {
                Id = id.ToString(),
                Username = userDetails.FirstName,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
                Email = userDetails.Email
            };
            Region region = new()
            {
                Regionid = 1,
                Name = userDetails.City,
            };
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
            _context.Aspnetusers.Add(user);
            _context.Regions.Add(region);
            _context.Concierges.Add(concierge);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        //Business Request
        public IActionResult BusinessRequest()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BusinessRequest(BusinessSubmitRequest userDetails)
        {
            Guid g = Guid.NewGuid();

            Aspnetuser user = new()
            {
                Id = g.ToString(),
                Username = userDetails.FirstName,
                Createddate = DateTime.Now,
                Modifieddate = DateTime.Now,
                Email = userDetails.Email
            };
            _context.Aspnetusers.Add(user);
            _context.SaveChanges();

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
                Isurgentemailsent=true
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            Requestbusiness requestbusiness = new()
            {
                //requestbusiness.Businessid = business.Id;
                Requestid = request.Requestid,
                Businessid=business.Businessid,
                
            };

            Requeststatuslog requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 4,
                Createddate = DateTime.Now
            };


            Requesttype requesttype = new()
            {
                Name=userDetails.FirstName+" "+ userDetails.LastName
            };


            if (userDetails.FileUpload != null && userDetails.FileUpload.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.FileUpload.FileName);
                 
                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDetails.FileUpload.CopyTo(stream);
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


            //Requestclient requestclient = new()
            //{ 
            //    Requestid = request.Requestid,
            //    Notes = userDetails.Symptoms,
            //    Firstname = userDetails.PatientFirstName,
            //    Lastname = userDetails.PatientLastName,
            //    Phonenumber = userDetails.PatientPhone,
            //    Email = userDetails.PatientEmail,
            //};
            //_context.Requestclients.Add(requestclient);


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