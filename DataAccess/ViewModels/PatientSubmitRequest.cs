using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataModels;

namespace DataAccess.ViewModels
{
    public class PatientSubmitRequest
    {
        public string Symptoms { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Room { get; set; }
        public string Document { get; set; }
        public IFormFile File { get; set; }
    }
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
    public class ConciergeSubmitRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PropertyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string RelationWithPatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string Symptoms { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public string PatientStreet { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string PatientRoom { get; set; }
        public IFormFile File { get; set; }


    }
    public class FamilyFriendSubmitRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RelationWithPatient { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string Symptoms { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Room { get; set; }
        public IFormFile File { get; set; }

    }
    public class PatientDashboard
    {
        public int reqId { get; set; }
        public string Name { get; set; }
        public DateTime Createddate { get; set; }
        public List<int> FileCount { get; set; }
        public string Status { get; set; }
        public List<Request> requests { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

    }

}
