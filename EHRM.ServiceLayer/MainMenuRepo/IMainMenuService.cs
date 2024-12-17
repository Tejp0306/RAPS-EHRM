using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.MainMenu;
using EHRM.ViewModel.Master;

namespace EHRM.ServiceLayer.MainMenuRepo
{
    public interface IMainMenuService
    {

        Task <Result> CreateMainMenuAsync(MainMenuViewModel model);
        Task<Result> GetAllMainMenuAsync();

        Task<Result> DeleteMainMenuAsync(int id);

        Task<Result> GetmainmenuByIdAsync(int id);

        Task<Result> UpdateMainMenuAsync(int id, string updatedBy, MainMenuViewModel model);
    }
}