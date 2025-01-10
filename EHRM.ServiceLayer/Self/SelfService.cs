using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ViewModel.Employee;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Self
{
    public class SelfService : ISelfService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly EhrmContext _context;
        public SelfService(IUnitOfWork unitOfWork, EhrmContext context)
        {
            _UnitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<List<EmployeeDetail>> GetDetailsByEmpIdDOB(int EmpId, string Dob)
        {
            var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository
            var employee = await employeeRepository.GetByEmpIdDOB(EmpId, Dob);  // Fetch employee based on empid and DOB
            //Viewbag.employee=employee;
            return employee;
        }
        

        public async Task<List<GetAllEmployeeViewModel>> GetAllSelfEmployeeRecordDetails(int EmpId)
        {
            var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
            

            // Await the async operations to get actual collections
            var employees = await employeeRepository.GetAllAsync();
            

            // LINQ query to join all related tables using EmpId, with left joins to include null values for missing data
            var employeeWithDetails = (from e in employees
                                       where e.EmpId == EmpId  // Filter by specific EmpId if needed
                                       select new GetAllEmployeeViewModel
                                       {
                                           Id = e.Id,
                                           //PrefixName = e.PrefixName,
                                           EmpId = e.EmpId,
                                           //Title = e.Title,
                                           FirstName = e.FirstName,
                                           MiddleName = e.MiddleName,
                                           LastName = e.LastName,
                                           //LoginId = e.LoginId,
                                           //Password = e.Password,
                                           Age = e.Age ?? 0,  // Null coalescing to handle nullable Age
                                           RoleId = e.RoleId,
                                           //RoleName = GetRoleName(e.RoleId),
                                           DateOfBirth = e.DateOfBirth,
                                           Gender = e.Gender,
                                           //MaritalStatus = e.MaritalStatus,
                                           AadharNumber = e.AadharNumber,
                                           EmailAddress = e.EmailAddress,
                                           HomePhone = e.HomePhone,
                                           CellPhone = e.CellPhone,
                                           //OfficePhone = e.OfficePhone,
                                           TeamId = e.TeamId,
                                           TeamName = GetTeamName(e.TeamId),
                                           //MarriageAnniversary = e.MarriageAnniversary,
                                           Street = e.Street,
                                           City = e.City,
                                           Country = e.Country,
                                           ZipCode = e.ZipCode,
                                           //Nationality = e.Nationality,
                                           CreatedAt = e.CreatedAt,
                                           UpdatedAt = e.UpdatedAt,
                                           FileName = e.Image,
                                       
                               
                                       }).ToList();

            return employeeWithDetails;
        }
        private string GetTeamName(int teamid)
        {
            var teamName = _context.Teams.Where(x => x.Id == teamid).Select(x => x.Name).FirstOrDefault();
            return teamName != null ? teamName : "N.A";  // Fixed missing semicolon
        }
    }
}
