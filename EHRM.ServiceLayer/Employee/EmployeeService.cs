using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Enumerations;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Master;
using Microsoft.EntityFrameworkCore;
using System.Transactions;


namespace EHRM.ServiceLayer.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EhrmContext _context;

        public EmployeeService(IUnitOfWork unitOfWork, EhrmContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
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
                    EmpId = (int)model.EmpId,
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
                    Active = false,
                    CreatedById = createdById
                };
                var employeedetailRepository = _unitOfWork.GetRepository<EmployeeDetail>();
                await employeedetailRepository.AddAsync(newEmployeeDetails);
                await _unitOfWork.SaveAsync();

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

            var roleRepository = _unitOfWork.GetRepository<Role>();
            var role = await roleRepository.GetAllAsync();
            return new Result { Success = true, Data = role };

        }
        public async Task<Result> GetTeamAsync()
        {
            try
            {
                var teamsRepository = _unitOfWork.GetRepository<Team>();
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
                var employeeRepository = _unitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository
                var employee = await employeeRepository.GetAllAsync();  // Fetch all roles
                return new Result { Success = true, Data = employee };
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get Employee Data by EmpID

        public async Task<List<GetAllEmployeeViewModel>> GetAllEmployeeRecordDetails(int EmpId)
            {
            var employeeRepository = _unitOfWork.GetRepository<EmployeeDetail>();
            var employmentTypeRepository = _unitOfWork.GetRepository<EmployementTypeDetail>();
            var qualificationRepository = _unitOfWork.GetRepository<Qualification>();
            var salaryRepository = _unitOfWork.GetRepository<Salary>();
            var declartionRepository = _unitOfWork.GetRepository<Declaration>();

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
                                           RoleName = GetRoleNameById(e.RoleId),
                                           DateOfBirth = e.DateOfBirth,
                                           Gender = e.Gender,
                                           MaritalStatus = e.MaritalStatus,
                                           AadharNumber = e.AadharNumber,
                                           EmailAddress = e.EmailAddress,
                                           HomePhone = e.HomePhone,
                                           CellPhone = e.CellPhone,
                                           OfficePhone = e.OfficePhone,
                                           TeamName = GetTeamNameById(e.TeamId),
                                           TeamId = e.TeamId,
                                           MarriageAnniversary = e.MarriageAnniversary,
                                           Street = e.Street,
                                           City = e.City,
                                           Country = e.Country,
                                           ZipCode = e.ZipCode,
                                           Nationality = e.Nationality,
                                           CreatedAt = e.CreatedAt,
                                           UpdatedAt = e.UpdatedAt,
                                           RoleId=e.RoleId,
                                           FileName =  e.Image,

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
                                               Concentration = qualification.Concentration,
                                               QualificationEarned = qualification.QualificationEarned,
                                               InstitutionName = qualification.InstitutionName,
                                               CountryName = qualification.CountryName,
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
                                               AppointedService = etype.AppointedService,
                                               EmploymentStatusId = etype.EmploymentStatusId,
                                               ManagerName = GetManagerNameByManagerId(etype.ManagerId),
                                               ManagerId = etype.ManagerId, 

                                           },

                                           Declarations = declaration == null ? new DeclarationViewModel() : new DeclarationViewModel
                                           {
                                               HrRepresentativeName = declaration.HrRepresentativeName,
                                               HrRepresentativeDesignation = declaration.HrRepresentativeDesignation,
                                               HrContactInfo = declaration.HrContactInfo,
                                               Date = declaration.Date,
                                               Signature = declaration.Signature,
                                               VerificationCrossCheck = (bool)declaration.VerificationCrossCheck,
                                               VerificationMandatory = (bool)declaration.VerificationMandatory,
                                               EmployeeName = e.FirstName + e.LastName
                                           }



                                       }).ToList();

            return employeeWithDetails;
        }



        //Get Employee Data by EmpId
        public async Task<Result> GetEmployeeDataByEmpIdAsync (int EmpId)
        {
            var employeeRepository = _unitOfWork.GetRepository<EmployeeDetail>();

            // Await the async operations to get actual collections
            var employees = await employeeRepository.GetEmployeeDetailsByIdAsync(EmpId);
          
            return new Result { Success = true, Data = employees };



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

                var empDeclarationRepository = _unitOfWork.GetRepository<EmployeesDeclaration>();
                await empDeclarationRepository.AddAsync(newDeclaration);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Data Saved successfully."
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> GetManagerAsync()
        {
            try
            {
                // Assuming _UnitOfWork is properly injected
                var employeeRepository = _unitOfWork.GetRepository<EmployeeDetail>();

                // Await GetAllAsync to get the list of employees
                var employees = await employeeRepository.GetAllAsync();

                // Filter the employee with EmpId == 4 and select necessary fields
                var employeeWithDetails = employees
                    .Where(e => e.RoleId == 4)  // Filtering in-memory after awaiting the result
                    .Select(e => new GetAllEmployeeViewModel
                    {
                        EmpId = e.EmpId,
                        FirstName = e.FirstName,
                        LastName = e.LastName
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
                var employeeProfileRepository = _unitOfWork.GetRepository<EmployeesDeclaration>();  // Using generic repository
                var employee = await employeeProfileRepository.GetByDeclarationEmpIdDOB(EmpId, DOB);  // Fetch employee based on empid and DOB
                                                                                                      //Viewbag.employee=employee;
                return employee;
            }
        }

        public async Task<List<EmployeeDeclarationViewModel>> GetAllEmployeeProfileDetails(int EmpId)
        {
            var employeeRepository = _unitOfWork.GetRepository<EmployeesDeclaration>();

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


        //Save Employment details
        public async Task<Result> SaveEmploymentInfoAsync(GetAllEmployeeViewModel model, int createdById)
        {
            try
            {
                var newEmploymentDetails = new EmployementTypeDetail

                {
                    EmpId = model.EmploymentDetails.EmpId,
                    EmpType = model.EmploymentDetails.EmpType,
                    AppointmentDate = model.EmploymentDetails.AppointmentDate,
                    StartDate = model.EmploymentDetails.StartDate,
                    EmploymentStatusId = model.EmploymentDetails.EmploymentStatusId,
                    ManagerId = model.EmploymentDetails.ManagerId

                    //EmpTypeName = model.EmpTypeName,



                };
                var employementdetailRepository = _unitOfWork.GetRepository<EmployementTypeDetail>();
                await employementdetailRepository.AddAsync(newEmploymentDetails);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employement data saved successfully.",

                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Save Qualification details
        public async Task<Result> SaveQualificationInfoAsync(GetAllEmployeeViewModel model, int createdById)
        {
            try
            {
                var newQualificationDetails = new Qualification

                {
                    EmpId = model.Qualifications.EmpId,
                    CourseName = model.Qualifications.CourseName,
                    Concentration = model.Qualifications.Concentration,
                    QualificationEarned = model.Qualifications.QualificationEarned,
                    InstitutionName = model.Qualifications.InstitutionName,
                    CountryName = model.Qualifications.CountryName,
                    PassedDate = model.Qualifications.PassedDate,
                    Details= model.Qualifications.Details,
                    Document= model.Qualifications.Document

                };
                var qualificationdetailRepository = _unitOfWork.GetRepository<Qualification>();
                await qualificationdetailRepository.AddAsync(newQualificationDetails);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employement data saved successfully.",

                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Save Salary details

        public async Task<Result> SaveSalaryInfoAsync(GetAllEmployeeViewModel model, int createdById)
        {
            try
            {
                var newSalaryDetails = new Salary

                {
                   EmpId = model.SalaryDetails.EmpId,
                   Ctc= model.SalaryDetails.Ctc,
                   StartYear = model.SalaryDetails.StartYear,
                   Endyear = model.SalaryDetails.EndYear,
                   Description = model.SalaryDetails.Description,

                };
                var salarydetailRepository = _unitOfWork.GetRepository<Salary>();
                await salarydetailRepository.AddAsync(newSalaryDetails);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Salary data saved successfully.",

                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Save Declaration details

        public async Task<Result> SaveDecalarationInfoAsync(GetAllEmployeeViewModel model, int createdById)
        {
            try
            {
                var newDecalarationDetails = new Declaration

                {
                    EmpId = model.Declarations.EmpId,
                    HrRepresentativeName = model.Declarations.HrRepresentativeName,
                    HrRepresentativeDesignation = model.Declarations.HrRepresentativeDesignation,
                    HrContactInfo = model.Declarations.HrContactInfo,
                    Date = model.Declarations.Date,
                    Signature = model.Declarations.Signature,
                    VerificationCrossCheck = model.Declarations.VerificationCrossCheck,
                    VerificationMandatory = model.Declarations.VerificationMandatory,
                    
                    

                };


                var declarationdetailRepository = _unitOfWork.GetRepository<Declaration>();
                await declarationdetailRepository.AddAsync(newDecalarationDetails);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Declaration data saved successfully.",

                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        //Update Personal Info
        public async Task<Result> UpdatePersonalInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                var personalinfoRepository = _unitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository
                var existingEmployee = await personalinfoRepository.GetEmployeeDetailsByIdAsync(id);  // Fetch existing role by ID

                if (existingEmployee == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                // Update role properties
                existingEmployee[0].EmpId = (int)model.EmpId;
                existingEmployee[0].PrefixName = model.PrefixName;
                existingEmployee[0].Title = model.Title;
                existingEmployee[0].FirstName = model.FirstName;
                existingEmployee[0].MiddleName = model.MiddleName;
                existingEmployee[0].LastName = model.LastName;
                existingEmployee[0].Gender = model.Gender;
                existingEmployee[0].RoleId = model.RoleId;
                existingEmployee[0].DateOfBirth = model.DateOfBirth;
                existingEmployee[0].MaritalStatus = model.MaritalStatus;
                existingEmployee[0].AadharNumber = model.AadharNumber;
                existingEmployee[0].EmailAddress = model.EmailAddress;
                existingEmployee[0].HomePhone = model.HomePhone;
                existingEmployee[0].CellPhone = model.CellPhone;
                existingEmployee[0].OfficePhone = model.OfficePhone;
                existingEmployee[0].TeamId = model.TeamId;
                existingEmployee[0].MarriageAnniversary = model.MarriageAnniversary;
                existingEmployee[0].Street = model.Street;
                existingEmployee[0].City = model.Street;
                existingEmployee[0].Country = model.Country;
                existingEmployee[0].ZipCode = model.ZipCode;
                existingEmployee[0].Nationality = model.Nationality;
                //existingEmployee.Image = model.ProfileImg;


                await personalinfoRepository.UpdateAsync(existingEmployee[0]);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Role updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }


        // Update Employment Info

        public async Task<Result> UpdateEmploymentInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                var employmentinfoRepository = _unitOfWork.GetRepository<EmployementTypeDetail>();  // Using generic repository
                var existingEmploymentdetails = await employmentinfoRepository.GetEmployementTypeDetailsByIdAsync(id);  // Fetch existing role by ID

                if (existingEmploymentdetails == null || !existingEmploymentdetails.Any())
                {
                    var newEmploymentDetails = new EmployementTypeDetail

                    {
                        EmpId = model.EmploymentDetails.EmpId,
                        EmpType = model.EmploymentDetails.EmpType,
                        AppointmentDate = model.EmploymentDetails.AppointmentDate,
                        StartDate = model.EmploymentDetails.StartDate,
                        EmploymentStatusId = model.EmploymentDetails.EmploymentStatusId,
                        ManagerId = model.EmploymentDetails.ManagerId

                    };
                    var employementdetailRepository = _unitOfWork.GetRepository<EmployementTypeDetail>();
                    await employementdetailRepository.AddAsync(newEmploymentDetails);
                    await _unitOfWork.SaveAsync();

                    return new Result
                    {
                        Success = true,
                        Message = "Employement data saved successfully.",

                    };

                }

                // Update role properties
                existingEmploymentdetails[0].EmpId = (int)model.EmploymentDetails.EmpId;
                existingEmploymentdetails[0].EmpType = model.EmploymentDetails.EmpType;
                existingEmploymentdetails[0].AppointmentDate = model.EmploymentDetails.AppointmentDate;
                existingEmploymentdetails[0].StartDate = model.EmploymentDetails.StartDate;
                existingEmploymentdetails[0].EmploymentStatusId = model.EmploymentDetails.EmploymentStatusId;
                existingEmploymentdetails[0].ManagerId = model.EmploymentDetails.ManagerId;



                await employmentinfoRepository.UpdateAsync(existingEmploymentdetails[0]);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Employment details  updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }




        //Update Qualification Info
        public async Task<Result> UpdateQualificationInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                var qualificationinfoRepository = _unitOfWork.GetRepository<Qualification>();  // Using generic repository
                var existingEmployeeQualification = await qualificationinfoRepository.GetQualificationDetailsByIdAsync(id);  // Fetch existing role by ID

                if (existingEmployeeQualification == null || !existingEmployeeQualification.Any())
                {
                    var newQualificationDetails = new Qualification

                    {
                        EmpId = model.Qualifications.EmpId,
                        CourseName = model.Qualifications.CourseName,
                        Concentration = model.Qualifications.Concentration,
                        QualificationEarned = model.Qualifications.QualificationEarned,
                        InstitutionName = model.Qualifications.InstitutionName,
                        CountryName = model.Qualifications.CountryName,
                        PassedDate = model.Qualifications.PassedDate,
                        Details = model.Qualifications.Details,
                        Document = model.Qualifications.Document

                    };
                    var qualificationdetailRepository = _unitOfWork.GetRepository<Qualification>();
                    await qualificationdetailRepository.AddAsync(newQualificationDetails);
                    await _unitOfWork.SaveAsync();

                    return new Result
                    {
                        Success = true,
                        Message = "Employement data saved successfully.",

                    };
                }

                // Update role properties
                existingEmployeeQualification[0].EmpId = (int)model.Qualifications.EmpId;
                existingEmployeeQualification[0].CourseName = model.Qualifications.CourseName;
                existingEmployeeQualification[0].Concentration = model.Qualifications.Concentration;
                existingEmployeeQualification[0].QualificationEarned = model.Qualifications.QualificationEarned;
                existingEmployeeQualification[0].InstitutionName = model.Qualifications.InstitutionName;
                existingEmployeeQualification[0].CountryName = model.Qualifications.CountryName;
                existingEmployeeQualification[0].PassedDate = model.Qualifications.PassedDate;
                existingEmployeeQualification[0].Details = model.Qualifications.Details;
                existingEmployeeQualification[0].Document = model.Qualifications.Document;



                await qualificationinfoRepository.UpdateAsync(existingEmployeeQualification[0]);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Qualification updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }

        //Update Salary Info
        public async Task<Result> UpdateSalaryInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                var salaryinfoRepository = _unitOfWork.GetRepository<Salary>();  // Using generic repository
                var existingEmployeeSalary = await salaryinfoRepository.GetSalaryDetailsByIdAsync(id);  // Fetch existing role by ID

                if (existingEmployeeSalary == null || !existingEmployeeSalary.Any())
                {
                    var newSalaryDetails = new Salary

                    {
                        EmpId = model.SalaryDetails.EmpId,
                        Ctc = model.SalaryDetails.Ctc,
                        StartYear = model.SalaryDetails.StartYear,
                        Endyear = model.SalaryDetails.EndYear,
                        Description = model.SalaryDetails.Description,

                    };
                    var salarydetailRepository = _unitOfWork.GetRepository<Salary>();
                    await salarydetailRepository.AddAsync(newSalaryDetails);
                    await _unitOfWork.SaveAsync();

                    return new Result
                    {
                        Success = true,
                        Message = "Salary data saved successfully.",

                    };
                }

                // Update role properties
                existingEmployeeSalary[0].EmpId = (int)model.SalaryDetails.EmpId;
                existingEmployeeSalary[0].Ctc = model.SalaryDetails.Ctc;
                existingEmployeeSalary[0].StartYear = model.SalaryDetails.StartYear;
                existingEmployeeSalary[0].Endyear = model.SalaryDetails.EndYear;
                existingEmployeeSalary[0].Description = model.SalaryDetails.Description;


                await salaryinfoRepository.UpdateAsync(existingEmployeeSalary[0]);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Qualification updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating role: {ex.Message}"
                };
            }
        }


        //Update Declaration Info

        public async Task<Result> UpdateDeclarationInfoAsync(int id, string updatedBy, GetAllEmployeeViewModel model)
        {
            try
            {
                var declarationinfoRepository = _unitOfWork.GetRepository<Declaration>();  // Using generic repository
                var existingEmployeeDeclaration = await declarationinfoRepository.GetDeclarationDetailsByIdAsync(id);  // Fetch existing role by ID

                if (existingEmployeeDeclaration == null || !existingEmployeeDeclaration.Any())
                {
                    var newDecalarationDetails = new Declaration

                    {
                        EmpId = model.Declarations.EmpId,
                        HrRepresentativeName = model.Declarations.HrRepresentativeName,
                        HrRepresentativeDesignation = model.Declarations.HrRepresentativeDesignation,
                        HrContactInfo = model.Declarations.HrContactInfo,
                        Date = model.Declarations.Date,
                        Signature = model.Declarations.Signature,
                        VerificationCrossCheck = model.Declarations.VerificationCrossCheck,
                        VerificationMandatory = model.Declarations.VerificationMandatory


                    };
                    var declarationdetailRepository = _unitOfWork.GetRepository<Declaration>();
                    await declarationdetailRepository.AddAsync(newDecalarationDetails);
                    var personalRepository = _unitOfWork.GetRepository<EmployeeDetail>();  // Using generic repository
                    var Employee = await personalRepository.GetEmployeeDetailsByIdAsync(id);
                    Employee[0].IsProfileCompleted = true;
                    await _unitOfWork.SaveAsync();

                    return new Result
                    {
                        Success = true,
                        Message = "Declaration data saved successfully.",

                    };
                }

                // Update role properties
                existingEmployeeDeclaration[0].EmpId = (int)model.Declarations.EmpId;
                existingEmployeeDeclaration[0].HrRepresentativeName = model.Declarations.HrRepresentativeName;
                existingEmployeeDeclaration[0].HrRepresentativeDesignation = model.Declarations.HrRepresentativeDesignation;
                existingEmployeeDeclaration[0].HrContactInfo = model.Declarations.HrContactInfo;
                existingEmployeeDeclaration[0].Date = model.Declarations.Date;
                existingEmployeeDeclaration[0].Signature = model.Declarations.Signature;
                existingEmployeeDeclaration[0].VerificationCrossCheck = model.Declarations.VerificationCrossCheck;
                existingEmployeeDeclaration[0].VerificationMandatory = model.Declarations.VerificationMandatory;

                await declarationinfoRepository.UpdateAsync(existingEmployeeDeclaration[0]);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Declaration updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Declaration: {ex.Message}"
                };
            }
        }



        public string? GetRoleNameById(int roleId)
        {
            var roleName = _context.Roles
                .Where(r => r.RoleId == roleId)
                .Select(r => r.RoleName)
                .FirstOrDefault();
            return roleName;
        }

        public string? GetTeamNameById(int teamid)
        {
            var teamName = _context.Teams
                .Where(t => t.TeamId == teamid)
                .Select(t => t.Name)
                .FirstOrDefault();
            return teamName;

        }

        public string GetManagerNameByManagerId(int?managerid)
        {
            var ManagerName = _context.EmployeeDetails
                .Where(t => t.EmpId == managerid)
                .Select(t => t.FirstName + " " + t.LastName)
                .FirstOrDefault();
            return ManagerName;

        }


        public bool CheckUserInDbByEmpId(int? EmpId)
        {
            // Use Any() to check if a record exists with the specified EmpId
            return _context.EmployeeDetails.Any(c => c.EmpId == EmpId);
        }

        public bool CheckUserInEmpCredDbByEmpId(int? EmpId)
        {
            // Use Any() to check if a record exists with the specified EmpId
            return _context.EmployeesCreds.Any(c => c.EmpId == EmpId);
        }



        #endregion

    }

}
