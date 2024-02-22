using BusinessLogic.Interface;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;





namespace BusinessLogic.Repository
{
    public class AuthService : IAuthService
    {

        private readonly DataAccess.DataContext.ApplicationDbContext _context;


        public AuthService(DataAccess.DataContext.ApplicationDbContext db)
        {
            _context = db;

        }
        //Generate Password Hash function
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

        public bool PatientLogin(PatientLogin patientLogin)
        {
            string hashPassword = GenerateSHA256(patientLogin.Password);
            return _context.Aspnetusers.Any(Au => Au.Email == patientLogin.Email && Au.Passwordhash == hashPassword);
        }

        public bool PatientForgotPassword(PatientForgotPassword patientForgotPassword)
        {
            Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == patientForgotPassword.Email);

            var user = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == patientForgotPassword.Email);
            return _context.Aspnetusers.Any(x => x.Email == patientForgotPassword.Email);

        }
        public void PatientResetPassword(PatientResetPassword patientResetPassword)
        {
            if (patientResetPassword.Password == patientResetPassword.ConfirmPassword)
            {
                Aspnetuser user = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == patientResetPassword.Email);
                string hashPassword = GenerateSHA256(patientResetPassword.Password);
                user.Passwordhash = hashPassword;
                _context.Aspnetusers.Update(user);
                _context.SaveChanges();
            }
        }

        public void CreateNewAccount(CreateNewAccount newAccount)
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
            }
        }
    }
}