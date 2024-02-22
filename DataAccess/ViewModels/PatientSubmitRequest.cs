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
        public string? Symptoms { get; set; }
        [Required(ErrorMessage = "Please Enter Your Firstname")]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        [Required(ErrorMessage = "Please Enter Your Email")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        //[Required(AllowEmptyStrings = true)]
        //[DataType(DataType.Password)]
        public string? Password { get; set; }

        //[Required(AllowEmptyStrings = true)]
        //[DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Room { get; set; }
        public IFormFile? File { get; set; }
    }
    public class FamilyFriendSubmitRequest
    {
        [Required(ErrorMessage ="Enter Your FirstName")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage ="Enter Your Phone Number")]
        public string Phone { get; set; }
        [Required(ErrorMessage ="Enter Your Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Enter Your Relationship with Patient")]
        public string RelationWithPatient { get; set; }
        public string? Symptoms { get; set; }
        [Required(ErrorMessage ="Enter Patient's FirstName")]
        public string PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        [Required(ErrorMessage ="Enter Patient's Date of birth")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage ="Enter Patient's Email")]
        public string PatientEmail { get; set; }
        [Required(ErrorMessage ="Enter Patient's Phone Number")]
        public string PatientPhone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Room { get; set; }
        public IFormFile? File { get; set; }

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
    public class PatientDashboard
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<Request> Requests { get; set; }
        public List<int> DocumentCount { get; set; }
    }

    public class PatientProfile
    {
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Location { get; set; }
    }

}
