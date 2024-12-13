using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Master
{
    public interface IMasterService
    {
        Task<Result> CreateRoleAsync(RoleViewModel model, string createdBy);
        Task<Result> UpdateRoleAsync(int id, string updatedBy, RoleViewModel model);
        Task<Result> DeleteRoleAsync(int id);
        Task<Result> GetRoleByIdAsync(int id);
        Task<Result> GetAllRolesAsync();

        #region Team Screen
        Task<Result> CreateTeamAsync(TeamScreenViewModel model, int createdBy);
        Task<Result> UpdateTeamAsync(int id, int updatedBy, TeamScreenViewModel model);
        Task<Result> DeleteTeamAsync(int id);
        Task<Result> GetTeamByIdAsync(int id);
        Task<Result> GetAllTeamAsync();

        #endregion
        #region
        //Holiday Screen
        Task<Result> CreateHolidayAsync(HolidayViewModel model, string createdBy);

        //Task<Result> UpdateHolidayAsync(int id, string updatedBy, HolidayViewModel model);

        Task<Result> GetAllHolidayAsync();

        Task<Result> DeleteHolidayAsync(int id);

        Task<Result> UpdateHolidayAsync(int id, string updatedBy, HolidayViewModel model);

        Task<Result> GetTeamAsync();

        //Task<Result> GetAllHolidayAsync();
        Task<Result> GetHolidayByIdAsync(int id);


        #endregion

        #region Employee_Type
        Task<Result> CreateEmployeeTypeAsync(EmployeeTypeViewModel model);
        Task<Result> UpdateEmployeeTypeAsync(int id, EmployeeTypeViewModel model);
        Task<Result> DeleteEmployeeTypeAsync(int id);
        Task<Result> GetEmployeeTypeIdAsync(int id);
        Task<Result> GetAllEmployeeTypeAsync();
        #endregion

    }



}
