using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IAdminService
    {
        bool AdminLogin(AdminLogin adminLogin);
        void CreateAdminAccount(CreateAdminAccount createNewAccount);
        AdminDashboard AdminDashboard();
        ViewCaseViewModel ViewCaseViewModel(int reqClientId);
        ViewNotes ViewNotes(int reqClientId);
        public AdminDashboard SearchPatient(SearchViewModel obj, AdminDashboard data);
        void CancelCase(CancelCase cancelCase);
        void AssignCase(AssignCase assignCase);
        void BlockCase(BlockCase blockCase);
        void ViewNotesUpdate(ViewNotes viewNotes);
        ViewUploads ViewUploads(int reqClientId);
        void UploadFiles(ViewUploads viewUploads);
        void DeleteFile(int Requestwisefileid);
        bool SendFilesViaMail(List<int> fileIds, int requestId);
        Orders Orders(int reqClientId); 
    }
}
