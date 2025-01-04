using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<Result> SavePersonalInfoAsync(EmployeeViewModel model, int createdById, String filepath)
        {
            try
            {
                var newEmployeeDetails = new EmployeeDetail

                {
                    AadharNumber = model.AadharNumber,
                    CellPhone = model.CellPhone,
                    City = model.City,
                    Country = model.Country,
                    DateOfBirth = model.DateOfBirth,
                    EmailAddress = model.EmailAddress,
                    EmpId = model.EmpId,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    HomePhone = model.HomePhone,
                    LastName = model.LastName,
                    MaritalStatus = model.MaritalStatus,
                    MarriageAnniversary = model.MarriageAnniversary,
                    MiddleName = model.MiddleName,
                    Nationality = model.Nationality,
                    OfficePhone = model.OfficePhone,
                    Password = model.Password,
                    PrefixName = model.PrefixName,
                    Image = filepath,
                    RoleId = model.RoleId,
                    Street = model.Street,
                    TeamId = model.TeamId,
                    Title = model.Title,
                    ZipCode = model.ZipCode,
                    LoginId = model.LoginId,
                    Age = model.Age,
                    IsProfileCompleted = false,
                    Active = true,
                    CreatedById = createdById
                };
                var employeedetailRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
                await employeedetailRepository.AddAsync(newEmployeeDetails);
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employee created successfully."
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> GetRoleAsync()
        {

            var roleRepository = _UnitOfWork.GetRepository<Role>();
            var role = await roleRepository.GetAllAsync();
            return new Result { Success = true, Data = role };

        }
        public async Task<Result> GetTeamAsync()
        {
            try
            {
                var teamsRepository = _UnitOfWork.GetRepository<Team>();
                var teams = await teamsRepository.GetAllAsync();
                return new Result { Success = true, Data = teams };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> GetEmployeeDataAsync()
        {
            try
            {
                var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository
                var employee = await employeeRepository.GetAllAsync();  // Fetch all roles
                return new Result { Success = true, Data = employee };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GetAllEmployeeViewModel>> GetAllEmployeeRecordDetails(int EmpId)
        {
            var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();
            var employmentTypeRepository = _UnitOfWork.GetRepository<EmployementTypeDetail>();
            var qualificationRepository = _UnitOfWork.GetRepository<Qualification>();
            var salaryRepository = _UnitOfWork.GetRepository<Salary>();
            var declartionRepository = _UnitOfWork.GetRepository<Declaration>();

            // Await the async operations to get actual collections
            var employees = await employeeRepository.GetAllAsync();
            var employmentTypes = await employmentTypeRepository.GetAllAsync();
            var qualifications = await qualificationRepository.GetAllAsync();
            var salaries = await salaryRepository.GetAllAsync();
            var declarations = await declartionRepository.GetAllAsync();

            // LINQ query to join all related tables using EmpId, with left joins to include null values for missing data
            var employeeWithDetails = (from e in employees
                                       join etype in employmentTypes on e.EmpId equals etype.EmpId into etypes
                                       from etype in etypes.DefaultIfEmpty()
                                       join q in qualifications on e.EmpId equals q.EmpId into quals
                                       from qualification in quals.DefaultIfEmpty()
                                       join s in salaries on e.EmpId equals s.EmpId into sal
                                       from salary in sal.DefaultIfEmpty()
                                       join d in declarations on e.EmpId equals d.EmpId into decls
                                       from declaration in decls.DefaultIfEmpty()
                                       where e.EmpId == EmpId  // Filter by specific EmpId if needed
                                       select new GetAllEmployeeViewModel
                                       {
                                           Id = e.Id,
                                           PrefixName = e.PrefixName,
                                           EmpId = e.EmpId,
                                           Title = e.Title,
                                           FirstName = e.FirstName,
                                           MiddleName = e.MiddleName,
                                           LastName = e.LastName,
                                           LoginId = e.LoginId,
                                           Password = e.Password,
                                           Age = e.Age ?? 0,  // Null coalescing to handle nullable Age
                                           RoleId = e.RoleId,
                                           DateOfBirth = e.DateOfBirth,
                                           Gender = e.Gender,
                                           MaritalStatus = e.MaritalStatus,
                                           AadharNumber = e.AadharNumber,
                                           EmailAddress = e.EmailAddress,
                                           HomePhone = e.HomePhone,
                                           CellPhone = e.CellPhone,
                                           OfficePhone = e.OfficePhone,
                                           TeamId = e.TeamId,
                                           MarriageAnniversary = e.MarriageAnniversary,
                                           Street = e.Street,
                                           City = e.City,
                                           Country = e.Country,
                                           ZipCode = e.ZipCode,
                                           Nationality = e.Nationality,
                                           CreatedAt = e.CreatedAt,
                                           UpdatedAt = e.UpdatedAt,

                                           SalaryDetails = salary == null ? new List<SalaryViewModel>() : new List<SalaryViewModel>
                                {
                                    new SalaryViewModel
                                    {
                                        Id = salary.Id,
                                        Ctc = salary.Ctc,
                                        StartYear = salary.StartYear,
                                        EndYear = salary.Endyear,
                                        Description = salary.Description
                                    }
                                },
                               Qualifications = qualification == null ? new List<QualificationViewModel>() : new List<QualificationViewModel>
                                {
                                    new QualificationViewModel
                                    {
                                        Id = qualification.Id,
                                        CourseName = qualification.CourseName,
                                        InstitutionName = qualification.InstitutionName,
                                        PassedDate = qualification.PassedDate,
                                        Details = qualification.Details
                                    }
                                },

                          EmploymentDetails = etype == null ? new List<EmploymentTypeDetailViewModel>() : new List<EmploymentTypeDetailViewModel>
                                {
                                    new EmploymentTypeDetailViewModel
                                    {
                                        Id = etype.Id,
                                        EmpType = etype.EmpType,
                                        AppointmentDate = etype.AppointmentDate,
                                        StartDate = etype.StartDate,
                                        EndDate = etype.EndDate,
                                        TotalService = etype.TotalService,
                                        AppointedService = etype.AppointedService
                                    }
                                },

                          Declarations = declaration == null ? new List<DeclarationViewModel>() : new List<DeclarationViewModel>
                                {
                                    new DeclarationViewModel
                                    {
                                        Id = declaration.Id,
                                        HrRepresentativeName = declaration.HrRepresentativeName,
                                        HrRepresentativeDesignation = declaration.HrRepresentativeDesignation,
                                        HrContactInfo = declaration.HrContactInfo,
                                        Date = declaration.Date,
                                        Signature = declaration.Signature,
                                        VerificationCrossCheck = declaration.VerificationCrossCheck,
                                        VerificationMandatory = declaration.VerificationMandatory
                                    }
                                }


                                       }).ToList();

            return employeeWithDetails;
        }


    }
}
