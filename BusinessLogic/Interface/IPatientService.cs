using DataAccess.DataModels;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IPatientService
    {

        void PatientRequest(PatientSubmitRequest userDetails);
        void FamilyFriendRequest(FamilyFriendSubmitRequest userDetails);
        void ConciergeRequest(ConciergeSubmitRequest userDetails);
        void BusinessRequest(BusinessSubmitRequest userDetails);
        List<PatientDashboard> GetMedicalHistory(User user);

    }
}
