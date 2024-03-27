﻿using DataAccess.DataModels;
using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IAdminService
    {
        bool AdminLogin(AdminLogin adminLogin);
        AdminDashboard AdminDashboard();
        ViewCaseViewModel ViewCaseViewModel(int reqClientId);
        ViewNotes ViewNotes(int reqClientId);
        public AdminDashboard SearchPatient(SearchViewModel obj, AdminDashboard data);
        void CancelCase(CancelCase cancelCase);
        void AssignCase(AssignCase assignCase);
        void TransferCase(TransferCase transferCase);
        void BlockCase(BlockCase blockCase);
        void ViewNotesUpdate(ViewNotes viewNotes);
        ViewUploads ViewUploads(int reqClientId);
        void UploadFiles(ViewUploads viewUploads);
        void DeleteFile(int Requestwisefileid);
        bool SendFilesViaMail(List<int> fileIds, int requestId);
        Orders Orders(int reqClientId); 
        JsonArray FetchVendors(int selectedValue);
        JsonArray FetchPhysician(int selectedValue);
        Healthprofessional VendorDetails(int selectedValue);
        void SendOrder(Orders orders);
        void ClearCase(int reqClientId);
        void SendAgreementCase(SendAgreementCase sendAgreement, string link);
        CloseCase CloseCaseView(int reqClientId);   
        void CloseToUnpaidCase(int reqClientId);
        void CloseCaseSave(CloseCase closeCase);
        EncounterModel Encounter(int reqClientId);
        void EncounterSubmit(EncounterModel encounter);
        
        
        /// AdminProfile
        AdminProfile ProfileInfo(int adminId);
        void UpdateProfile(AdminProfile profile);

        /////////////ProviderMenu/////////////////////
        ProviderMenu ProviderMenu();
        void ContactProvider(ContactYourProvider contactYourProvider);
        void StopProviderNotif(int PhysicianId);

        EditPhysicianAccount EditPhysician(int PhysicianId);
        void EditPhysicianAccount(EditPhysicianAccount obj);




        /////////////////////////Access//////////////////////
        List<AccountAccess> AccountAccess();
        CreateAccess FetchRole(short selectedValue);


        void CreateRole(List<int> menuIds,string roleName, short accountType);
        void CreateAdminAccount(CreateAdminAccount createNewAccount);
        void DeleteRole(int roleId);

        void CreateProviderAccount(CreateProviderAccount model);
    }
}
