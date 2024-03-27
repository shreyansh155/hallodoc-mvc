using BusinessLogic.Interface;
using DataAccess.ViewModels;
using System.Text;
using DataAccess.DataContext;
using System.Security.Cryptography;
using DataAccess.DataModels;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;


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


        public static void InsertFileAfterRename(IFormFile file, string path, string updateName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] oldfiles = Directory.GetFiles(path, updateName + ".*");
            foreach (string f in oldfiles)
            {
                System.IO.File.Delete(f);
            }

            string extension = Path.GetExtension(file.FileName);

            string fileName = updateName + extension;

            string fullPath = Path.Combine(path, fileName);

            using FileStream stream = new(fullPath, FileMode.Create);
            file.CopyTo(stream);
        }

        public bool AdminLogin(AdminLogin adminLogin)
        {
            string hashPassword = GenerateSHA256(adminLogin.Password); ;
            return _context.Aspnetusers.Any(Au => Au.Email == adminLogin.Email && Au.Passwordhash == hashPassword);
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
                              where t1.Status == 5 || t1.Status == 7
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
                               where req.Status == 5 || req.Status == 7
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
            Request request = _context.Requests.FirstOrDefault(x => x.Requestid == obj.Requestid);
            ViewCaseViewModel viewCaseViewModel = new()
            {
                Status = request.Status,
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
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == req.Requestid);
            Physician physician = _context.Physicians.First(x => x.Physicianid == 1);
            List<Requeststatuslog> requeststatuslog = _context.Requeststatuslogs.Where(x => x.Requestid == req.Requestid).ToList();


            ViewNotes? viewNote = new()
            {
                Status = request.Status,
                PhysicianName = physician.Firstname,
                AdminNotes = obj?.Adminnotes ?? "",
                PhysicianNotes = obj?.Physiciannotes ?? "",
                Statuslogs = requeststatuslog,
                Requestclientid = reqClientId,
            };
            return viewNote;
        }
        public void ViewNotesUpdate(ViewNotes viewNotes)
        {
            Requestclient? req = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == viewNotes.Requestclientid);
            Requestnote? obj = _context.Requestnotes.FirstOrDefault(x => x.Requestid == req.Requestid);
            if (obj == null)
            {
                Requestnote reqNoteDb = new()
                {
                    Requestid = req.Requestid,
                    Adminnotes = viewNotes.TextBox,
                    Createddate = DateTime.Now,
                    Createdby = "admin"
                };
                _context.Requestnotes.Add(reqNoteDb);
                _context.SaveChanges();
            }
            else
            {
                obj.Adminnotes = viewNotes.TextBox ?? "";
                obj.Modifieddate = DateTime.Now;
                obj.Modifiedby = "admin";
                _context.Requestnotes.Update(obj);
                _context.SaveChanges();
            }

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
                Status = request.Status,
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

                //    /UploadedFiles/123/ABC

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

            return true;
        }
        public Orders Orders(int RequestId)
        {
            Request? req = _context.Requests.FirstOrDefault(x => x.Requestid == RequestId);
            var healthprofessionaltype = _context.Healthprofessionaltypes.ToList();
            var healthprofessionals = _context.Healthprofessionals.ToList();

            Orders orders = new()
            {
                Status = req.Status,
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
        public void SendAgreementCase(SendAgreementCase sendAgreement, string link)
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
                Body = "<h3>Admin has sent you the agreement papers to review. Click on the link below to read the agreement.</h3><a href=\"" + link + "\">Review Agreement link</a>",
            };

            mailMessage.To.Add(sendAgreement.Email);

            client.Send(mailMessage);
        }
        public CloseCase CloseCaseView(int reqClientId)
        {
            Requestclient reqCli = _context.Requestclients.FirstOrDefault(requestCli => requestCli.Requestclientid == reqClientId);
            Request request = _context.Requests.FirstOrDefault(x => x.Requestid == reqCli.Requestid);
            var FileList = _context.Requestwisefiles.Where(x => x.Requestid == request.Requestid && (x.Isdeleted == null || x.Isdeleted == false)).ToList();

            CloseCase obj = new()
            {
                Status = request.Status,
                ReqClientid = reqClientId,
                FirstName = reqCli.Firstname,
                LastName = reqCli.Lastname,
                UserId = (int)request.Userid,
                PhoneNumber = request.Phonenumber,
                Email = request.Email,
                DateOfBirth = DateTime.Now,
                Files = FileList,
                ConfirmationNumber = ""
            };
            return obj;

        }
        public void CloseToUnpaidCase(int reqClientId)
        {
            Requestclient? requestclient = _context.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);
            request.Status = 13;
            request.Modifieddate = DateTime.Now;

            _context.Requests.Update(request);
            Requeststatuslog? requeststatuslog = new()
            {
                Requestid = request.Requestid,
                Status = 13,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
        }
        public void CloseCaseSave(CloseCase closeCase)
        {
            Requestclient? requestclient = _context.Requestclients.Where(x => x.Requestclientid == closeCase.ReqClientid).FirstOrDefault();
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);

            requestclient.Phonenumber = closeCase.PhoneNumber;
            requestclient.Email = closeCase.Email;

            request.Phonenumber = closeCase.PhoneNumber;
            request.Email = closeCase.Email;


            _context.Requestclients.Update(requestclient);
            _context.Requests.Update(request);
            _context.SaveChanges();
        }
        public EncounterModel Encounter(int reqClientId)
        {
            Requestclient? requestclient = _context.Requestclients.Where(x => x.Requestclientid == reqClientId).FirstOrDefault();
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == requestclient.Requestid);

            EncounterModel encounter = new()
            {
                Status = request.Status,
                ReqClientid = reqClientId,
                Requestid = request.Requestid
            };
            return encounter;
        }
        public void EncounterSubmit(EncounterModel encounter)
        {
            Requestclient? requestclient = _context.Requestclients.Where(x => x.Requestclientid == encounter.ReqClientid).FirstOrDefault();
            Request? request = _context.Requests.FirstOrDefault(x => x.Requestid == encounter.Requestid);

            Encounter obj = _context.Encounters.First(x => x.Requestid == encounter.Requestid);
            if (obj == null)
            {
                Encounter user = new()
                {

                    Requestid = (int)encounter.Requestid,
                    Firstname = encounter.FirstName,
                    LastName = encounter.LastName,
                    Location = encounter.Location,
                    Strmonth = encounter.DateOfBirth.Month.ToString("MMM"),
                    Intyear = encounter.DateOfBirth.Year,
                    Intdate = encounter.DateOfBirth.Day,
                    Servicedate = DateTime.Now,
                    Phonenumber = encounter.PhoneNumber,
                    Email = encounter.Email,
                    PresentIllnessHistory = encounter.PatientHistory,
                    MedicalHistory = encounter.MedicalHistory,
                    Medications = encounter.Medications,
                    Allergies = encounter.Allergies,
                    Temperature = encounter.Temp,
                    HeartRate = encounter.Hr,
                    RespirationRate = encounter.Rr,
                    BloodPressureSystolic = encounter.BloodPressureS,
                    BloodPressureDiastolic = encounter.BloodPressureD,
                    OxygenLevel = encounter.O2,
                    Pain = encounter.Pain,
                    Heent = encounter.Heent,
                    Cardiovascular = encounter.CV,
                    Chest = encounter.Chest,
                    Abdomen = encounter.ABD,
                    Extremities = encounter.Extr,
                    Skin = encounter.Skin,
                    Neuro = encounter.Neuro,
                    Other = encounter.Other,
                    Diagnosis = encounter.Diagnosis,
                    TreatmentPlan = encounter.Treatment,
                    MedicationsDispensed = encounter.MedicationsDispensed,
                    Procedures = encounter.Procedures,
                    FollowUp = encounter.Followup,
                    CreatedDate = DateTime.Now,

                    Isfinalized = true,
                    FinalizedDate = DateTime.Now,
                };
                _context.Encounters.Add(user);
                _context.SaveChanges();
            }
            else
            {
                obj.ModifiedDate = DateTime.Now;
            }
            _context.Encounters.Add(obj);
            _context.SaveChanges();
        }
        public AdminProfile ProfileInfo(int adminId)
        {
            Admin? obj = _context.Admins.FirstOrDefault(x => x.Adminid == adminId);

            var region = _context.Regions.FirstOrDefault(x => x.Regionid == obj.Regionid).Name;


            var regions = _context.Regions.ToList();


            IEnumerable<int> adminRegions = _context.Adminregions.Where(region => region.Adminid == adminId).ToList().Select(x => x.Regionid);


            AdminProfile profile = new()
            {
                UserName = obj.Firstname + " " + obj.Lastname,
                AdminId = adminId,
                //AdminPassword=obj.,
                Status = obj.Status,
                Role = obj.Roleid.ToString() ?? "",
                FirstName = obj.Firstname,
                LastName = obj.Lastname,
                AdminPhone = obj.Mobile,
                Email = obj.Email,
                ConfirmEmail = obj.Email,
                Address1 = obj.Address1,
                Address2 = obj.Address2,
                City = region,
                State = region,
                Zip = obj.Zip,
                BillingPhone = obj.Altphone,
                RegionList = regions,
                AdminRegion = adminRegions.ToList(),
            };

            return profile;
        }

        public void UpdateProfile(AdminProfile obj)
        {
            int adminId = obj.AdminId;
            Admin? adminRow = _context.Admins.Where(x => x.Adminid == adminId).FirstOrDefault();
            Aspnetuser user = _context.Aspnetusers.FirstOrDefault(x => x.Id == adminRow.Aspnetuserid);

            switch (obj.FormId)
            {
                case 1:
                    user.Passwordhash = GenerateSHA256(obj.AdminPassword);
                    adminRow.Status = obj.Status;
                    adminRow.Modifieddate = DateTime.Now;
                    user.Username = obj.UserName;
                    break;


                case 2:
                    adminRow.Firstname = obj.FirstName;
                    adminRow.Lastname = obj.LastName;
                    adminRow.Email = obj.Email;
                    adminRow.Mobile = obj.AdminPhone;
                    adminRow.Modifieddate = DateTime.Now;
                    break;


                case 3:
                    adminRow.Altphone = obj.BillingPhone;
                    adminRow.Address1 = obj.Address1;
                    adminRow.Address2 = obj.Address2;
                    adminRow.Modifieddate = DateTime.Now;
                    adminRow.Zip = obj.Zip;
                    break;
            };

            _context.Admins.Update(adminRow);
            _context.Aspnetusers.Update(user);
            _context.SaveChanges();
        }

        public ProviderMenu ProviderMenu()
        {

            var providerMenu = from phy in _context.Physicians
                               join role in _context.Roles on phy.Roleid equals role.Roleid
                               join phyid in _context.Physiciannotifications on phy.Physicianid equals phyid.Physicianid
                               orderby phy.Physicianid
                               select new ProviderList
                               {
                                   PhysicianId = phy.Physicianid,
                                   FirstName = phy.Firstname,
                                   LastName = phy.Lastname,
                                   Status = phy.Status,
                                   Role = role.Name,
                                   OnCallStatus = "",
                                   Notification = phyid.Isnotificationstopped
                               };

            var obj = new ProviderMenu()
            {
                ProviderLists = providerMenu,
            };
            return obj;
        }

        public void ContactProvider(ContactYourProvider contactYourProvider)
        {

        }

        public void StopProviderNotif(int PhysicianId)
        {
            Physiciannotification user = _context.Physiciannotifications.FirstOrDefault(x => x.Physicianid == PhysicianId);

            if (user != null)
            {
                if (user.Isnotificationstopped)
                {
                    user.Isnotificationstopped = false;
                }
                else
                {
                    user.Isnotificationstopped = true;
                }

                _context.Physiciannotifications.Update(user);
                _context.SaveChanges();
                return;
            }
            else
            {
                Physiciannotification obj = new()
                {
                    Physicianid = PhysicianId,
                    Isnotificationstopped = true
                };
                _context.Physiciannotifications.Add(obj);
                _context.SaveChanges();
                return;
            }
        }

        public EditPhysicianAccount EditPhysician(int physicianId)
        {
            var phy = _context.Physicians.FirstOrDefault(x => x.Physicianid == physicianId);
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(x => x.Id == phy.Aspnetuserid);
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == phy.Regionid).Name;
            EditPhysicianAccount obj = new()
            {
                PhysicianId = physicianId,
                UserName = aspnetuser.Username,
                FirstName = phy.Firstname,
                LastName = phy.Lastname,
                Email = phy.Email,
                PhoneNumber = phy.Mobile,
                MedicalLicenseNumber = phy.Medicallicense,
                NPINumber = phy.Npinumber,
                SyncEmail = phy.Syncemailaddress,
                Address1 = phy.Address1,
                Address2 = phy.Address2,
                City = phy.City,
                State = region,
                Zip = phy.Zip,
                Phone = phy.Altphone,
                BusinessName = phy.Businessname,
                BusinessWebsite = phy.Businesswebsite,
                //Photo=phy.Photo,
                //Signature = phy.Signature,
            };

            return obj;
        }
        public void EditPhysicianAccount(EditPhysicianAccount obj)
        {
            var phy = _context.Physicians.FirstOrDefault(x => x.Physicianid == obj.PhysicianId);
            var aspnetuser = _context.Aspnetusers.FirstOrDefault(x => x.Id == phy.Aspnetuserid);
            var region = _context.Regions.FirstOrDefault(x => x.Regionid == phy.Regionid)?.Name;


            void FileUpload(IFormFile file)
            {
                if (file != null && file.Length > 0)
                {
                    //get file name

                    var fileName = Path.GetFileName(file.FileName);

                    string rootPath = _environment.WebRootPath + "/PhysicianImages";


                    string physicianId = phy.Physicianid.ToString();

                    string userFolder = Path.Combine(rootPath, physicianId);

                    if (!Directory.Exists(userFolder))
                    {
                        Directory.CreateDirectory(userFolder);
                    }


                    //define path
                    string filePath = Path.Combine(userFolder, fileName);


                    // Copy the file to the desired location
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    _context.Physicians.Update(phy);
                    _context.SaveChanges();
                }
            };

            switch (obj.FormId)
            {
                case 1:
                    aspnetuser.Username = obj.UserName;
                    aspnetuser.Passwordhash = GenerateSHA256(obj.Password);

                    break;


                case 2:
                    phy.Firstname = obj.FirstName;
                    phy.Lastname = obj.LastName;
                    phy.Email = obj.Email;
                    phy.Mobile = obj.Phone;
                    phy.Medicallicense = obj.MedicalLicenseNumber;
                    phy.Npinumber = obj.NPINumber;
                    phy.Syncemailaddress = obj.SyncEmail;

                    break;


                case 3:
                    phy.Address1 = obj.Address1;
                    phy.Address2 = obj.Address2;
                    phy.City = obj.City;
                    //phy.Region=
                    phy.Zip = obj.Zip;
                    phy.Altphone = obj.Phone;
                    break;


                case 4:
                    FileUpload(obj.Photo);
                    FileUpload(obj.Signature);
                    phy.Businessname = obj.BusinessName;
                    phy.Businesswebsite = obj.BusinessWebsite;
                    phy.Photo = obj.Photo.FileName;
                    phy.Signature = obj.Signature.FileName;
                    break;
                case 5:
                    break;
            };
        }





        public List<AccountAccess> AccountAccess()
        {
            var obj = (from role in _context.Roles
                       where role.Isdeleted != true
                       select new AccountAccess
                       {
                           Name = role.Name,
                           RoleId = role.Roleid,
                           AccountType = role.Accounttype,
                       }).ToList();
            return obj;
        }
        public void DeleteRole(int roleId)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Roleid == roleId);
            role.Isdeleted = true;
            _context.Roles.Update(role);
            _context.SaveChanges();
        }
        public CreateAccess FetchRole(short selectedValue)
        {
            if (selectedValue == 0)
            {
                CreateAccess obj = new()
                {
                    Menu = _context.Menus.ToList(),
                };
                return obj;
            }
            else if (selectedValue == 1 || selectedValue == 2)
            {

                CreateAccess obj = new()
                {
                    Menu = _context.Menus.Where(x => x.Accounttype == selectedValue).ToList(),
                };
                return obj;
            }
            else
            {
                CreateAccess obj = new();
                return obj;
            }
        }

        public void CreateRole(List<int> menuIds, string roleName, short accountType)
        {
            Role role = new()
            {
                Name = roleName,
                Accounttype = accountType,
                Createdby = "Admin",
                Createddate = DateTime.Now,
                Isdeleted = false,
            };
            _context.Roles.Add(role);
            _context.SaveChanges();

            foreach (int menuId in menuIds)
            {
                Rolemenu rolemenu = new()
                {
                    Roleid = role.Roleid,
                    Menuid = menuId,
                };
                _context.Rolemenus.Add(rolemenu);
                _context.SaveChanges();
            };


        }


        public void CreateAdminAccount(CreateAdminAccount obj)
        {
            Guid id = Guid.NewGuid();
            Aspnetuser aspnetuser = new()
            {
                Id = id.ToString(),
                Username = obj.UserName,
                Passwordhash = GenerateSHA256(obj.AdminPassword),
                Email = obj.Email,
                Phonenumber = obj.AdminPhone,
                Createddate = DateTime.Now,

            };
            _context.Aspnetusers.Add(aspnetuser);
            _context.SaveChanges();

            Admin admin = new()
            {
                Aspnetuserid = id.ToString(),
                Firstname = obj.FirstName,
                Lastname = obj.LastName,
                Email = obj.Email,
                Mobile = obj.AdminPhone,
                Address1 = obj.Address1,
                Address2 = obj.Address2,
                Zip = obj.Zip,
                Altphone = obj.BillingPhone,
                Createdby = "admin",
                Createddate = DateTime.Now,
                Isdeleted = false,

            };
            _context.Admins.Add(admin);
            _context.SaveChanges();



            var AdminRegions = obj.AdminRegion.ToList();
            for (int i = 0; i < AdminRegions.Count; i++)
            {
                Adminregion adminregion = new()
                {
                    Adminid = admin.Adminid,
                    Regionid = _context.Regions.First(x => x.Regionid == AdminRegions[0]).Regionid,
                };

                _context.Adminregions.Add(adminregion);
                _context.SaveChanges();
            }


        }
        public void CreateProviderAccount(CreateProviderAccount model)
        {
            List<string> validProfileExtensions = new() { ".jpeg", ".png", ".jpg" };
            List<string> validDocumentExtensions = new() { ".pdf" };

            try
            {
                Guid generatedId = Guid.NewGuid();

                Aspnetuser aspUser = new()
                {
                    Id = generatedId.ToString(),
                    Username = model.UserName,
                    Passwordhash = GenerateSHA256(model.Password),
                    Email = model.Email,
                    Phonenumber = model.Phone,
                    Createddate = DateTime.Now,
                };                _context.Aspnetusers.Add(aspUser);
                _context.SaveChanges();


                Physician phy = new()
                {
                    Aspnetuserid = generatedId.ToString(),
                    Firstname = model.FirstName,
                    Lastname = model.LastName,
                    Email = model.Email,
                    Mobile = model.Phone,
                    Medicallicense = model.MedicalLicenseNumber,
                    Adminnotes = model.AdminNote,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    //Regionid = model.RegionId,
                    Zip = model.Zip,
                    Altphone = model.PhoneNumber,
                    Createdby = "admin",
                    Createddate = DateTime.Now,
                    //Status = (short)model.s,
                    //Roleid = model.RoleId,
                    Npinumber = model.NPINumber,
                    Businessname = model.BusinessName,
                    Businesswebsite = model.BusinessWebsite,
                };

                _context.Physicians.Add(phy);
                _context.SaveChanges();


                string path = Path.Combine(_environment.WebRootPath, "PhysicianImages", phy.Physicianid.ToString());

                if (model.Photo != null)
                {
                    string fileExtension = Path.GetExtension(model.Photo.FileName);
                    if (validDocumentExtensions.Contains(fileExtension))
                    {
                        phy.Isnondisclosuredoc = true;
                        InsertFileAfterRename(model.Photo, path, "ProfilePhoto");
                    }
                }
                if (model.ICA != null)
                {
                    string fileExtension = Path.GetExtension(model.ICA.FileName);
                    if (validDocumentExtensions.Contains(fileExtension))
                    {
                        phy.Isnondisclosuredoc = true;
                        InsertFileAfterRename(model.ICA, path, "ICA");
                    }
                }
                if (model.BGCheck != null)
                {
                    string fileExtension = Path.GetExtension(model.BGCheck.FileName);
                    if (validDocumentExtensions.Contains(fileExtension))
                    {
                        phy.Isnondisclosuredoc = true;
                        InsertFileAfterRename(model.BGCheck, path, "BackgroundCheck");
                    }
                }
                if (model.HIPAACompliance != null)
                {
                    string fileExtension = Path.GetExtension(model.HIPAACompliance.FileName);
                    if (validDocumentExtensions.Contains(fileExtension))
                    {
                        phy.Isnondisclosuredoc = true;
                        InsertFileAfterRename(model.HIPAACompliance, path, "HipaaCompliance");
                    }
                }
                if (model.NDA != null)
                {
                    string fileExtension = Path.GetExtension(model.NDA.FileName);
                    if (validDocumentExtensions.Contains(fileExtension))
                    {
                        phy.Isnondisclosuredoc = true;
                        InsertFileAfterRename(model.NDA, path, "NDA");
                    }
                }
            }
            catch (Exception e)
            {
            };


        }
    }

}

