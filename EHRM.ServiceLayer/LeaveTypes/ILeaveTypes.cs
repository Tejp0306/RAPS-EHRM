using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Leave;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Review;

namespace EHRM.ServiceLayer.LeaveTypes
{
    public interface ILeaveTypes
    {
        Task<Result> CreateLeaveTypeAsync(LeaveTypeViewModel model);
        Task<Result> UpdateLeaveTypeAsync(int id, LeaveTypeViewModel model);
        Task<Result> GetAllLeaveTypeAsync();
        Task<Result> DeleteLeaveTypeAsync(int id);
        Task<Result> GetLeaveTypeByIdAsync(int id);

        #region Leave Apply

        Task<Result> GetLeaveTypeAsync();
        Task<Result> LeaveApplyAsync(LeaveApplyViewModel model);
        Task<Result> UpdateLeaveApplyAsync(int id, LeaveApplyViewModel model);
        Task<List<LeaveApplyViewModel>> GetLeaveDataByEmpId(int empId);

        #endregion


        #region Leave Status

        Task<Result> GetLeaveAsync();

        Task<Result> LeaveStatusAsync(LeaveStatusViewModel model);

        Task<Result> UpdateLeaveStatusAsync(int id, LeaveStatusViewModel model);

        Task<LeaveBalanceViewModel> GetLeaveBalanceByEmpId(int empId);
        #endregion
    }
}
