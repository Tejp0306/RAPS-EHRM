using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.MasterEmployee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
