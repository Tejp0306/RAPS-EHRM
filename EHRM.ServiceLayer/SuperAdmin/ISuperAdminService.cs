using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.PostJoining;
using EHRM.ViewModel.Self;
using EHRM.ViewModel.SuperAdmin;

namespace EHRM.ServiceLayer.SuperAdmin
{
    public interface ISuperAdminService
    {
        #region Tenant Registration

        Task<Result> CreateTenantRegistrationFormAsync(TenantRegistrationViewModel model);
        Task<Result> GetTenantRegistrationFormAsync(int id);
        Task<Result> UpdateTenantFormAsync(int id,TenantRegistrationViewModel model);
        Task<Result> GetTenantFormAsync(); // this is for displaying at Tenant Form details

        Task<Result> GetTenantDetailsAsync(int Id);

        #endregion
    }
}
