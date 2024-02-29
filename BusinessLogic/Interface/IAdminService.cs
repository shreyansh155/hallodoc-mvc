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
    }
}
