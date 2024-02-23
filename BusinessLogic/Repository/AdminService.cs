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

        public List<AdminDashboard> GetRequestsByStatus()
        {
            var query = from r in _context.Requests
                        join rc in _context.Requestclients on r.Requestid equals rc.Requestid
                        select new AdminDashboard
                        {
                            firstName = rc.Firstname,
                            lastName = rc.Lastname,
                            intDate = rc.Intdate,
                            intYear = rc.Intyear,
                            strMonth = rc.Strmonth,
                            requestorFname = r.Firstname,
                            requestorLname = r.Lastname,
                            createdDate = r.Createddate,
                            mobileNo = rc.Phonenumber,
                            city = rc.City,
                            state = rc.State,
                            street = rc.Street,
                            zipCode = rc.Zipcode,
                            requestTypeId = r.Requesttypeid,
                            status = r.Status,

                        };

            var result = query.ToList();


            return result;
        }
    }
}
