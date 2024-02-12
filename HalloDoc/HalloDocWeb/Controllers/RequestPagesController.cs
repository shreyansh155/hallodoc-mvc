
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

            //Request request = new()
            //{
            //    Requesttypeid=2,
            //    Userid=user1.Userid,
            //    Firstname = userDetails.FirstName,
            //    Lastname = userDetails.LastName,
            //    Email = userDetails.Email,
            //    Status=1, //To be dynamically assigned when admin side is created
            //    Phonenumber = userDetails.Phone,
            //    Createddate = DateTime.Now,


            //};


            _context.Aspnetusers.Add(user);
            _context.Users.Add(user1);
            //_context.Requests.Add(request);
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

        public IActionResult BusinessRequest()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BusinessRequest(BusinessSubmitRequest userDetails)
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

            Business business = new()
            {
                Name = userDetails.FirstName + " " + userDetails.LastName,
                Createdby = DateTime.Now.ToString()

            };

            User user1 = new()
            {
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
                //request.Userid = user.Userid;
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status = 4,
                Createddate = DateTime.Now
            };
            //request.Isurgentemailsent = false;

            Requestbusiness requestbusiness = new()
            {
                //requestbusiness.Businessid = business.Id;
                Requestid = request.Requestid
            };
            _context.Requestbusinesses.Add(requestbusiness);

            Requeststatuslog requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 4,
                Createddate = DateTime.Now
            };


            _context.Aspnetusers.Add(user);
            _context.Users.Add(user1);
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.Requests.Add(request);
            _context.Businesses.Add(business);
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