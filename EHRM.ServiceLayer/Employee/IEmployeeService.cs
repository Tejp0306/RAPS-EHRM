using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;


namespace EHRM.ServiceLayer.Employee
{
    public interface IEmployeeService
    {
        Task <Result> SavePersonalInfoAsync(EmployeeViewModel model,String createdById, String filepath);

        Task<Result> GetRoleAsync();

        Task<Result> GetTeamAsync();

        Task<Result> GetEmployeeDataAsync();
    }
}
