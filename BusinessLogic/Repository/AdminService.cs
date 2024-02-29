using BusinessLogic.Interface;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.DataContext;
using System.Security.Cryptography;
using DataAccess.DataModels;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
namespace BusinessLogic.Repository
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;


        public AdminService(ApplicationDbContext context)
        {
            _context = context;

        }
        public static string GenerateSHA256(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hashEngine = SHA256.Create();
            var hashedBytes = hashEngine.ComputeHash(bytes, 0, bytes.Length);
            var sb = new StringBuilder();
            foreach (var b in hashedBytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
        public bool AdminLogin(AdminLogin adminLogin)
        {
            string hashPassword = GenerateSHA256(adminLogin.Password); ;
            return _context.Aspnetusers.Any(Au => Au.Email == adminLogin.Email && Au.Passwordhash == hashPassword);
        }
        public void CreateAdminAccount(CreateAdminAccount newAccount)
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
                Admin admin = new()
                {
                    Aspnetuserid = aspnetuser.Id,
                    Firstname = newAccount.UserName,
                    Createdby = "admin",
                    Createddate = DateTime.Now,
                    Email = newAccount.Email,

                };
                _context.Admins.Add(admin);
                _context.SaveChanges();
            }
        }
        public AdminDashboard AdminDashboard()
        {


            var newCount = (from t1 in _context.Requests
                            where t1.Status == 1
                            select t1
                            ).Count();
            var pendingCount = (from t1 in _context.Requests
                                where t1.Status == 2
                                select t1
                            ).Count();
            var activeCount = (from t1 in _context.Requests
                               where t1.Status == 8
                               select t1
                            ).Count();
            var concludeCount = (from t1 in _context.Requests
                                 where t1.Status == 4
                                 select t1
                            ).Count();
            var closeCount = (from t1 in _context.Requests
                              where t1.Status == 5
                              select t1
                            ).Count();
            var unpaidCount = (from t1 in _context.Requests
                               where t1.Status == 13
                               select t1
                            ).Count();
            var count = new CountRequestViewModel
            {
                newCount = newCount,
                pendingCount = pendingCount,
                activeCount = activeCount,
                concludeCount = concludeCount,
                closeCount = closeCount,
                unpaidCount = unpaidCount,
                RegionList = _context.Regions.ToList()
            };

            var newReqData = (from req in _context.Requests
                              join rc in _context.Requestclients on req.Requestid equals rc.Requestid
                              where req.Status == 1
                              select new NewReqViewModel
                              {
                                  reqClientId = rc.Requestclientid,
                                  Firstname = rc.Firstname,
                                  Lastname = rc.Lastname,
                                  reqFirstname = req.Firstname,
                                  reqLastname = req.Lastname,
                                  Strmonth = rc.Strmonth,
                                  Createddate = req.Createddate,
                                  Phonenumber = rc.Phonenumber,
                                  ConciergePhonenumber = req.Phonenumber,
                                  FamilyPhonenumber = req.Phonenumber,
                                  BusinessPhonenumber = req.Phonenumber,
                                  Street = rc.Street,
                                  City = rc.City,
                                  State = rc.State,
                                  Zipcode = rc.Zipcode,
                                  Notes = rc.Notes,
                                  reqTypeId = req.Requesttypeid,
                                  Casetags = _context.Casetags.ToList(),
                              });
            var pendingReqData = from req in _context.Requests
                                 join rc in _context.Requestclients on req.Requestid equals rc.Requestid
                                 join phy in _context.Physicians on req.Physicianid equals phy.Physicianid
                                 where req.Status == 2
                                 select new PendingReqViewModel
                                 {
                                     reqClientId = rc.Requestclientid,
                                     Firstname = rc.Firstname,
                                     Lastname = rc.Lastname,
                                     reqFirstname = req.Firstname,
                                     reqLastname = req.Lastname,
                                     Strmonth = rc.Strmonth,
                                     Createddate = req.Createddate,
                                     Phonenumber = rc.Phonenumber,
                                     ConciergePhonenumber = req.Phonenumber,
                                     FamilyPhonenumber = req.Phonenumber,
                                     BusinessPhonenumber = req.Phonenumber,
                                     Street = rc.Street,
                                     City = rc.City,
                                     State = rc.State,
                                     Zipcode = rc.Zipcode,
                                     Notes = rc.Notes,
                                     reqTypeId = req.Requesttypeid,
                                     physicianName = phy.Firstname + " " + phy.Lastname
                                 };
            var closeReqData = from req in _context.Requests
                               join rc in _context.Requestclients on req.Requestid equals rc.Requestid
                               join phy in _context.Physicians on req.Physicianid equals phy.Physicianid
                               where req.Status == 5
                               select new CloseReqViewModel
                               {
                                   reqClientId = rc.Requestclientid,
                                   Firstname = rc.Firstname,
                                   Lastname = rc.Lastname,
                                   reqFirstname = req.Firstname,
                                   reqLastname = req.Lastname,
                                   Strmonth = rc.Strmonth,
                                   Createddate = req.Createddate,
                                   Phonenumber = rc.Phonenumber,
                                   ConciergePhonenumber = req.Phonenumber,
                                   FamilyPhonenumber = req.Phonenumber,
                                   BusinessPhonenumber = req.Phonenumber,
                                   Street = rc.Street,
                                   City = rc.City,
                                   State = rc.State,
                                   Zipcode = rc.Zipcode,
                                   Notes = rc.Notes,
                                   reqTypeId = req.Requesttypeid,
                                   physicianName = phy.Firstname + " " + phy.Lastname
                               };
            var concludeReqData = from req in _context.Requests
                                  join rc in _context.Requestclients on req.Requestid equals rc.Requestid
                                  join phy in _context.Physicians on req.Physicianid equals phy.Physicianid
                                  where req.Status == 4
                                  select new ConcludeReqViewModel
                                  {
                                      reqClientId = rc.Requestclientid,
                                      Firstname = rc.Firstname,
                                      Lastname = rc.Lastname,
                                      reqFirstname = req.Firstname,
                                      reqLastname = req.Lastname,
                                      Strmonth = rc.Strmonth,
                                      Createddate = req.Createddate,
                                      Phonenumber = rc.Phonenumber,
                                      ConciergePhonenumber = req.Phonenumber,
                                      FamilyPhonenumber = req.Phonenumber,
                                      BusinessPhonenumber = req.Phonenumber,
                                      Street = rc.Street,
                                      City = rc.City,
                                      State = rc.State,
                                      Zipcode = rc.Zipcode,
                                      Notes = rc.Notes,
                                      reqTypeId = req.Requesttypeid,
                                      physicianName = phy.Firstname + " " + phy.Lastname
                                  };
            var unpaidReqData = from req in _context.Requests
                                join rc in _context.Requestclients on req.Requestid equals rc.Requestid
                                join phy in _context.Physicians on req.Physicianid equals phy.Physicianid
                                where req.Status == 13
                                select new UnpaidReqViewModel
                                {
                                    reqClientId = rc.Requestclientid,
                                    Firstname = rc.Firstname,
                                    Lastname = rc.Lastname,
                                    reqFirstname = req.Firstname,
                                    reqLastname = req.Lastname,
                                    Strmonth = rc.Strmonth,
                                    Createddate = req.Createddate,
                                    Phonenumber = rc.Phonenumber,
                                    ConciergePhonenumber = req.Phonenumber,
                                    FamilyPhonenumber = req.Phonenumber,
                                    BusinessPhonenumber = req.Phonenumber,
                                    Street = rc.Street,
                                    City = rc.City,
                                    State = rc.State,
                                    Zipcode = rc.Zipcode,
                                    Notes = rc.Notes,
                                    reqTypeId = req.Requesttypeid,
                                    physicianName = phy.Firstname + " " + phy.Lastname
                                };
            var activeReqData = from req in _context.Requests
                                join rc in _context.Requestclients on req.Requestid equals rc.Requestid
                                join phy in _context.Physicians on req.Physicianid equals phy.Physicianid
                                where req.Status == 8
                                select new ActiveReqViewModel
                                {
                                    reqClientId = rc.Requestclientid,
                                    Firstname = rc.Firstname,
                                    Lastname = rc.Lastname,
                                    reqFirstname = req.Firstname,
                                    reqLastname = req.Lastname,
                                    Strmonth = rc.Strmonth,
                                    Createddate = req.Createddate,
                                    Phonenumber = rc.Phonenumber,
                                    ConciergePhonenumber = req.Phonenumber,
                                    FamilyPhonenumber = req.Phonenumber,
                                    BusinessPhonenumber = req.Phonenumber,
                                    Street = rc.Street,
                                    City = rc.City,
                                    State = rc.State,
                                    Zipcode = rc.Zipcode,
                                    Notes = rc.Notes,
                                    reqTypeId = req.Requesttypeid,
                                    physicianName = phy.Firstname + " " + phy.Lastname
                                };




            var data = new AdminDashboard
            {
                CountRequestViewModel = count,
                NewReqViewModel = newReqData,
                ConcludeReqViewModel = concludeReqData,
                CloseReqViewModels = closeReqData,
                ActiveReqViewModels = activeReqData,
                PendingReqViewModel = pendingReqData,
                UnpaidReqViewModels = unpaidReqData,
            };
            return data;
        }
        public ViewCaseViewModel ViewCaseViewModel(int reqClientId)
        {
            Requestclient obj = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            ViewCaseViewModel viewCaseViewModel = new()
            {
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Phonenumber = obj.Phonenumber,
                Notes = obj.Notes,
                Street = obj.Street,
                City = obj.City,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Room = obj.Address,
                DateOfBirth = new DateTime((int)obj.Intyear, DateTime.ParseExact(obj.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, (int)obj.Intdate),
                Requestclientid = reqClientId,
            };
            return viewCaseViewModel;
        }
        public ViewNotes ViewNotes(int reqClientId)
        {
            Requestclient req = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            Requestnote obj = _context.Requestnotes.FirstOrDefault(x => x.Requestid == req.Requestid);
            Physician physician = _context.Physicians.First(x => x.Physicianid == 1);
            var requeststatuslog = _context.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).ToList();


            ViewNotes viewNote = new()
            {
                PhysicianName = physician.Firstname,
                AdminNotes = obj.Adminnotes,
                PhysicianNotes = obj.Physiciannotes,
                Statuslogs = requeststatuslog,
            };
            return viewNote;
        }
        public AdminDashboard SearchPatient(SearchViewModel obj, AdminDashboard data)
        {
            if (obj.Name == null)
            {
                return data;
            }
            else
            {
                var name = obj.Name.ToUpper();
                var sortedNew = data.NewReqViewModel.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedConclude = data.ConcludeReqViewModel.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedClose = data.CloseReqViewModels.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedPending = data.PendingReqViewModel.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedUnpaid = data.UnpaidReqViewModels.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));
                var sortedActive = data.ActiveReqViewModels.Where(s => s.Firstname.ToUpper().Contains(name) || s.Lastname.ToUpper().Contains(name) || s.reqFirstname.ToUpper().Contains(name) || s.reqLastname.ToUpper().Contains(name));

                data.NewReqViewModel = sortedNew;
                data.ConcludeReqViewModel = sortedConclude;
                data.CloseReqViewModels = sortedClose;
                data.PendingReqViewModel = sortedPending;
                data.ActiveReqViewModels = sortedActive;
                data.UnpaidReqViewModels = sortedUnpaid;

                return data;
            }
        }
    }
}