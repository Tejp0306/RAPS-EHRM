using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
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

        public async Task<Result> SavePersonalInfoAsync(GetAllEmployeeViewModel model, int createdById, String filepath)
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
                    Message = "Employee created successfully.",
                    Data = newEmployeeDetails.EmpId
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

        #region Employee Declaration

        public async Task<Result> CreateDeclarationAsync(EmployeeDeclarationViewModel model)
        {
            try
            {
                var newDeclaration = new EmployeesDeclaration
                {
                    EmployeeName = model.EmployeeName,
                    EmpId = model.EmpId,
                    Designation = model.Designation,
                    BandLevel = model.BandLevel,
                    DateOfJoining = model.DateOfJoining,
                    ProbationStatus = model.ProbationStatus,
                    ProbationDate = model.ProbationDate,
                    Location = model.Location,
                    Project = model.Project,
                    BloodGroup = model.BloodGroup,
                    DateOfBirth = Convert.ToDateTime(model.DateOfBirth),
                    Age = model.Age,
                    Gender = model.Gender,
                    SpouseFatherMotherName = model.SpouseFatherMotherName,
                    RelationWithSpouse = model.RelationWithSpouse,
                    SpouseFatherDateOfBirth = model.SpouseFatherDateOfBirth,
                    MaritalStatus = model.MaritalStatus,
                    OfficialContact = model.OfficialContact,
                    PersonalContact = model.PersonalContact,
                    OfficialEmail = model.OfficialEmail,
                    PersonalEmail = model.PersonalEmail,
                    TenureInRaps = model.TenureInRAPS,
                    YearsInRaps = model.YearsInRAPS,
                    PriorWorkExperience = model.PriorWorkExperience,
                    TotalWorkExperience = model.TotalWorkExperience,
                    FirstOrganisation = model.FirstOrganisation,
                    FirstOrganisationExperience = model.FirstOrganisationExperience,
                    SecondOrganisation = model.SecondOrganisation,
                    SecondOrganisationExperience = model.SecondOrganisationExperience,
                    ThirdOrganisation = model.ThirdOrganisation,
                    ThirdOrganisationExperience = model.ThirdOrganisationExperience,
                    FourthOrganisation = model.FourthOrganisation,
                    FourthOrganisationExperience = model.FourthOrganisationExperience,
                    Dependent1Name = model.Dependent1Name,
                    Dependent1Relationship = model.Dependent1Relationship,
                    Dependent1Dob = model.Dependent1Dob,
                    EmergencyName1 = model.EmergencyName1,
                    EmergencyContact1 = model.EmergencyContact1,
                    EmergencyRelationship1 = model.EmergencyRelationship1,
                    EmergencyName2 = model.EmergencyName2,
                    EmergencyContact2 = model.EmergencyContact2,
                    EmergencyRelationship2 = model.EmergencyRelationship2,
                    XthInstitution = model.XthInstitution,
                    XthPassingYear = model.XthPassingYear,
                    XiithInstitution = model.XiithInstitution,
                    XiithPassingYear = model.XiithPassingYear,
                    BachelorInstitution = model.BachelorInstitution,
                    BachelorCompleteYear = model.BachelorCompleteYear,
                    BachelorDegrees = model.BachelorDegrees,
                    MasterInstitution = model.MasterInstitution,
                    MasterCompleteYear = model.MasterCompleteYear,
                    FatherHusbandName = model.FatherHusbandName,
                    UanNo = model.UanNo,
                    AdharNo = model.AdharNo,
                    PanCardNo = model.PanCardNo,
                    BankName = model.BankName,
                    AccountNumber = model.AccountNumber,
                    IfscCode = model.IfscCode,
                    PermanentAddress = model.PermanentAddress,
                    PostalAddress = model.PostalAddress,
                    Ctc = model.Ctc,
                    FilingPerson = model.FilingPerson,
                    FilingRecheck = model.FilingRecheck,
                    ResignationDate = model.ResignationDate,
                    ExitDate = model.ExitDate,
                    ReasonForLeaving = model.ReasonForLeaving
                };

                var empDeclarationRepository = _UnitOfWork.GetRepository<EmployeesDeclaration>();
                await empDeclarationRepository.AddAsync(newDeclaration);
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Data Saved successfully."
        public async Task<Result> GetManagerAsync()
        {
            try
            {
                // Assuming _UnitOfWork is properly injected
                var employeeRepository = _UnitOfWork.GetRepository<EmployeeDetail>();

                // Await GetAllAsync to get the list of employees
                var employees = await employeeRepository.GetAllAsync();

                // Filter the employee with EmpId == 4 and select necessary fields
                var employeeWithDetails = employees
                    .Where(e => e.RoleId == 4)  // Filtering in-memory after awaiting the result
                    .Select(e => new GetAllEmployeeViewModel
                    {
                        EmpId = e.EmpId,
                        FirstName = e.FirstName, 
                        LastName=e.LastName
                    })
                    .ToList();  // Get the first matching employee

                if (employeeWithDetails == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Employee not found."
                    };
                }

                // Return the result with the employee data
                return new Result
                {
                    Success = true,
                    Message = "Employee found.",
                    Data = employeeWithDetails
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error Saving Data: {ex.Message}"
                };
            }
        }

        public async Task<List<EmployeesDeclaration>> GetEmployeeDetailsByEmpIdDOB(int EmpId, DateTime DOB)
        {
            {
                var employeeProfileRepository = _UnitOfWork.GetRepository<EmployeesDeclaration>();  // Using generic repository
                var employee = await employeeProfileRepository.GetByDeclarationEmpIdDOB(EmpId, DOB);  // Fetch employee based on empid and DOB
                                                                                           //Viewbag.employee=employee;
                return employee;
            }
        }

        public async Task<List<EmployeeDeclarationViewModel>> GetAllEmployeeProfileDetails(int EmpId)
        {
            var employeeRepository = _UnitOfWork.GetRepository<EmployeesDeclaration>();

            // Await the async operations to get actual collections
            var employees = await employeeRepository.GetAllAsync();

            // LINQ query to join all related tables using EmpId, with left joins to include null values for missing data
            var employeeWithDetails = (from e in employees
                                       where e.EmpId == EmpId  // Filter by specific EmpId if needed
                                       select new EmployeeDeclarationViewModel
                                       {
                                           Id = e.Id,
                                           EmployeeName = e.EmployeeName,
                                           EmpId = (int)e.EmpId,
                                           Designation = e.Designation,
                                           BandLevel = e.BandLevel,
                                           DateOfJoining = e.DateOfJoining,
                                           ProbationStatus = e.ProbationStatus,
                                           ProbationDate = e.ProbationDate,
                                           Location = e.Location,
                                           Project = e.Project,
                                           BloodGroup = e.BloodGroup,
                                           DateOfBirth = Convert.ToString(e.DateOfBirth),
                                           Age = (int)e.Age,
                                           Gender = e.Gender,
                                           SpouseFatherMotherName = e.SpouseFatherMotherName,
                                           RelationWithSpouse = e.RelationWithSpouse,
                                           SpouseFatherDateOfBirth = e.SpouseFatherDateOfBirth,
                                           MaritalStatus = e.MaritalStatus,
                                           OfficialContact = e.OfficialContact,
                                           PersonalContact = e.PersonalContact,
                                           OfficialEmail = e.OfficialEmail,
                                           PersonalEmail = e.PersonalEmail,
                                           TenureInRAPS = (int)e.TenureInRaps,
                                           YearsInRAPS = (int)e.YearsInRaps,
                                           PriorWorkExperience = (int)e.PriorWorkExperience,
                                           TotalWorkExperience = (int)e.TotalWorkExperience,
                                           FirstOrganisation = e.FirstOrganisation,
                                           FirstOrganisationExperience = (int)e.FirstOrganisationExperience,
                                           SecondOrganisation = e.SecondOrganisation,
                                           SecondOrganisationExperience = (int)e.SecondOrganisationExperience,
                                           ThirdOrganisation = e.ThirdOrganisation,
                                           ThirdOrganisationExperience = (int)e.ThirdOrganisationExperience,
                                           FourthOrganisation = e.FourthOrganisation,
                                           FourthOrganisationExperience = (int)e.FourthOrganisationExperience,
                                           Dependent1Name = e.Dependent1Name,
                                           Dependent1Relationship = e.Dependent1Relationship,
                                           Dependent1Dob = e.Dependent1Dob,
                                           EmergencyName1 = e.EmergencyName1,
                                           EmergencyContact1 = e.EmergencyContact1,
                                           EmergencyRelationship1 = e.EmergencyRelationship1,
                                           EmergencyName2 = e.EmergencyName2,
                                           EmergencyContact2 = e.EmergencyContact2,
                                           EmergencyRelationship2 = e.EmergencyRelationship2,
                                           XthInstitution = e.XthInstitution,
                                           XthPassingYear = e.XthPassingYear,
                                           XiithInstitution = e.XiithInstitution,
                                           XiithPassingYear = e.XiithPassingYear,
                                           BachelorInstitution = e.BachelorInstitution,
                                           BachelorCompleteYear = e.BachelorCompleteYear,
                                           BachelorDegrees = e.BachelorDegrees,
                                           MasterInstitution = e.MasterInstitution,
                                           MasterCompleteYear = e.MasterCompleteYear,
                                           FatherHusbandName = e.FatherHusbandName,
                                           UanNo = e.UanNo,
                                           AdharNo = e.AdharNo,
                                           PanCardNo = e.PanCardNo,
                                           BankName = e.BankName,
                                           AccountNumber = e.AccountNumber,
                                           IfscCode = e.IfscCode,
                                           PermanentAddress = e.PermanentAddress,
                                           PostalAddress = e.PostalAddress,
                                           Ctc = e.Ctc,
                                           FilingPerson = e.FilingPerson,
                                           FilingRecheck = e.FilingRecheck,
                                           ResignationDate = e.ResignationDate,
                                           ExitDate = e.ExitDate,
                                           ReasonForLeaving = e.ReasonForLeaving


                                       }).ToList();

            return employeeWithDetails;
        }

        #endregion

                // Log exception or handle it appropriately
                throw new Exception("An error occurred while retrieving the manager details.", ex);
            }
        }


    }
}
