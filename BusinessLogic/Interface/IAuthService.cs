using DataAccess.DataModels;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.Interface
{
    public interface IAuthService
    {
        bool PatientLogin(PatientLogin patientLogin);
        bool PatientForgotPassword(PatientForgotPassword patientForgotPassword);
        void PatientResetPassword(PatientResetPassword patientResetPassword);
        void CreateNewAccount(CreateNewAccount createNewAccount);
        void ReviewAgreementModal(int ReqClientId);  
        void CancelAgreementSubmit(int ReqClientId, string Description);
    }
}
