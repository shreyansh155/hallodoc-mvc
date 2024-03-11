﻿using BusinessLogic.Interface;
using DataAccess.ViewModels;
using System.Text;
using DataAccess.DataContext;
using System.Security.Cryptography;
using DataAccess.DataModels;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
namespace BusinessLogic.Repository
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public AdminService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _config = config;
            _context = context;
            _environment = webHostEnvironment;
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
                Aspnetuser? aspnetuser = new()
                {
                    Id = id.ToString(),
                    Passwordhash = hashedPassword,
                    Createddate = DateTime.Now,
                    Username = newAccount.UserName,
                    Email = newAccount.Email,

                };
                _context.Aspnetusers.Add(aspnetuser);
                _context.SaveChanges();
                Admin? admin = new()
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
                                   RequestId = req.Requestid,
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
                                      RequestId = req.Requestid,
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
                                    RequestId = req.Requestid,
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
            Requestclient? obj = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
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
        public void CancelCase(CancelCase cancelCase)
        {
            Requestclient? requestclient = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == cancelCase.ReqClientid);
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);


            Requeststatuslog? requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 5,
                Createddate = DateTime.Now,
                Notes = cancelCase.AddOnNotes,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);

            request.Status = 5;
            request.Modifieddate = DateTime.Now;
            request.Casetag = cancelCase.CaseTagId.ToString();

            _context.Requests.Update(request);
            _context.SaveChanges();

        }
        public ViewNotes ViewNotes(int reqClientId)
        {
            Requestclient? req = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            Requestnote? obj = _context.Requestnotes.FirstOrDefault(x => x.Requestid == req.Requestid);
            Physician physician = _context.Physicians.First(x => x.Physicianid == 1);
            List<Requeststatuslog> requeststatuslog = _context.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).ToList();


            ViewNotes? viewNote = new()
            {
                PhysicianName = physician.Firstname,
                AdminNotes = obj?.Adminnotes ?? "",
                PhysicianNotes = obj?.Physiciannotes ?? "",
                Statuslogs = requeststatuslog,
                Requestclientid = reqClientId,
            };
            return viewNote;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void ViewNotesUpdate(ViewNotes viewNotes)
        {
            Requestclient? req = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == viewNotes.Requestclientid);
            Requestnote? obj = _context.Requestnotes.FirstOrDefault(x => x.Requestid == req.Requestid);
            obj.Adminnotes = viewNotes.TextBox;

        }
        public void AssignCase(AssignCase assignCase)
        {
            Requestclient? requestclient = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == assignCase.ReqClientid);
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);

            Requeststatuslog? requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 2,
                Createddate = DateTime.Now,
                Notes = assignCase.Description,
                Physicianid = assignCase.PhysicianId,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);

            request.Status = 2;
            request.Modifieddate = DateTime.Now;

            _context.Requests.Update(request);
            _context.SaveChanges();

        }
        public void BlockCase(BlockCase blockCase)
        {
            Requestclient? requestclient = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == blockCase.ReqClientid);
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);

            Blockrequest blockrequest = new()
            {
                Phonenumber = request.Phonenumber,
                Email = request.Email,
                Isactive = true,
                Reason = blockCase.BlockReason,
                Requestid = request.Requestid,
                Createddate = DateTime.Now,
            };
            _context.Blockrequests.Add(blockrequest);
            _context.SaveChanges();

            Requeststatuslog? requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 11,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();

            request.Status = 11;
            request.Modifieddate = DateTime.Now;

            _context.Requests.Update(request);
            _context.SaveChanges();
        }
        public ViewUploads ViewUploads(int reqClientId)
        {
            Requestclient? requestclient = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            Request? request = _context.Requests.FirstOrDefault(r => r.Requestid == requestclient.Requestid);
            User? user = _context.Users.FirstOrDefault(u => u.Userid == request.Userid);

            List<Requestwisefile> fileList = _context.Requestwisefiles.Where(reqFile => reqFile.Requestid == request.Requestid).Where(x => x.Isdeleted == false || x.Isdeleted == null).ToList();

            ViewUploads viewUploads = new()
            {
                PatientName = user.Firstname + " " + user.Lastname,
                RequestWiseFiles = fileList,
                RequestId = request.Requestid,
                reqClientId = reqClientId,
            };
            return viewUploads;
        }
        public void UploadFiles(ViewUploads viewUploads)
        {
            if (viewUploads.File != null && viewUploads.File.Length > 0)
            {
                //get file name
                var obj = _context.Requests.FirstOrDefault(x => x.Requestid == viewUploads.RequestId);
                User? user = _context.Users.First(x => x.Userid == obj.Userid);
                var fileName = Path.GetFileName(viewUploads.File.FileName);

                string rootPath = _environment.WebRootPath + "/UploadedFiles";


                string requestId = obj.Requestid.ToString();

                string userFolder = Path.Combine(rootPath, requestId);

                if (!Directory.Exists(userFolder))
                {
                    Directory.CreateDirectory(userFolder);
                }


                //define path
                string filePath = Path.Combine(userFolder, fileName);


                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    viewUploads.File.CopyTo(stream)
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
        public void DeleteFile(int Requestwisefileid)
        {
            Requestwisefile requestwisefile = _context.Requestwisefiles.Where(x => x.Requestwisefileid == Requestwisefileid).FirstOrDefault();
            requestwisefile.Isdeleted = true;
            _context.Requestwisefiles.Update(requestwisefile);
            _context.SaveChanges();
        }
        public bool SendFilesViaMail(List<int> fileIds, int requestId)
        {
            Requestclient reqCli = _context.Requestclients.FirstOrDefault(requestCli => requestCli.Requestid == requestId);

            string? senderEmail = _config.GetSection("OutlookSMTP")["Sender"];
            string? senderPassword = _config.GetSection("OutlookSMTP")["Password"];

            SmtpClient client = new("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Hallodoc documents attachments",
                IsBodyHtml = true,
                Body = "<h3>Admin has sent you documents regarding your request.</h3>",
            };

            MemoryStream memoryStream;
            foreach (int fileId in fileIds)
            {
                Requestwisefile? file = _context.Requestwisefiles.FirstOrDefault(reqFile => reqFile.Requestwisefileid == fileId);
                string documentPath = Path.Combine(_environment.WebRootPath, "UploadedFiles", requestId.ToString(), file.Filename);
                byte[] fileBytes = System.IO.File.ReadAllBytes(documentPath);
                memoryStream = new MemoryStream(fileBytes);
                mailMessage.Attachments.Add(new Attachment(memoryStream, file.Filename));
            }

            mailMessage.To.Add(reqCli.Email);

            client.Send(mailMessage);

            //TempData["success"] = "Email with selected documents has been successfully sent to " + reqCli.Email;
            return true;
        }
        public Orders Orders(int RequestId)
        {
            Request? req = _context.Requests.FirstOrDefault(x => x.Requestid == RequestId);
            var healthprofessionaltype = _context.Healthprofessionaltypes.ToList();
            var healthprofessionals = _context.Healthprofessionals.ToList();

            Orders orders = new()
            {
                ProfessionTypes = healthprofessionaltype,
                HealthProfessionals = healthprofessionals,
                Requestid = RequestId,
            };
            return orders;
        }
        public JsonArray FetchVendors(int selectedValue)
        {
            var result = new JsonArray();
            IEnumerable<Healthprofessional> businesses = _context.Healthprofessionals.Where(prof => prof.Profession == selectedValue);

            foreach (Healthprofessional business in businesses)
            {
                result.Add(new { businessId = business.Vendorid, businessName = business.Vendorname });
            }
            return result;
        }
        public JsonArray FetchPhysician(int selectedValue)
        {
            var result = new JsonArray();
            IEnumerable<Physician> physician = _context.Physicians.Where(x => x.Regionid == selectedValue);

            foreach (Physician item in physician)
            {
                result.Add(new { physicianId = item.Physicianid, physicianName = item.Firstname });
            }
            return result;
        }
        public Healthprofessional VendorDetails(int selectedValue)
        {
            Healthprofessional business = _context.Healthprofessionals.First(prof => prof.Vendorid == selectedValue);

            return business;
        }
        public void SendOrder(Orders orders)
        {
            Orderdetail orderdetail = new()
            {
                Requestid = orders.Requestid,
                Vendorid = orders.Vendorid,
                Faxnumber = orders.FaxNumber,
                Businesscontact = orders.BusinessContact,
                Email = orders.Email,
                Noofrefill = orders.NumberOfRefills,
                Prescription = orders.Prescription,
                Createddate = DateTime.Now,
                Createdby = "Admin",
            };
            _context.Orderdetails.Add(orderdetail);
            _context.SaveChanges();
        }
        public void TransferCase(TransferCase transferCase)
        {
            Requestclient? requestclient = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == transferCase.ReqClientid);
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);

            Requeststatuslog? requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 2,
                Createddate = DateTime.Now,
                Notes = transferCase.Description,
                Physicianid = transferCase.PhysicianId,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);

            request.Modifieddate = DateTime.Now;

            _context.Requests.Update(request);
            _context.SaveChanges();
        }
        public void ClearCase(int reqClientId)
        {
            Requestclient? requestclient = _context.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);
            request.Status = 10;
            request.Modifieddate = DateTime.Now;

            _context.Requests.Update(request);
            Requeststatuslog? requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 10,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();

        }
        public void SendAgreementCase(SendAgreementCase sendAgreement)
        {

            Requestclient reqCli = _context.Requestclients.FirstOrDefault(requestCli => requestCli.Requestclientid == sendAgreement.ReqClientid);

            string? senderEmail = _config.GetSection("OutlookSMTP")["Sender"];
            string? senderPassword = _config.GetSection("OutlookSMTP")["Password"];

            SmtpClient client = new("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Hallodoc review agreement",
                IsBodyHtml = true,
                Body = "<h3>Admin has sent you the agreement papers to review. Click on the link below to read the agreement.</h3>",
            };

            mailMessage.To.Add(reqCli.Email);

            client.Send(mailMessage);
        }
    }
}