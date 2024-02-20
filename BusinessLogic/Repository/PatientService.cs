using BusinessLogic.Interface;
using DataAccess.DataContext;
using DataAccess.DataModels;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public PatientDashboard GetMedicalHistory(User user)
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
                int count = _context.Requestwisefiles.Count(rf => rf.Requestid == medicalhistory[i].Requestid);
                fileCount.Add(count);
            }
            return medicalhistory;
        }

        public List<PatientDashboard> GetPatientInfos()
        {
            //var user = _context.Requests.Where(x => x.Email == PatientDashboard.Email.FirstOrDefault());
            return new List<PatientDashboard> { };
        }
    }
}
