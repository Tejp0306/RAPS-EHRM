using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.ExitFormalities;
using EHRM.ViewModel.PostJoining;

namespace EHRM.ServiceLayer.ExitFormalities
{
    public interface IExitFormalitiesService
    {

        #region Exit Interview

        Task<Result> CreateExitInterviewFormAsync(ExitInterviewViewModel model);

        Task<Result> GetExitInterviewFormAsync(); // this is for displaying at ExitInterview Form details

        Task<Result> GetExitInterviewFormByIdAsync(int Id);

        #endregion

        #region Resignation Form

        Task<Result> CreateResignationFormAsync(ResignationFormViewModel model);
        Task<Result> GetResignationFormAsync(); // this is for displaying at Resignation Form details
        Task<Result> GetResignationFormByIdAsync(int Id);

        #endregion

        #region Employee Undertaking Form

        Task<Result> CreateEmployeeUndertakingFormAsync(EmployeeUndertakingViewModel model);
        Task<Result> GetEmpUndertakingFormAsync(); // this is for displaying at Employee Undertaking Form details
        Task<Result> GetEmpUndertakingFormByIdAsync(int Id);

        #endregion

        #region Exit Checklist

        Task<Result> CreateExitChecklistFormAsync(ExitChecklistViewModel model);
        Task<Result> GetExitChecklistFormAsync(); // this is for displaying at Exit Checklist Form details
        Task<Result> GetExitChecklistFormByIdAsync(int Id);

        #endregion

    }
}
