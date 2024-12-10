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
    }

}
