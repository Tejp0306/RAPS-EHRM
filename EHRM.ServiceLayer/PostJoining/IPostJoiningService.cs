
﻿using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.MasterEmployee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.PostJoining;
using EHRM.ViewModel.Self;


namespace EHRM.ServiceLayer.PostJoining
{
    public interface IPostJoiningService
    {

        Task<bool> SaveMasterSheetAsync(EmployeeFormViewModel model);

        Task<bool> SaveMasterContactAsync(EmployeeFormViewModel model);

        Task<bool> SaveMasterAddressAsync(EmployeeFormViewModel model);
        Task<bool> SaveMasterEducationAsync(EmployeeFormViewModel model);

        Task<bool> SaveMasterExperienceAsync(EmployeeFormViewModel model);
        Task<bool> SaveMasterBankAsync(EmployeeFormViewModel model);
        Task<bool> SaveMasterEmergencyAsync(EmployeeFormViewModel model);
        Task<bool> SaveMasterReportingAsync(EmployeeFormViewModel model);
        Task<bool> SaveMasterFamilyAsync(EmployeeFormViewModel model);
        Task<bool> SaveMasterDependentAsync(EmployeeFormViewModel model);
        Task<bool> SaveBGVFormAsync(BGVViewModel model);

        public EmployeeFormViewModel GetMasterSheetDataAsync(int EmpId);

        Task<bool> SavePreviousEmploymentsAsync(EmploymentViewModel model);

        Task<BGVFormDTO> GetEmployeeDetailsAsync(int empId);

        Task<Result> GetBackGroundFormDetailsAsync();

        Task<Result> GetMasterSheetFormDetailsAsync();

        #region Acknowldegement Form
        Task<Result> CreateAcknowldegementFormAsync(AcknowledgementFormViewModel model);
        Task<Result> GetAllAcknowledgeFormAsync();
        Task<Result> GetAcknowledgeFormAsync(); // this is for displaying at Acknowldegement Form details
        Task<Result> GetAcknowldegementFormByIdAsync(int empId);
        Task<Result> GetAcknowldegementAsync(int Id);
        #endregion


        #region Personal Information
        Task<Result> CreatePersonalInformationFormAsync(PersonalInfomationViewModel model);
        Task<Result> GetAllPersonalInformationFormAsync();
        Task<Result> GetPersonalInfoByIdAsync(int empId);
        Task<Result> GetPersonalInfoAsync(int Id);

        Task<Result> GetPersonalInfoFormAsync(); // this is for displaying at PersonalInfo Form details

        #endregion


        #region Client Propert Declaration

        Task<Result> CreateClientPropertDeclarationFormAsync(ClientPropertyDeclarationViewModel model);

        Task<Result> GetAllClientPropertDeclarationFormAsync();

        Task<Result> GetClientPropertDeclarationByIdAsync(int empId);
        Task<Result> GetClientPropertDecByIdAsync(int Id);

        Task<Result> GetClientPropertDeclarationFormAsync(); // this is for displaying at ClientPropertDeclaration Form details


        #endregion


        #region NDA Form

        Task<Result> CreateNDAFormAsync(NDAFormViewModel model);

        Task<Result> GetAllNDAFormAsync();
        Task<Result> GetNDAByIdAsync(int Id);

        Task<Result> GetNDAFormByIdAsync(int empId);

        Task<Result> GetNDAFormAsync(); // this is for displaying at NDA Form details


        #endregion


    }
}
