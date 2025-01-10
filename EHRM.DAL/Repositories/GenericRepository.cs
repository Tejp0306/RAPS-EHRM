using EHRM.DAL.Database;
using EHRM.DAL.Proc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EHRM.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EhrmContext _context;

        public GenericRepository(EhrmContext context)
        {
            _context = context;
        }

    
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
          
            }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public async Task<List<EmployeeDetail>> GetByRoleIdAsync(int RoleId)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.RoleId == RoleId).ToListAsync();
        }

        public async Task<List<EmployeeDetail>> GetByTeamIdAsync(int TeamId)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.TeamId == TeamId).ToListAsync();
        }

        public async Task<List<EmployeeDetail>> GetByEmpIdDOB(int EmpId, String DOB)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.EmpId == EmpId && x.DateOfBirth==DOB).ToListAsync();
        }

    }


}
