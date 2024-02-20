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
        PatientDashboard GetMedicalHistory(User user);

    }
}
