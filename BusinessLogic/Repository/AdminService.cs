using BusinessLogic.Interface;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string hashPassword = GenerateSHA256(adminLogin.Password);
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
                unpaidCount = unpaidCount
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
                                  reqTypeId = req.Requesttypeid
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
                UnpaidReqViewModels = unpaidReqData

            };
            return data;
        }

        public ViewCaseViewModel ViewCase(int reqClientId)
        {
            var data = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            var cNumber = _context.Requests.FirstOrDefault(x => x.Requestid == data.Requestid);
            var user = _context.Users.FirstOrDefault(x => x.Userid == cNumber.Userid);
            var confirm = cNumber.Confirmationnumber;

            var viewdata = new ViewCaseViewModel
            {
                Requestclientid = reqClientId,
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Strmonth = data.Strmonth,
                ConfirmationNumber = confirm,
                Notes = data.Notes,
                Address = data.Address,
                Email = data.Email,
                Phonenumber = data.Phonenumber,
                City = user.City,
                State = user.State,
                Street = user.Street,
                Zipcode = user.Zip,
                Regionid = data.Regionid,
                

            };

            return viewdata;
        }

        public bool ViewCase(ViewCaseViewModel obj)
        {
            try
            {
                var rcId = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == obj.Requestclientid);
                var rId = rcId.Requestid;
                var rRow = _context.Requests.FirstOrDefault(x => x.Requestid == rId);
                var uid = rRow.Userid;
                var uRow = _context.Users.FirstOrDefault(x => x.Userid == uid);
                var aspId = uRow.Aspnetuserid;
                var aspRow = _context.Aspnetusers.FirstOrDefault(x => x.Id == aspId);

                var email = aspRow.Email;


                uRow.Firstname = obj.Firstname;
                uRow.Lastname = obj.Lastname;
                uRow.Email = obj.Email;
                uRow.Mobile = obj.Phonenumber;
                uRow.Strmonth = obj.Strmonth;

                _context.Users.Update(uRow);
                _context.SaveChanges();


                aspRow.Email = obj.Email;
                aspRow.Username = obj.Email;
                aspRow.Phonenumber = obj.Phonenumber;
                aspRow.Modifieddate = DateTime.UtcNow;

                _context.Aspnetusers.Update(aspRow);
                _context.SaveChanges();

                _context.Requestclients.Where(x => x.Email == email).ToList().ForEach(item =>
                {
                    item.Firstname = obj.Firstname;
                    item.Lastname = obj.Lastname;
                    item.Email = obj.Email;
                    item.Phonenumber = obj.Phonenumber;
                    item.Address = obj.Address;
                    item.Strmonth = obj.Strmonth;

                    _context.Requestclients.Update(item);
                    _context.SaveChanges();
                });

                _context.Requests.Where(x => x.Email == email).ToList().ForEach(item =>
                {
                    item.Firstname = obj.Firstname;
                    item.Lastname = obj.Lastname;
                    item.Email = obj.Email;
                    item.Phonenumber = obj.Phonenumber;
                    item.Modifieddate = DateTime.UtcNow;
                    _context.Requests.Update(item);
                    _context.SaveChanges();
                });

                return true;
            }
            catch
            {
                return false;
            }
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
            };
            return viewCaseViewModel;
        }
        public ViewNotes ViewNotes(int reqClientId)
        {
            Requestclient req = _context.Requestclients.FirstOrDefault(x => x.Requestclientid== reqClientId);
            Requestnote obj = _context.Requestnotes.FirstOrDefault(x => x.Requestid==req.Requestid);
            ViewNotes viewNote = new() 
            {
                AdminNotes=obj.Adminnotes,
                PhysicianNotes=obj.Physiciannotes,
            };
            return viewNote;
        }
    }
}