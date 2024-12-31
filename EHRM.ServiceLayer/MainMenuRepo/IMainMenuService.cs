using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.MainMenu;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Proc;
using EHRM.ViewModel.SubMenu;

namespace EHRM.ServiceLayer.MainMenuRepo
{
    public interface IMainMenuService
    {

        Task <Result> CreateMainMenuAsync(MainMenuViewModel model);
        Task<Result> GetAllMainMenuAsync();

        Task<Result> DeleteMainMenuAsync(int id);

        Task<Result> GetmainmenuByIdAsync(int id);

        Task<Result> UpdateMainMenuAsync(int id, string updatedBy, MainMenuViewModel model);

        Task<Result> GetMainMenuIdAsync();
        Task<Result> GetRoleAsync();

        Task<Result> GetEmployeeByRoleIdAsync(int id);

        Task<Result> CreateSubMenuAsync(SubMenuViewModel model);

        //Task<Result> GetAllSubMenuAsync();
        
        //Get SubMenu data
        Task<Result> GetSubMenuAsync();

        //for get data from store procedure
        Task<List<SubMenuProcDetails>> CallSubMenuStoredProcedureAsync(int param1);

        Task<Result> DeleteSubMenuAsync(int id);


        Task<Result> GetsubmenuByIdAsync(int id);

        Task<Result> UpdateSubMenuAsync(int id, string updatedBy, SubMenuViewModel model);



    }
}