using EHRM.DAL.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<List<EmployeeDetail>> GetByTeamIdAsync(int RoleId);

        Task<List<EmployeeDetail>> GetByEmpIdDOB(int EmpId, String DOB);
        



    }
}
