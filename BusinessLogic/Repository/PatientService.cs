using BusinessLogic.Interface;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace BusinessLogic.Repository
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PatientService(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _environment = webHostEnvironment;
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

        public List<PatientDashboard> GetMedicalHistory(User user)
        {
            var medicalhistory = (from request in _context.Requests
                                  join requestfile in _context.Requestwisefiles
                                  on request.Requestid equals requestfile.Requestid
                                  where request.Email == user.Email && request.Email != null
                                  group requestfile by request.Requestid into groupedFiles
                                  select new PatientDashboard
                                  {
                                      FirstName = user.Firstname,
                                      reqId = groupedFiles.Select(x => x.Request.Requestid).FirstOrDefault(),
                                      Createddate = groupedFiles.Select(x => x.Request.Createddate).FirstOrDefault(),
                                      Status = groupedFiles.Select(x => x.Request.Status).FirstOrDefault().ToString(),
                                      File = groupedFiles.Select(x => x.Filename.ToString()).ToList()
                                  }).ToList();
            List<int> fileCount = new();
            for (int i = 0; i < medicalhistory.Count; i++)
            {
                int count = _context.Requestwisefiles.Count(rf => rf.Requestid == medicalhistory[i].reqId);
                fileCount.Add(count);
            }
            return medicalhistory;
        }

        public List<PatientDashboard> GetPatientInfos()
        {
            return new List<PatientDashboard> { };
        }

        public void PatientRequest(PatientSubmitRequest userDetails)
        {
            Guid id = Guid.NewGuid();


            var user = _context.Aspnetusers.Where(x => x.Email == userDetails.Email).FirstOrDefault();

            Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == userDetails.Email);
            if (obj == null)
            {
                if (userDetails.Password == userDetails.ConfirmPassword)
                {
                    Aspnetuser aspnetuser = new()
                    {
                        Id = id.ToString(),
                        Username = userDetails.FirstName,
                        Passwordhash = userDetails.Password,
                        Createddate = DateTime.Now,
                        Email = userDetails.Email,
                        Phonenumber = userDetails.Phone,

                    };
                    _context.Aspnetusers.Add(aspnetuser);
                    _context.SaveChanges();
                    user = aspnetuser;
                }

            }

            User user1 = new()
            {
                Aspnetuserid = user.Id,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Mobile = userDetails.Phone,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zip = userDetails.ZipCode,
                Createdby = "admin",
                Createddate = DateTime.Now,
                Modifiedby = userDetails.FirstName,
                Modifieddate = DateTime.Now
            };
            _context.Users.Add(user1);
            _context.SaveChanges();
            
            Requesttype requesttype = new()
            {
                Name = userDetails.FirstName + " " + userDetails.LastName
            };
            _context.Requesttypes.Add(requesttype);
            _context.SaveChanges();

            Request request = new()
            {
                Requesttypeid = requesttype.Requesttypeid,
                Userid = user1.Userid,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status = 4,
                Createddate = DateTime.Now,
                Isurgentemailsent = true
            };
            _context.Requests.Add(request);
                _context.SaveChanges();


            Requeststatuslog requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 4,
                Createddate = DateTime.Now
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();




            //uploading files
            if (userDetails.File != null && userDetails.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.File.FileName);

                string rootPath = _environment.WebRootPath + "/UploadedFiles";


                string userId = user.Id;

                string userFolder = Path.Combine(rootPath, userId);

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }

                
                //define path
                string filePath = Path.Combine(userFolder, fileName);

                
                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    userDetails.File.CopyTo(stream)
;
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

            _context.SaveChanges();

        }
        public void FamilyFriendRequest(FamilyFriendSubmitRequest familyFriendSubmitRequest)
        {
        }
        public void ConciergeRequest(ConciergeSubmitRequest conciergeSubmitRequest) { }
        public void BusinessRequest(BusinessSubmitRequest businessSubmitRequest) { }

    }
}
