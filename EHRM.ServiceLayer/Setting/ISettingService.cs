using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Setting;
using EHRM.Web.Models;

namespace EHRM.ServiceLayer.Setting
{
    public interface ISettingService
    {
        #region Customize Login

        Task<Result> CreateCustomLoginAsync(CustomizeSettingViewModel model);
        Task<Result> GetCustomLoginByIdAsync(int id);
       
        Task<Result> UpdateCustomLoginAsync(int id, CustomizeSettingViewModel model);
        #endregion

    }
}
