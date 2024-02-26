using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class AdminLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class CreateAdminAccount
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;

    }
    public class AdminDashboard
    {
        public countRequestViewModel? countRequestViewModel { get; set; }

        //public searchViewModel searchViewModel { get; set; }

        public IEnumerable<newReqViewModel>? newReqViewModel { get; set; }

        public IEnumerable<pendingReqViewModel>? pendingReqViewModel { get; set; }

        public IEnumerable<concludeReqViewModel>? concludeReqViewModel { get; set; }

        public IEnumerable<closeReqViewModel>? closeReqViewModels { get; set; }

        public IEnumerable<unpaidReqViewModel>? unpaidReqViewModels { get; set; }
        public IEnumerable<activeReqViewModel>? activeReqViewModels { get; set; }
    }
    public class concludeReqViewModel
    {
        public int reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
    }
    public class closeReqViewModel
    {
        public int reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
    }
    public class activeReqViewModel
    {
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
        public int reqClientId { get; set; }
    }
    public class countRequestViewModel
    {
        public int newCount { get; set; }

        public int pendingCount { get; set; }

        public int activeCount { get; set; }

        public int concludeCount { get; set; }
        public int closeCount { get; set; }
        public int unpaidCount { get; set; }
    }
    public class newReqViewModel
    {
        public int reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
    }
    public class pendingReqViewModel
    {
        public int reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
    }
    public class unpaidReqViewModel
    {
        public int reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }

        public string? Phonenumber { get; set; }

        public string? ConciergePhonenumber { get; set; }

        public string? BusinessPhonenumber { get; set; }

        public string? FamilyPhonenumber { get; set; }

        public string? Email { get; set; }

        public int? Regionid { get; set; }

        public short Status { get; set; }

        public int? Physicianid { get; set; }

        public DateTime Createddate { get; set; }

        public string? Notes { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public int? reqTypeId { get; set; }

        public string? physicianName { get; set; }
    }
    public class viewCaseViewModel
    {
        public int Requestclientid { get; set; }

        public int? Requestid { get; set; }

        public string Firstname { get; set; } = null!;

        public string? Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phonenumber { get; set; }

        public string? Address { get; set; }

        public int? Regionid { get; set; }

        public string? Notes { get; set; }

        public string? Email { get; set; }

        public string? Strmonth { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }
        public string? Room { get; set; }

        public String confirmationNumber { get; set; }
    }
}