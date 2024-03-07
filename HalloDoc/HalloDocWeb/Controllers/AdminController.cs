using Microsoft.AspNetCore.Mvc;
using DataAccess.ViewModels;
using DataAccess.DataContext;
using BusinessLogic.Interface;
using BusinessLogic.Repository;
using DataAccess.DataModels;
using System.Text.Json.Nodes;


namespace HalloDocWeb.Controllers
{
    [CustomAuthorize((int)AllowRole.Admin)]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdminService _adminService;

        public AdminController(ApplicationDbContext context, IAdminService adminService)
        {
            _context = context;
            _adminService = adminService;
        }
        public IActionResult CreateAdminAccount()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAdminAccount(CreateAdminAccount newAccount)
        {
            if (ModelState.IsValid)
            {
                _adminService.CreateAdminAccount(newAccount);
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        public IActionResult AdminDashboard()
        {

            var data = _adminService.AdminDashboard();
            int? adminId = HttpContext.Session.GetInt32("adminId");
            var dashData = new AdminDashboard
            {
                CountRequestViewModel = data.CountRequestViewModel,
            };
            return View(dashData);
        }
        public IActionResult ViewCase(int reqClientId)
        {
            int? adminId = HttpContext.Session.GetInt32("adminId");

            var obj = _adminService.ViewCaseViewModel(reqClientId);

            return View(obj);
        }
        public IActionResult ViewNotes(int reqClientId)
        {
            _ = HttpContext.Session.GetInt32("adminId");
            var obj = _adminService.ViewNotes(reqClientId);
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewNotesUpdate(ViewNotes viewNotes)
        {
            if (ModelState.IsValid)
            {
                int? adminId = HttpContext.Session.GetInt32("adminId");
                _adminService.ViewNotesUpdate(viewNotes);
                return ViewNotes(viewNotes.Requestclientid);
            }
            return View(viewNotes);
        }
        [HttpPost]
        public IActionResult PartialTable(int status, SearchViewModel obj)
        {
            var data = _adminService.AdminDashboard();

            if (obj.Name != null || obj.Sorting != null)
            {
                var searchData = _adminService.SearchPatient(obj, data);
                if (status == 1)
                {
                    var parseData = searchData.NewReqViewModel;
                    return PartialView("_newRequestView", parseData);
                }
                else if (status == 2)
                {
                    var parseData = searchData.PendingReqViewModel;
                    return PartialView("_PendingRequestView", parseData);
                }
                else if (status == 8)
                {
                    var parseData = searchData.ActiveReqViewModels;
                    return PartialView("_activeRequestView", parseData);
                }
                else if (status == 4)
                {
                    var parseData = searchData.ConcludeReqViewModel;
                    return PartialView("_concludeReqView", parseData);
                }
                else if (status == 5)
                {
                    var parseData = searchData.CloseReqViewModels;
                    return PartialView("_closeReqView", parseData);
                }
                else
                {
                    var parseData = searchData.UnpaidReqViewModels;
                    return PartialView("_unpaidReqView", parseData);
                }
            }

            if (status == 1)
            {
                var parseData = data.NewReqViewModel;
                return PartialView("_newRequestView", parseData);
            }
            else if (status == 2)
            {
                var parseData = data.PendingReqViewModel;
                return PartialView("_PendingRequestView", parseData);
            }
            else if (status == 8)
            {
                var parseData = data.ActiveReqViewModels;
                return PartialView("_activeRequestView", parseData);
            }
            else if (status == 4)
            {
                var parseData = data.ConcludeReqViewModel;
                return PartialView("_concludeReqView", parseData);
            }
            else if (status == 5)
            {
                var parseData = data.CloseReqViewModels;
                return PartialView("_closeReqView", parseData);
            }
            else
            {
                var parseData = data.UnpaidReqViewModels;
                return PartialView("_unpaidReqView", parseData);
            }

        }
        [HttpPost]
        public IActionResult AdminDashboard(SearchViewModel obj)
        {
            //ViewBag.AdminName = HttpContext.Session.GetString("adminToken").ToString();
            ViewBag.AdminId = HttpContext.Session.GetInt32("adminId");
            var data = _adminService.AdminDashboard();

            var searchedData = _adminService.SearchPatient(obj, data);

            var dashData = new AdminDashboard
            {
                CountRequestViewModel = searchedData.CountRequestViewModel,
            };

            return View(dashData);

        }
        public IActionResult CancelCaseModal(int requestClientId)
        {
            CancelCase cancelCase = new()
            {
                ReqClientid = requestClientId,
                Casetags = _context.Casetags,
            };
            return PartialView("_CancelCaseView", cancelCase);
        }
        public IActionResult AssignCaseModal(int requestClientId)
        {
            AssignCase assignCase = new()
            {
                ReqClientid = requestClientId,
                Region = _context.Regions,
                Physicians = _context.Physicians
            };
            return PartialView("_AssignCaseView", assignCase);
        }
        public IActionResult BlockCaseModal(int requestClientId)
        {
            BlockCase blockCase = new()
            {
                ReqClientid = requestClientId,
            };
            return PartialView("_BlockCaseView", blockCase);
        }
        [HttpPost]
        public IActionResult CancelCase(int ReqClientid, int CaseTagId, string AddOnNotes)
        {
            CancelCase cancelCase = new()
            {
                ReqClientid = ReqClientid,
                CaseTagId = CaseTagId,
                AddOnNotes = AddOnNotes,
            };
            if (ModelState.IsValid)
            {
                _adminService.CancelCase(cancelCase);
                return RedirectToAction("AdminDashboard");
            }

            return PartialView("_CancelCaseView", cancelCase);
        }
        [HttpPost]
        public IActionResult AssignCase(int ReqClientid, int PhysicianId, int RegionId, string Description)
        {
            AssignCase assignCase = new()
            {
                ReqClientid = ReqClientid,
                PhysicianId = PhysicianId,
                RegionId = RegionId,
                Description = Description
            };
            if (ModelState.IsValid)
            {
                _adminService.AssignCase(assignCase);
                return RedirectToAction("AdminDashboard");
            }

            return PartialView("_AssignCaseView", assignCase);
        }
        [HttpPost]
        public IActionResult BlockCase(int ReqClientId, string BlockReason)
        {
            BlockCase blockCase = new()
            {
                ReqClientid = ReqClientId,
                BlockReason = BlockReason
            };
            if (ModelState.IsValid)
            {
                _adminService.BlockCase(blockCase);
                return RedirectToAction("AdminDashboard");
            }
            return PartialView("_BlockCaseView");
        }
        public IActionResult ViewUploads(int reqClientId)
        {
            int? adminId = HttpContext.Session.GetInt32("adminId");
            var obj = _adminService.ViewUploads(reqClientId);

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ViewUploads(ViewUploads viewUploads)
        {
            if (ModelState.IsValid)
            {
                _adminService.UploadFiles(viewUploads);
                return ViewUploads(viewUploads.reqClientId);
            }
            return View(viewUploads);
        }
        public void DeleteFile(int Requestwisefileid)
        {
            _adminService.DeleteFile(Requestwisefileid);
        }
        [HttpPost]        public bool SendFilesViaMail(List<int> fileIds, int requestId)        {            try            {                _adminService.SendFilesViaMail(fileIds, requestId);                return true;            }            catch            {                return false;            }        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("hallodoc");
            return RedirectToAction("AdminLogin", "Home");
        }
        public IActionResult Orders(int reqclientId)
        {
            var obj = _adminService.Orders(reqclientId);
            return View(obj);
        }

        [HttpGet]
        public JsonArray FetchVendors(int selectedValue)        {            var result = _adminService.FetchVendors(selectedValue);            return result;        }

        [HttpGet]        public Healthprofessional VendorDetails(int selectedValue)        {            var result = _adminService.VendorDetails(selectedValue);            return result;        }    }
}
