using EHRM.DAL.Database;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;


namespace EHRM.ServiceLayer.Employee
{
    public interface IEmployeeService
    {
        Task <Result> SavePersonalInfoAsync(EmployeeViewModel model,int createdById, String filepath);
        Task<Result> GetRoleAsync();
        Task<Result> GetTeamAsync();
        Task<Result> GetEmployeeDataAsync();
        Task<List<GetAllEmployeeViewModel>> GetAllEmployeeRecordDetails(int EmpId);

        #region Employee Declaration

        Task<Result> CreateDeclarationAsync(EmployeeDeclarationViewModel model);

        Task<List<EmployeesDeclaration>> GetEmployeeDetailsByEmpIdDOB(int EmpId, DateTime DOB);

        Task<List<EmployeeDeclarationViewModel>> GetAllEmployeeProfileDetails(int EmpId);
        #endregion
    }
}
