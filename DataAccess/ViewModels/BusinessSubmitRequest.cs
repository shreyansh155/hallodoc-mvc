using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class BusinessSubmitRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string CaseNumber { get; set; }
        public string Symptoms { get; set; }
        [Required]
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        [Required]

        public string PatientDateOfBirth { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PatientRoom { get; set; }
        public IFormFile File { get; set; }

    }
}
