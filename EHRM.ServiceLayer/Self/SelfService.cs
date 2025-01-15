using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
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

        public async Task<List<GetAllEmployeeViewModel>> GetAllEmployeeDataDetails(int EmpId)
        {
            var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
            var employmentTypeRepository = _UnitOfWork.GetRepository<EmployementTypeDetail>();
            var qualificationRepository = _UnitOfWork.GetRepository<Qualification>();
            var salaryRepository = _UnitOfWork.GetRepository<Salary>();

            // Await the async operations to get actual collections
            var employees = await employeeRepository.GetEmployeeDetailsByIdAsync(EmpId);
            var employmentTypes = await employmentTypeRepository.GetEmployementTypeDetailsByIdAsync(EmpId);
            var qualifications = await qualificationRepository.GetQualificationDetailsByIdAsync(EmpId);
            var salaries = await salaryRepository.GetSalaryDetailsByIdAsync(EmpId);


            // LINQ query to join all related tables using EmpId, with left joins to include null values for missing data
            var employeeWithDetails = (from e in employees
                                       join etype in employmentTypes on e.EmpId equals etype.EmpId into etypes
                                       from etype in etypes.DefaultIfEmpty()
                                       join q in qualifications on e.EmpId equals q.EmpId into quals
                                       from qualification in quals.DefaultIfEmpty()
                                       join s in salaries on e.EmpId equals s.EmpId into sal
                                       from salary in sal.DefaultIfEmpty()
                                       where e.EmpId == EmpId  // Filter by specific EmpId if needed
                                       select new GetAllEmployeeViewModel
                                       {
                                           Id = e.Id,
                                           EmpId = e.EmpId,
                                           FirstName = e.FirstName,
                                           MiddleName = e.MiddleName,
                                           LastName = e.LastName,
                                           Age = e.Age ?? 0,  // Null coalescing to handle nullable Age
                                           EmailAddress = e.EmailAddress,
                                           CellPhone = e.CellPhone,
                                           Street = e.Street,
                                           City = e.City,
                                           Country = e.Country,
                                           ZipCode = e.ZipCode,
                                           Nationality = e.Nationality,
                                           CreatedAt = e.CreatedAt,
                                           UpdatedAt = e.UpdatedAt,
                                           SalaryDetails = salary == null ? new SalaryViewModel() : new SalaryViewModel
                                           {
                                               Id = salary.Id,
                                               Ctc = salary.Ctc,
                                               StartYear = salary.StartYear,
                                               EndYear = salary.Endyear,
                                               Description = salary.Description
                                           },
                                           Qualifications = qualification == null ? new QualificationViewModel() : new QualificationViewModel
                                           {
                                               Id = qualification.Id,
                                               CourseName = qualification.CourseName,
                                               InstitutionName = qualification.InstitutionName,
                                               PassedDate = qualification.PassedDate,
                                               Details = qualification.Details
                                           },

                                           EmploymentDetails = etype == null ? new EmploymentTypeDetailViewModel() : new EmploymentTypeDetailViewModel
                                           {
                                               Id = etype.Id,
                                               EmpType = etype.EmpType,
                                               //EmpTypeName = GetEmpTypeName(etype.EmpType),
                                               AppointmentDate = etype.AppointmentDate,
                                               StartDate = etype.StartDate,
                                               EndDate = etype.EndDate,
                                               TotalService = etype.TotalService,
                                               AppointedService = etype.AppointedService
                                           },

                                       }).ToList();

            return employeeWithDetails;
        }

    }
}
