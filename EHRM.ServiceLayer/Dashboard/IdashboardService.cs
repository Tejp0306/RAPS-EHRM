using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.PunchDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Dashboard
{
    public interface IdashboardService
    {
        List<EmployeeViewModel> GetEmployeesByManager(int managerId);
        List<EmployeeViewModel> GetAllEmployeeDataForAdmin();

        List<EmployeeViewModel> GetDataForUserDashboard(int userempId);

        #region Punch Details
        Task<Result> SavePunchInAsync(int EmpId, string userName);
        Task<Result> UpdatePunchOutAsync(int EmpId);
        #endregion
        
    }
}
