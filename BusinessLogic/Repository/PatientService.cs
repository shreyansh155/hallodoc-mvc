using BusinessLogic.Interface;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;


namespace BusinessLogic.Repository
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public PatientService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
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
            var medicalhistory = new List<PatientDashboard>();
            //var medicalhistory = (from request in _context.Requests
            //                      join requestfile in _context.Requestwisefiles
            //                      on request.Requestid equals requestfile.Requestid
            //                      where request.Email == user.Email && request.Email != null
            //                      group requestfile by request.Requestid into groupedFiles
            //                      select new PatientDashboard
            //                      {
            //                          FirstName = user.Firstname,
            //                          reqId = groupedFiles.Select(x => x.Request.Requestid).FirstOrDefault(),
            //                          Createddate = groupedFiles.Select(x => x.Request.Createddate).FirstOrDefault(),
            //                          Status = groupedFiles.Select(x => x.Request.Status).FirstOrDefault().ToString(),
            //                          File = groupedFiles.Select(x => x.Filename.ToString()).ToList()
            //                      }).ToList();
            List<int> fileCount = new();
            //for (int i = 0; i < medicalhistory.Count; i++)
            //{
            //    int count = _context.Requestwisefiles.Count(rf => rf.Requestid == medicalhistory[i].reqId);
            //    fileCount.Add(count);
            //}
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
                        Passwordhash = GenerateSHA256(userDetails.Password),
                        Createddate = DateTime.Now,
                        Email = userDetails.Email,
                        Phonenumber = userDetails.Phone,

                    };
                    _context.Aspnetusers.Add(aspnetuser);
                    _context.SaveChanges();
                    user = aspnetuser;

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
                        Strmonth = userDetails.DateOfBirth.ToString("MMMM"),
                        Intdate = userDetails.DateOfBirth.Day,
                        Intyear = userDetails.DateOfBirth.Year,
                    };
                    _context.Users.Add(user1);
                    _context.SaveChanges();

                    Request request = new()
                    {
                        Requesttypeid = 1,
                        Userid = user1.Userid,
                        Firstname = userDetails.FirstName,
                        Lastname = userDetails.LastName,
                        Email = userDetails.Email,
                        Status = 1,
                        Physicianid = 1,
                        Createddate = DateTime.Now,
                        Isurgentemailsent = true
                    };
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    Requestclient requestclient = new()
                    {

                        Requestid = request.Requestid,
                        Firstname = userDetails.FirstName,
                        Lastname = userDetails.LastName,
                        Email = userDetails.Email,
                        Phonenumber = userDetails.Phone,
                        Notes = userDetails.Symptoms,
                        Strmonth = userDetails.DateOfBirth.ToString("MMM"),
                        Intdate = userDetails.DateOfBirth.Day,
                        Intyear = userDetails.DateOfBirth.Year,
                        Street = userDetails.Street,
                        City = userDetails.City,
                        State = userDetails.State,
                        Zipcode = userDetails.ZipCode,

                    };
                    _context.Requestclients.Add(requestclient);
                    _context.SaveChanges();
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
                }
            }
            else
            {
                User user2 = _context.Users.First(x => x.Email == userDetails.Email);

                Request request = new()
                {
                    Requesttypeid = 1,
                    Userid = user2.Userid,
                    Firstname = userDetails.FirstName,
                    Lastname = userDetails.LastName,
                    Email = userDetails.Email,
                    Status = 1,
                    Physicianid = 1,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = true
                };
                _context.Requests.Add(request);
                _context.SaveChanges();

                Requestclient requestclient = new()
                {

                    Requestid = request.Requestid,
                    Firstname = userDetails.FirstName,
                    Lastname = userDetails.LastName,
                    Email = userDetails.Email,
                    Phonenumber = userDetails.Phone,
                    Notes = userDetails.Symptoms,
                    Strmonth = userDetails.DateOfBirth.ToString("MMM"),
                    Intdate = userDetails.DateOfBirth.Day,
                    Intyear = userDetails.DateOfBirth.Year,
                    Street = userDetails.Street,
                    City = userDetails.City,
                    State = userDetails.State,
                    Zipcode = userDetails.ZipCode,

                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();

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
            }

            _context.SaveChanges();

        }
        public void FamilyFriendRequest(FamilyFriendSubmitRequest userDetails)
        {
            Guid id = Guid.NewGuid();
            var user = _context.Aspnetusers.Where(x => x.Email == userDetails.Email).FirstOrDefault();


            //patient credentials goes into aspnetuser table, if it is null, an instance is created for it. password is added when the patient create their account from mail.
            Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == userDetails.PatientEmail);
            if (obj == null)
            {
                Aspnetuser aspnetuser = new()
                {
                    Id = id.ToString(),
                    Username = userDetails.PatientFirstName,
                    Createddate = DateTime.Now,
                    Email = userDetails.PatientFirstName,
                    Phonenumber = userDetails.PatientPhone,

                };
                _context.Aspnetusers.Add(aspnetuser);
                _context.SaveChanges();
                obj = aspnetuser;
            }
            //patient details goes into Users table
            User user1 = new()
            {
                Aspnetuserid = obj.Id,
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
            _context.Users.Add(user1);
            _context.SaveChanges();


            Request request = new()
            {
                Requesttypeid = 2,
                Userid = user1.Userid,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Status = 1,
                Physicianid = 1,

                Createddate = DateTime.Now,
                Isurgentemailsent = true
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            Requestclient requestclient = new()
            {
                Requestid = request.Requestid,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Phonenumber = userDetails.Phone,
                Notes = userDetails.Symptoms,
                Strmonth = userDetails.DateOfBirth.ToString("MMM"),
                Intdate = userDetails.DateOfBirth.Day,
                Intyear = userDetails.DateOfBirth.Year,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zipcode = userDetails.ZipCode,

            };
            _context.Requestclients.Add(requestclient);
            _context.SaveChanges();

            if (userDetails.File != null && userDetails.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.File.FileName);

                string rootPath = _environment.WebRootPath + "/UploadedFiles";

                string userId = user1.Userid.ToString();

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
        public void ConciergeRequest(ConciergeSubmitRequest userDetails)
        {
            Guid id = Guid.NewGuid();
            Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == userDetails.PatientEmail);
            if (obj == null)
            {
                if (userDetails.Password == userDetails.ConfirmPassword)
                {
                    Aspnetuser aspnetuser = new()
                    {
                        Id = id.ToString(),
                        Username = userDetails.PatientFirstName,
                        Createddate = DateTime.Now,
                        Email = userDetails.PatientFirstName,
                        Phonenumber = userDetails.PatientPhone,

                    };
                    _context.Aspnetusers.Add(aspnetuser);
                    _context.SaveChanges();
                    obj = aspnetuser;
                    User user1 = new()
                    {
                        Aspnetuserid = obj.Id,
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
                    _context.Users.Add(user1);
                    _context.SaveChanges();
            

                    Request request = new()
                    {
                        Requesttypeid = 3,
                        Firstname = userDetails.FirstName,
                        Lastname = userDetails.LastName,
                        Email = userDetails.Email,
                        Status = 1,
                        Physicianid = 1,
                        Createddate = DateTime.Now,
                        Isurgentemailsent = true
                    };
                    _context.Requests.Add(request);
                    _context.SaveChanges();

                    Concierge concierge = new()
                    {
                        Conciergename = userDetails.FirstName + userDetails.LastName,
                        Street = userDetails.Street,
                        City = userDetails.City,
                        State = userDetails.State,
                        Zipcode = userDetails.ZipCode,
                        Createddate = DateTime.Now,
                        Regionid=1,
                    };
                    _context.Concierges.Add(concierge);
                    _context.SaveChanges();
                    Requestconcierge requestconcierge = new()
                    {
                        Requestid = request.Requestid,
                        Conciergeid = concierge.Conciergeid,

                    };
                    _context.Requestconcierges.Add(requestconcierge);
                    _context.SaveChanges();

                    Requestclient requestclient = new()
                    {
                        Requestid = request.Requestid,
                        Firstname = userDetails.FirstName,
                        Lastname = userDetails.LastName,
                        Email = userDetails.Email,
                        Phonenumber = userDetails.Phone,
                        Notes = userDetails.Symptoms,
                        Strmonth = userDetails.DateOfBirth.ToString("MMMM"),
                        Intdate = userDetails.DateOfBirth.Day,
                        Intyear = userDetails.DateOfBirth.Year,
                        Street = userDetails.Street,
                        City = userDetails.City,
                        State = userDetails.State,
                        Zipcode = userDetails.ZipCode,

                    };
                    _context.Requestclients.Add(requestclient);
                    _context.SaveChanges();

                    if (userDetails.File != null && userDetails.File.Length > 0)
                    {
                        //get file name
                        var fileName = Path.GetFileName(userDetails.File.FileName);

                        string rootPath = _environment.WebRootPath + "/UploadedFiles";

                        string userId = user1.Userid.ToString();

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
            }
            else
            {
                var user = _context.Aspnetusers.Where(x => x.Email == userDetails.Email).FirstOrDefault();


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
                    Strmonth = userDetails.DateOfBirth.ToString("MMMM"),
                    Intdate = userDetails.DateOfBirth.Day,
                    Intyear = userDetails.DateOfBirth.Year,
                };

                _context.Users.Add(user1);
                _context.SaveChanges();


                Request request = new()
                {
                    Requesttypeid = 3,
                    Userid = user1.Userid,
                    Firstname = userDetails.FirstName,
                    Lastname = userDetails.LastName,
                    Email = userDetails.Email,
                    Status = 1,
                    Physicianid = 1,
                    Createddate = DateTime.Now,
                    Isurgentemailsent = true
                };
                _context.Requests.Add(request);
                _context.SaveChanges();

                Concierge concierge = new()
                {
                    Conciergename = userDetails.FirstName + userDetails.LastName,
                    Street = userDetails.Street,
                    City = userDetails.City,
                    State = userDetails.State,
                    Zipcode = userDetails.ZipCode,
                    Createddate = DateTime.Now
                };
                _context.Concierges.Add(concierge);
                _context.SaveChanges();
                Requestconcierge requestconcierge = new()
                {
                    Requestid = request.Requestid,
                    Conciergeid = concierge.Conciergeid,

                };
                _context.Requestconcierges.Add(requestconcierge);
                _context.SaveChanges();

                Requestclient requestclient = new()
                {
                    Requestid = request.Requestid,
                    Firstname = userDetails.FirstName,
                    Lastname = userDetails.LastName,
                    Email = userDetails.Email,
                    Phonenumber = userDetails.Phone,
                    Notes = userDetails.Symptoms,
                    Strmonth = userDetails.DateOfBirth.ToString("MMMM"),
                    Intdate = userDetails.DateOfBirth.Day,
                    Intyear = userDetails.DateOfBirth.Year,
                    Street = userDetails.Street,
                    City = userDetails.City,
                    State = userDetails.State,
                    Zipcode = userDetails.ZipCode,

                };
                _context.Requestclients.Add(requestclient);
                _context.SaveChanges();

                if (userDetails.File != null && userDetails.File.Length > 0)
                {
                    //get file name
                    var fileName = Path.GetFileName(userDetails.File.FileName);

                    string rootPath = _environment.WebRootPath + "/UploadedFiles";

                    string userId = user1.Userid.ToString();

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
        }
        public void BusinessRequest(BusinessSubmitRequest userDetails)
        {
            Guid id = Guid.NewGuid();

            Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(rq => rq.Email == userDetails.PatientEmail);
            if (obj == null)
            {
                Aspnetuser aspnetuser = new()
                {
                    Id = id.ToString(),
                    Username = userDetails.PatientFirstName,
                    Createddate = DateTime.Now,
                    Email = userDetails.PatientFirstName,
                    Phonenumber = userDetails.PatientPhone,

                };
                _context.Aspnetusers.Add(aspnetuser);
                _context.SaveChanges();
                obj = aspnetuser;
            }

            Business business = new()
            {
                Name = userDetails.FirstName + " " + userDetails.LastName,
                Createddate = DateTime.Now

            };
            _context.Businesses.Add(business);
            _context.SaveChanges();


            User user1 = new()
            {
                Aspnetuserid = obj.Id,
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
            _context.Users.Add(user1);
            _context.SaveChanges();


            Request request = new()
            {

                Requesttypeid = 4,
                Userid = user1.Userid,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Physicianid = 1,
                Status = 1,
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
            _context.Requestbusinesses.Add(requestbusiness);
            _context.SaveChanges();

            Requestclient requestclient = new()
            {
                Requestid = request.Requestid,
                Firstname = userDetails.FirstName,
                Lastname = userDetails.LastName,
                Email = userDetails.Email,
                Phonenumber = userDetails.Phone,
                Notes = userDetails.Symptoms,
                Strmonth = userDetails.PatientDateOfBirth.ToString("MMM"),
                Intdate = userDetails.PatientDateOfBirth.Day,
                Intyear = userDetails.PatientDateOfBirth.Year,
                Street = userDetails.Street,
                City = userDetails.City,
                State = userDetails.State,
                Zipcode = userDetails.ZipCode,

            };
            _context.Requestclients.Add(requestclient);
            _context.SaveChanges();
            if (userDetails.File != null && userDetails.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(userDetails.File.FileName);

                string rootPath = _environment.WebRootPath + "/UploadedFiles";

                string userId = user1.Userid.ToString();

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
        public void ViewDocument(ViewDocument viewDocument)
        {
            if (viewDocument.File != null && viewDocument.File.Length > 0)
            {
                //get file name
                var obj = _context.Requests.FirstOrDefault(x => x.Requestid == viewDocument.RequestId);
                User? user = _context.Users.First(x => x.Userid == obj.Userid);
                var fileName = Path.GetFileName(viewDocument.File.FileName);

                string rootPath = _environment.WebRootPath + "/UploadedFiles";


                string userId = user.Userid.ToString();

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
                    viewDocument.File.CopyTo(stream)
;
                }


                Requestwisefile requestwisefile = new()
                {
                    Filename = fileName,
                    Requestid = obj.Requestid,
                    Createddate = DateTime.Now
                };

                _context.Requestwisefiles.Add(requestwisefile);
                _context.SaveChanges();
            }
        }
        public void SubmitInfoAboutMe(PatientSubmitRequest u)
        {

        }
        public PatientDashboard PatientDashboard(int userId)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);
            PatientDashboard dashboardVM = new()
            {
                UserId = user.Userid,
                UserName = user.Firstname + " " + user.Lastname,
                Requests = _context.Requests.Where(req => req.Userid == user.Userid).ToList()
            };
            List<int> fileCounts = new();
            foreach (var request in dashboardVM.Requests)
            {
                int count = _context.Requestwisefiles.Count(reqFile => reqFile.Requestid == request.Requestid);
                fileCounts.Add(count);
            }
            dashboardVM.DocumentCount = fileCounts;
            return dashboardVM;
        }
        public PatientProfile Profile(int userId)
        {
            //string dobDate = user.Intyear + "-" + patientProfile.Strmonth + "-" + patientProfile.Intdate;

            User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);
            PatientProfile model = new()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Type = "Mobile",
                Phone = user.Mobile,
                Email = user.Email,
                Street = user.Street,
                City = user.City,
                State = user.State,
            };
            return model;
        }
        public void ProfileUpdate(PatientProfile profile, int userId)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Userid == userId);

            Aspnetuser obj = _context.Aspnetusers.FirstOrDefault(x => x.Id == user.Aspnetuserid);

            obj.Email = profile.Email;
            obj.Modifieddate = DateTime.Now;
            obj.Phonenumber = profile.Phone;
            obj.Username = profile.FirstName + " " + profile.LastName;

            _context.Aspnetusers.Update(obj);
            _context.SaveChanges();

            user.Firstname = profile.FirstName;
            user.Lastname = profile.LastName;
            user.Email = profile.Email;
            user.Mobile = profile.Phone;
            user.Street = profile.Street;
            user.City = profile.City;
            user.State = profile.State;
            user.Zip = profile.ZipCode;
            user.Modifiedby = profile.FirstName;
            user.Modifieddate = DateTime.Now;
            user.Strmonth = profile.Date?.ToString("MMMM");
            user.Intdate = profile.Date?.Day;
            user.Intyear = profile.Date?.Year;

            _context.Users.Update(user);
            _context.SaveChanges();

            Request request = _context.Requests.Where(x => x.Userid == user.Userid).FirstOrDefault();
            request.Firstname = profile.FirstName;
            request.Lastname = profile.LastName;
            request.Email = profile.Email;
            request.Phonenumber = profile.Phone;

            _context.Requests.Update(request);
            _context.SaveChanges();
        }
    }
}
