using BusinessLogic.Interface;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;





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




    }
}