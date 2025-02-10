using EHRM.DAL.Database;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Master;


namespace EHRM.ServiceLayer.Employee
{
    public interface IEmployeeService
    {
        Task <Result> SavePersonalInfoAsync(GetAllEmployeeViewModel model,int createdById, String filepath);
        Task<Result> SaveQualificationInfoAsync(GetAllEmployeeViewModel model, int createdById);

        Task<Result> SaveSalaryInfoAsync(GetAllEmployeeViewModel model, int createdById);

        Task<Result> SaveDecalarationInfoAsync(GetAllEmployeeViewModel model, int createdById);

        
        Task<Result> SaveEmploymentInfoAsync(GetAllEmployeeViewModel model, int createdById);
        Task<Result> GetRoleAsync();
        Task<Result> GetManagerAsync();
        Task<Result> GetTeamAsync();
        Task<Result> GetEmployeeDataAsync();
        Task<List<GetAllEmployeeViewModel>> GetAllEmployeeRecordDetails(int EmpId);

        Task<Result> GetEmployeeDataByEmpIdAsync(int EmpId);

        Task<Result> UpdatePersonalInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model);

        Task<Result> UpdateEmploymentInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model);

        Task<Result> UpdateQualificationInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model);

        Task<Result> UpdateSalaryInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model);

        Task<Result> UpdateDeclarationInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model);

        







        bool CheckUserInDbByEmpId(int? EmpId);

        bool CheckUserInEmpCredDbByEmpId(int? EmpId);

        




        #region Employee Declaration

        Task<Result> CreateDeclarationAsync(EmployeeDeclarationViewModel model);

        Task<List<EmployeesDeclaration>> GetEmployeeDetailsByEmpIdDOB(int EmpId, DateTime DOB);

        Task<List<EmployeeDeclarationViewModel>> GetAllEmployeeProfileDetails(int EmpId);
        #endregion
    }
}
