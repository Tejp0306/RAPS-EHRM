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
        public async Task<List<EmployeePunchDetail>> GetByEmpIdAsync(int EmpId)
        {
            var data=  await _context.Set<EmployeePunchDetail>().ToListAsync();
            return await _context.Set<EmployeePunchDetail>().Where(x => x.Empid == EmpId).ToListAsync();

        }
        public async Task<NonDisclosureAgreement> GetEmployeeNDAByIdAsync(int empId)
        {
            var data = await _context.Set<NonDisclosureAgreement>().ToListAsync();
            return await _context.Set<NonDisclosureAgreement>().Where(x => x.EmpId == empId).FirstAsync();

        }
        public async Task<PersonalInfo> GetEmployeePersonalInfoByIdAsync(int empId)
        {
            var data = await _context.Set<PersonalInfo>().ToListAsync();
            return await _context.Set<PersonalInfo>().Where(x => x.EmpId == empId).FirstAsync();

        }
        public async Task<ClientPropertyDeclaration> GetEmployeePropertyDeclarationByIdAsync(int empId)
        {
            var data = await _context.Set<ClientPropertyDeclaration>().ToListAsync();
            return await _context.Set<ClientPropertyDeclaration>().Where(x => x.EmpId == empId).FirstAsync();

        }
        public async Task<AcknowledgementForm> GetEmployeeAcknowledgementByIdAsync(int empId)
        {
            var data = await _context.Set<AcknowledgementForm>().ToListAsync();
            return await _context.Set<AcknowledgementForm>().Where(x => x.EmpId == empId).FirstAsync();

        }

        public async Task<List<Bgvform>> GetBGVByEmpIdAsync(int EmpId)
        {
            var data = await _context.Set<Bgvform>().ToListAsync();
            return await _context.Set<Bgvform>().Where(x => x.EmpId == EmpId).ToListAsync();

        }

        public async Task<List<PreviousEmployment>> GetPreviousEmploymentByEmpIdAsync(int EmpId)
        {
            var data = await _context.Set<PreviousEmployment>().ToListAsync();
            return await _context.Set<PreviousEmployment>().Where(x => x.EmpId == EmpId).ToListAsync();

        }



        public async Task<List<EmployeeDetail>> GetByRoleIdAsync(int RoleId)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.RoleId == RoleId).ToListAsync();
        }


        //Get Leave Status By Leave Id
        public async Task<List<LeaveStatuss>> GetStatusByLeaveIdAsync(int leaveid)
        {
            return await _context.Set<LeaveStatuss>().Where(x => x.LeaveId == leaveid).ToListAsync();
        }


        public async Task<List<EmployeeDetail>> GetByTeamIdAsync(int TeamId)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.TeamId == TeamId).ToListAsync();
        }



        public async Task<List<EmployeesDeclaration>> GetByDeclarationEmpIdDOB(int EmpId, DateTime DOB)
        {
            return await _context.Set<EmployeesDeclaration>().Where(x => x.EmpId == EmpId && x.DateOfBirth == DOB).ToListAsync();
        }

        public async Task<List<EmployeeDetail>> GetByEmpIdDOB(int EmpId, String DOB)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.EmpId == EmpId && x.DateOfBirth==DOB).ToListAsync();
        }

        // saksham changes
        public async Task<List<EmployeeDetail>> GetEmployeeDetailsByIdAsync(int? EmpId)
        {
            return await _context.Set<EmployeeDetail>().Where(x => x.EmpId == EmpId).ToListAsync();
        }


        //Get Emplyees details from employee cred table using empid
        public async Task<List<EmployeesCred>> GetEmployeeCredByIdAsync(int EmpId)
        {
            return await _context.Set<EmployeesCred>().Where(x => x.EmpId == EmpId).ToListAsync();
        }

        //Get Employees password and email from employee cred based on email

        public async Task<List<EmployeesCred>> GetEmployeeEmailPasswordByEmailAsync(string email)
        {
            return await _context.Set<EmployeesCred>()
                .Where(e => e.Email.ToLower() == email.ToLower())
                .Select(e => new EmployeesCred
                {
                    Email = e.Email,
                    TempPassword = e.TempPassword,
                    FirstName = e.FirstName
                })
                .ToListAsync();
        }


        public async Task<List<Qualification>> GetQualificationDetailsByIdAsync(int EmpId)
        {
            return await _context.Set<Qualification>().Where(x => x.EmpId == EmpId).ToListAsync();
        }
        public async Task<List<EmployementTypeDetail>> GetEmployementTypeDetailsByIdAsync(int EmpId)
        {
            return await _context.Set<EmployementTypeDetail>().Where(x => x.EmpId == EmpId).ToListAsync();
        }
        public async Task<List<Salary>> GetSalaryDetailsByIdAsync(int EmpId)
        {
            return await _context.Set<Salary>().Where(x => x.EmpId == EmpId).ToListAsync();
        }

        public async Task<List<EmployementTypeDetail>> FindByIdAsync(int ManagerId)
        {
            // Using asynchronous LINQ to filter based on ManagerId
            return await _context.Set<EmployementTypeDetail>()
                .Where(x => x.ManagerId == ManagerId)
                .ToListAsync();
        }
        public async Task<List<Declaration>> GetDeclarationDetailsByIdAsync(int EmpId)
        {
            return await _context.Set<Declaration>().Where(x => x.EmpId == EmpId).ToListAsync();
        }

      
    }


}
