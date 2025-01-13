using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;


namespace EHRM.ServiceLayer.Employee
{
    public interface IEmployeeService
    {
        Task <Result> SavePersonalInfoAsync(GetAllEmployeeViewModel model,int createdById, String filepath);
        Task<Result> GetRoleAsync();
        Task<Result> GetManagerAsync();
        Task<Result> GetTeamAsync();
        Task<Result> GetEmployeeDataAsync();
        Task<List<GetAllEmployeeViewModel>> GetAllEmployeeRecordDetails(int EmpId);

    }
}
