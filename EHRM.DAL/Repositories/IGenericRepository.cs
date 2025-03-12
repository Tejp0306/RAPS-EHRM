using EHRM.DAL.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<List<EmployeeDetail>> GetByRoleIdAsync(int RoleId);
        Task <List<EmployeePunchDetail>> GetByEmpIdAsync(int EmpId);
  
        Task<List<EmployeeDetail>> GetByTeamIdAsync(int RoleId);
        Task<List<EmployeesDeclaration>> GetByDeclarationEmpIdDOB(int EmpId, DateTime DOB);

        Task<List<EmployeeDetail>> GetByEmpIdDOB(int EmpId, String DOB);
        //saksham changes
        Task<List<EmployeeDetail>> GetEmployeeDetailsByIdAsync(int? EmpId);

        Task<List<EmployeesCred>> GetEmployeeCredByIdAsync(int EmpId);

        Task<List<EmployeesCred>> GetEmployeeEmailPasswordByEmailAsync(String email);
        Task<List<Qualification>> GetQualificationDetailsByIdAsync(int EmpId);
        Task<List<EmployementTypeDetail>> GetEmployementTypeDetailsByIdAsync(int EmpId);
        Task<List<Salary>> GetSalaryDetailsByIdAsync(int EmpId);

        Task<List<EmployementTypeDetail>> FindByIdAsync(int ManagerId);

        Task<List<Declaration>> GetDeclarationDetailsByIdAsync(int EmpId);



    }
}
