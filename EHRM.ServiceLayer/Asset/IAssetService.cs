using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Master;

namespace EHRM.ServiceLayer.Asset
{
    public interface IAssetService
    {
        Task<Result> GetTeamAsync();
        Task<Result> GetEmployeeByTeamIdAsync(int id);
        Task<Result> DeleteAssetAsync(int id);
        Task<Result> CreateAssetAsync(AssetViewModel model, int createdBy);
        Task<Result> UpdateAssetAsync(int id, int updatedBy, AssetViewModel model);
        Task<Result> GetAssetByIdAsync(int id);
        Task<Result> GetAllAssetAsync();
        Task<string> GetTeamByIdAsync(int id);
    }
}
