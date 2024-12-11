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
    }



}
