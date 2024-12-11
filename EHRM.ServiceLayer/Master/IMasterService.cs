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
        #region AddNoticeBoard
        Task<Result> CreateAddNoticeBoardAsync(AddNoticeBoardViewModel model, int createdBy,string? filepath);
        Task<Result> UpdateAddNoticeBoardAsync(int id, int updatedBy, AddNoticeBoardViewModel model);
        Task<Result> DeleteAddNoticeBoardAsync(int id);
        Task<Result> GetAllAddNoticeBoardAsync();
        Task<Result> GetAllAddNoticeBoardByIdAsync(int id);

        Task<string> GetFilePathByIdAsync(int id);






        #endregion
    }

}
