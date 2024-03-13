using DataAccess.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class AdminLogin
    {
        [Required]
        public string? Email { get; set; }
        [Required]
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
    public class ConcludeReqViewModel
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

        public int RequestId { get; set; }
        public string? physicianName { get; set; }
    }
    public class CloseReqViewModel
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
        public int RequestId { get; set; }
    }
    public class ActiveReqViewModel
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
        public int RequestId { get; set; }

    }
    public class CountRequestViewModel
    {
        public int newCount { get; set; }

        public int pendingCount { get; set; }

        public int activeCount { get; set; }

        public int concludeCount { get; set; }
        public int closeCount { get; set; }
        public int unpaidCount { get; set; }
        public IEnumerable<Region>? RegionList { get; set; }

    }
    public class NewReqViewModel
    {

        public int? reqClientId { get; set; }
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? reqFirstname { get; set; }

        public string? reqLastname { get; set; }
        public IEnumerable<Casetag>? Casetags { get; set; }
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
    public class PendingReqViewModel
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
    public class UnpaidReqViewModel
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
    public class ViewCaseViewModel
    {
        public int Status { get; set; }
        public int Requestclientid { get; set; }
        public int? Requestid { get; set; }
        public string? Firstname { get; set; } = null!;
        public string? Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
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
        public string? ConfirmationNumber { get; set; }
    }
    public class ViewNotes
    {
        public int Status { get; set; }

        public int Requestclientid { get; set; }
        public string? PhysicianNotes { get; set; }
        public string? PhysicianName { get; set; }
        public string? AdminNotes { get; set; }
        public string? TransferNotes { get; set; }
        public string? TextBox { get; set; }
        public List<Requeststatuslog>? Statuslogs { get; set; }
    }
    public class SearchViewModel
    {
        public string? Name { get; set; }

        public string? Sorting { get; set; }
    }
    public class CancelCase
    {
        public int? ReqClientid { get; set; }
        public string? Name { get; set; }
        public int? CaseTagId { get; set; }
        public IEnumerable<Casetag> Casetags { get; set; }
        public string? AddOnNotes { get; set; }
    }
    public class AssignCase
    {
        public int? ReqClientid { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Region> Region { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        public int? PhysicianId { get; set; }
        public int? RegionId { get; set; }
        public string? Description { get; set; }
    }
    public class BlockCase
    {
        public int? ReqClientid { get; set; }
        public string? Name { get; set; }
        public string? BlockReason { get; set; }
    }
    public class ViewUploads
    {
        public int reqClientId { get; set; }
        public int Status { get; set; }
        public string? PatientName { get; set; }
        public string? ConfirmationNumber { get; set; }
        public IFormFile? File { get; set; }
        public int RequestId { get; set; }
        public List<Requestwisefile>? RequestWiseFiles { get; set; }
        public int Requestwisefileid { get; set; }
    }
    public class AdminDashboard
    {
        public CountRequestViewModel? CountRequestViewModel { get; set; }

        public SearchViewModel? SearchViewModel { get; set; }
        public string? Name { get; set; }
        public string? Sorting { get; set; }
        public int Status { get; set; }
        public IEnumerable<NewReqViewModel>? NewReqViewModel { get; set; }

        public IEnumerable<PendingReqViewModel>? PendingReqViewModel { get; set; }

        public IEnumerable<ConcludeReqViewModel>? ConcludeReqViewModel { get; set; }

        public IEnumerable<CloseReqViewModel>? CloseReqViewModels { get; set; }

        public IEnumerable<UnpaidReqViewModel>? UnpaidReqViewModels { get; set; }
        public IEnumerable<ActiveReqViewModel>? ActiveReqViewModels { get; set; }
    }
    public class Orders
    {
        public int Status { get; set; }
        public List<Healthprofessionaltype> ProfessionTypes { get; set; }
        public List<Healthprofessional> HealthProfessionals { get; set; }
        public int Requestid { get; set; }
        public int Professionid { get; set; }
        public int Vendorid { get; set; }
        public string? BusinessContact { get; set; }
        public string? Email { get; set; }
        public string? FaxNumber { get; set; }
        public string? Prescription { get; set; }
        public int NumberOfRefills { get; set; }

    }
    public class TransferCase
    {
        public int? ReqClientid { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Region> Region { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        public int? PhysicianId { get; set; }
        public int? RegionId { get; set; }
        public string? Description { get; set; }
    }
    public class ClearCase
    {
        public int? ReqClientid { get; set; }

    }
    public class SendAgreementCase
    {
        public int? ReqClientid { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

    }
    public class CloseCase
    {
        public int Status { get; set; }
        public int? ReqClientid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Requestwisefile>? Files { get; set; }
        public string? ConfirmationNumber { get; set; }
    }
    public class EncounterModel
    {
        public int Status { get; set; }
        public int? ReqClientid { get; set; }
        public int? EncounterId { get; set; }
        public int? Requestid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateOnly Date { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PatientHistory { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Medications { get; set; }
        public string? Allergies { get; set; }
        public string? Temp { get; set; }
        public string? Hr { get; set; }
        public string? Rr { get; set; }
        public string? BloodPressureS { get; set; }
        public string? BloodPressureD { get; set; }
        public string? O2 { get; set; }
        public string? Pain { get; set; }
        public string? Heent { get; set; }
        public string? CV { get; set; }
        public string? Chest { get; set; }
        public string? ABD { get; set; }
        public string? Skin { get; set; }
        public string? Extr { get; set; }
        public string? Neuro { get; set; }
        public string? Other { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? MedicationsDispensed { get; set; }
        public string? Procedures { get; set; }
        public string? Followup { get; set; }
        public string? Location { get; set; }

    }
}