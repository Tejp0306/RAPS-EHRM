using EHRM.DAL.Database;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Review;
using EHRM.ViewModel.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Self
{
    public interface ISelfService
    {
        Task<List<EmployeeDetail>> GetDetailsByEmpIdDOB(int EmpId, string DOB);

        //Task<Result> GetDataIdAsync(int EmpId);
        Task<List<GetAllEmployeeViewModel>> GetAllSelfEmployeeRecordDetails(int EmpId);

        Task<List<GetAllEmployeeViewModel>> GetAllEmployeeDataDetails(int EmpId);

        #region TimeSheet
        Task<Result> CreateTimeSheetAsync(TimeSheetViewModel model, List<string> FilePath);
        Task<TimeSheetViewModel> GetTimeSheetByIdAsync(int timesheetId);
        Task<TimeSheetViewModel> GetTimeSheetsByIdAsync(int Id);
        Task<Result> GetTimeSheetByMonthAsync(string month);

        Task<Result> GetFilesAsync(int id);

        #endregion

    }
}
