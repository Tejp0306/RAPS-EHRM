using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.MasterEmployee;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Globalization;
using EHRM.ServiceLayer.Models;


namespace EHRM.ServiceLayer.PostJoining
{
    public class PostJoiningService : IPostJoiningService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly string _connectionString;

        public PostJoiningService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _connectionString = configuration.GetConnectionString("EHRMConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is not configured properly.");
            }
            _UnitOfWork = unitOfWork;
        }

        // Save Master Personal Info of MasterSheet
        public async Task<bool> SaveMasterSheetAsync(EmployeeFormViewModel model)
        {
            try
            {
                // Save MasterSheet data
                var masterSheet = new EmployeeMaster
                {
                    EmpId = model.MasterEmployee.EmpId,
                    FirstName = model.MasterEmployee.FirstName,
                    MiddleName = model.MasterEmployee.MiddleName,
                    LastName = model.MasterEmployee.LastName,
                    Gender = model.MasterEmployee.Gender,
                    //DateOfBirth = model.MasterEmployee.DateOfBirth,
                    Age = model.MasterEmployee.Age,
                    MaritalStatus = model.MasterEmployee.MaritalStatus,
                    //DateOfJoining = model.MasterEmployee.DateOfJoining,
                    BandLevel = model.MasterEmployee.BandLevel,
                    Designation = model.MasterEmployee.Designation,
                    Location = model.MasterEmployee.Location,
                    Department = model.MasterEmployee.Department, // Updated to Department
                    FunctionProject = model.MasterEmployee.FunctionProject, // Added mapping for FunctionProject
                    ProbationConfirmationStatus = model.MasterEmployee.ProbationConfirmationStatus,
                    //ProbationConfirmationDate = model.MasterEmployee.ProbationConfirmationDate,
                    TenureInRaps = model.MasterEmployee.TenureInRAPS,
                    YearsInRaps = model.MasterEmployee.YearsInRAPS,
                    TotalWorkExperience = model.MasterEmployee.TotalWorkExperience,
                    Uannumber = model.MasterEmployee.UANNumber,
                    AadharNumber = model.MasterEmployee.AadharNumber,
                    PancardNumber = model.MasterEmployee.PANCardNumber,
                    CtcperAnnumOnDoj = model.MasterEmployee.CTCPerAnnumOnDOJ,
                    FilingPerson = model.MasterEmployee.FilingPerson,
                    FilingRecheck = model.MasterEmployee.FilingRecheck,
                    Remark = model.MasterEmployee.Remark
                };


                var masteremployeeRepository = _UnitOfWork.GetRepository<EmployeeMaster>();
                await masteremployeeRepository.AddAsync(masterSheet);
                await _UnitOfWork.SaveAsync();



                await _UnitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }

        // Save Master Contact Info of MasterSheet
        public async Task<bool> SaveMasterContactAsync(EmployeeFormViewModel model)
        {
            try
            {
                // Save MasterSheet data
                var contact = new ContactDetail
                {
                    EmpId = model.MasterContactDetails.EmpId,
                    OfficialContactNo = model.MasterContactDetails.OfficialContactNo,
                    PersonalContactNo = model.MasterContactDetails.PersonalContactNo,
                    OfficialEmailId = model.MasterContactDetails.OfficialEmailId,
                    PersonalEmailId = model.MasterContactDetails.PersonalEmailId,
                    EmergencyContactName = model.MasterContactDetails.EmergencyContactName,
                    EmergencyContactNumber = model.MasterContactDetails.EmergencyContactNumber,
                    EmergencyRelationship = model.MasterContactDetails.EmergencyRelationship
                };



                var masterContactRepository = _UnitOfWork.GetRepository<ContactDetail>();
                await masterContactRepository.AddAsync(contact);
                await _UnitOfWork.SaveAsync();



                await _UnitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }

        // Save Master Address Info of MasterSheet
        public async Task<bool> SaveMasterAddressAsync(EmployeeFormViewModel model)
        {
            try
            {
                // Save MasterSheet data
                var address = new AddressDetail
                {
                    EmpId = model.MasterAddress.EmpId,
                    PermanentAddress = model.MasterAddress.PermanentAddress,
                    PostalAddress = model.MasterAddress.PostalAddress
                };



                var masterContactRepository = _UnitOfWork.GetRepository<AddressDetail>();
                await masterContactRepository.AddAsync(address);
                await _UnitOfWork.SaveAsync();



                await _UnitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }

        // Save Master Education Info of MasterSheet
        public async Task<bool> SaveMasterEducationAsync(EmployeeFormViewModel model)
        {
            try
            {
                // Save MasterSheet data
                var education = new EducationalDetail
                {
                    EmpId = model.MasterEducation.EmpId,

                    // Xth Details
                    XthInstitution = model.MasterEducation.XthInstitution,
                    XthPassingYear = model.MasterEducation.XthPassingYear,

                    // XIIth Details
                    XiithInstitution = model.MasterEducation.XIIthInstitution,
                    XiithPassingYear = model.MasterEducation.XIIthPassingYear,

                    // Bachelor's Details
                    BachelorInstitution = model.MasterEducation.BachelorInstitution,
                    BachelorCompletionYear = model.MasterEducation.BachelorCompletionYear,
                    BachelorDegree = model.MasterEducation.BachelorDegree,

                    // Master's Details
                    MasterInstitution = model.MasterEducation.MasterInstitution,
                    MasterCompletionYear = model.MasterEducation.MasterCompletionYear,
                    MasterDegree = model.MasterEducation.MasterDegree,

                    // Post-Doctorate Details
                    PostDoctorateInstitution = model.MasterEducation.PostDoctorateInstitution,
                    PostDoctorateDegree = model.MasterEducation.PostDoctorateDegree,
                    PostDoctorateCompletionYear = model.MasterEducation.PostDoctorateCompletionYear,

                    // Professional Courses
                    ProfessionalCoursesInstitution = model.MasterEducation.ProfessionalCoursesInstitution,
                    ProfessionalCoursesDegree = model.MasterEducation.ProfessionalCoursesDegree,
                    ProfessionalCoursesCompletionYear = model.MasterEducation.ProfessionalCoursesCompletionYear
                };




                var masterContactRepository = _UnitOfWork.GetRepository<EducationalDetail>();
                await masterContactRepository.AddAsync(education);
                await _UnitOfWork.SaveAsync();



                await _UnitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }


        // Save Master Experience Info of MasterSheet

        public async Task<bool> SaveMasterExperienceAsync(EmployeeFormViewModel model)
        {
            try
            {
                if (model.MasterWorkExperience != null && model.MasterWorkExperience.Count > 0)
                {
                    var experienceRepository = _UnitOfWork.GetRepository<WorkExperience>();

                    foreach (var experience in model.MasterWorkExperience)
                    {
                        var newExperience = new WorkExperience
                        {
                            EmpId = experience.EmpId,
                            OrganisationName = experience.OrganisationName,
                            Designation = experience.Designation,
                            YearsOfExperience = experience.YearsOfExperience,
                            FromDate = experience.FromDate,
                            ToDate = experience.ToDate,
                            ReasonForLeaving = experience.ReasonForLeaving
                        };

                        await experienceRepository.AddAsync(newExperience);
                    }

                    await _UnitOfWork.SaveAsync(); // Save all experiences to the DB
                    return true;
                }
                return false; // No experiences to save
            }
            catch (Exception)
            {
                // Log error (optional) or handle accordingly
                return false;
            }
        }


        // Save Master Bank Info of MasterSheet
        public async Task<bool> SaveMasterBankAsync(EmployeeFormViewModel model)
        {
            try
            {
                // Save MasterSheet data
                var Bank = new BankDetail
                {
                    EmpId = model.MasterBankDetails.EmpId,
                    BankName = model.MasterBankDetails.BankName,
                    AccountNumber = model.MasterBankDetails.AccountNumber,
                    Ifsccode = model.MasterBankDetails.IFSCCode
                };


                var masterBankRepository = _UnitOfWork.GetRepository<BankDetail>();
                await masterBankRepository.AddAsync(Bank);
                await _UnitOfWork.SaveAsync();



                await _UnitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }


        // Save Master Emergency Info of MasterSheet
        public async Task<bool> SaveMasterEmergencyAsync(EmployeeFormViewModel model)
        {
            try
            {
                //Save MasterSheet data
                var Emergency = new MasterEmergencyContact
                {
                    EmpId = model.MasterEmergencyContactViewModel.EmpId,
                    EmergencyContactNumber = model.MasterEmergencyContactViewModel.EmergencyContactNumber,
                    EmergencyName = model.MasterEmergencyContactViewModel.EmergencyName,
                    Relationship = model.MasterEmergencyContactViewModel.Relationship
                };


                var masterEmergencyRepository = _UnitOfWork.GetRepository<MasterEmergencyContact>();
                await masterEmergencyRepository.AddAsync(Emergency);
                await _UnitOfWork.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }


        // Save Master Reporting Info of MasterSheet
        public async Task<bool> SaveMasterReportingAsync(EmployeeFormViewModel model)
        {
            try
            {
                //Save MasterSheet data
                var Reporting = new ReportingDetail
                {
                    EmpId = model.MasterReportingDetails.EmpId,
                    DirectReporting = model.MasterReportingDetails.DirectReporting,
                    SkipReporting = model.MasterReportingDetails.SkipReporting,
                    DottedReporting = model.MasterReportingDetails.DottedReporting

                };


                var masterReportingRepository = _UnitOfWork.GetRepository<ReportingDetail>();
                await masterReportingRepository.AddAsync(Reporting);
                await _UnitOfWork.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }


        // Save Master Family Info of MasterSheet
        public async Task<bool> SaveMasterFamilyAsync(EmployeeFormViewModel model)
        {
            try
            {
                //Save MasterSheet data
                var Family = new FamilyDetail
                {
                    EmpId = model.MasterFamilyDetails.EmpId,
                    Name = model.MasterFamilyDetails.Name,
                    RelationWithEmployee = model.MasterFamilyDetails.RelationWithEmployee,
                    //DateOfBirth = model.MasterFamilyDetails.DateOfBirth

                };


                var masterFamilyRepository = _UnitOfWork.GetRepository<FamilyDetail>();
                await masterFamilyRepository.AddAsync(Family);
                await _UnitOfWork.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }


        // Save Master Family Info of MasterSheet
        public async Task<bool> SaveMasterDependentAsync(EmployeeFormViewModel model)
        {
            try
            {
                //Save MasterSheet data
                var Dependent = new DependentDetail
                {
                    EmpId = model.MasterDependentDetails.EmpId,
                    DependentName = model.MasterDependentDetails.DependentName,
                    Relationship = model.MasterDependentDetails.Relationship,
                    //DateOfBirth = model.MasterDependentDetails.DateOfBirth


                };


                var masterDependentRepository = _UnitOfWork.GetRepository<DependentDetail>();
                await masterDependentRepository.AddAsync(Dependent);
                await _UnitOfWork.SaveAsync();

                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }

        public async Task<bool> SaveBGVFormAsync(BGVViewModel model)
        {
            try
            {
                // Save MasterSheet data
                var BGVForm = new Bgvform
                {
                    // ✅ Employee Details
                    EmpId = model.EmpId,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName,

                    // ✅ Father's Details
                    FatherFirstName = model.FatherFirstName,
                    FatherMiddleName = model.FatherMiddleName,
                    FatherLastName = model.FatherLastName,

                    // ✅ Personal Details
                    DateOfBirth = model.DateOfBirth,
                    PlaceOfBirth = model.PlaceOfBirth,
                    Gender = model.Gender,
                    MaritalStatus = model.MaritalStatus,
                    Nationality = model.Nationality,

                    // ✅ Contact Details
                    MobileNumber = model.MobileNumber,
                    AlternateNumber = model.AlternateNumber,
                    Email = model.Email,

                    // ✅ Education Details
                    CourseName = model.CourseName,
                    ProgramType = model.ProgramType,
                    CollegeName = model.CollegeName,
                    UniversityBoardName = model.UniversityBoardName,
                    PassingYear = model.PassingYear,
                    ProofType = model.ProofType,

                    // ✅ Address Details
                    CompleteAddress = model.CompleteAddress,
                    NearestLandmark = model.NearestLandmark,

                    // ✅ Reference Details
                    ReferenceName = model.ReferenceName,
                    ReferenceContact = model.ReferenceContact,

                    // ✅ Consent
                    ConsentGiven = model.ConsentGiven,


                };



                var masteremployeeRepository = _UnitOfWork.GetRepository<Bgvform>();
                await masteremployeeRepository.AddAsync(BGVForm);
                await _UnitOfWork.SaveAsync();



                await _UnitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                // Log error (if necessary)
                return false;
            }
        }


        //public async Task<bool> SavePreviousEmploymentsAsync(EmploymentViewModel model)
        //{
        //    try
        //    {
        //        // ✅ Save Previous Employment Data
        //        if (model.PreviousEmployments != null && model.PreviousEmployments.Count > 0)
        //        {
        //            var previousEmploymentRepository = _UnitOfWork.GetRepository<PreviousEmployment>();
        //            var bgvformRepository = _UnitOfWork.GetRepository<Bgvform>();


        //            foreach (var experience in model.PreviousEmployments)
        //            {
        //                var previousEmployment = new PreviousEmployment
        //                {
        //                    //EmpId = model.EmpId, // Capture Employee Id
        //                    BgvformId = experience.Id,
        //                    NatureOfEmployment = experience.NatureOfEmployment,
        //                    CurrentDesignation = experience.CurrentDesignation,
        //                    Department = experience.Department,
        //                    OfficialTitle = experience.OfficialTitle,
        //                    PayrollCompanyName = experience.PayrollCompanyName,
        //                    OrganizationAddress = experience.OrganizationAddress,
        //                    //FromDate = experience.FromDate,
        //                    //ToDate = experience.ToDate,
        //                    EmployeeCode = experience.EmployeeCode,
        //                    //CTCPerAnnum = experience.CTCPerAnnum,
        //                    KeyResponsibility = experience.KeyResponsibility,
        //                    EmploymentTenure = experience.EmploymentTenure,
        //                    ReasonForLeaving = experience.ReasonForLeaving,
        //                    ReportingManagerName = experience.ReportingManagerName,
        //                    ReportingManagerDesignation = experience.ReportingManagerDesignation,
        //                    IsReportingManagerStillInCompany = experience.IsReportingManagerStillInCompany,
        //                    CompanyLandline = experience.CompanyLandline,
        //                    PersonalMobileNo = experience.PersonalMobileNo,
        //                    BestTimeToReach = experience.BestTimeToReach,
        //                    CreatedDate = DateTime.Now
        //                };

        //                // ✅ Add Previous Employment to Repository
        //                await previousEmploymentRepository.AddAsync(previousEmployment);
        //            }

        //            // ✅ Save Previous Employment Data
        //            await _UnitOfWork.SaveAsync();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // ❌ Log the exception (if necessary)
        //        // _logger.LogError(ex, "Error while saving Previous Employments.");
        //        return false;
        //    }
        //}

        public async Task<bool> SavePreviousEmploymentsAsync(EmploymentViewModel model)
        {
            try
            {
                if (model.PreviousEmployments != null && model.PreviousEmployments.Count > 0)
                {
                    var previousEmploymentRepository = _UnitOfWork.GetRepository<PreviousEmployment>();

                    // ✅ Extract the BgvformId
                    //int bgvFormId = bgvForm.BgvformId;

                    // ✅ Loop through each PreviousEmployment and assign the BgvformId
                    foreach (var experience in model.PreviousEmployments)
                    {
                        var previousEmployment = new PreviousEmployment
                        {
                            //BgvformId = bgvFormId, // ✅ Correctly set the foreign key reference
                            EmpId = experience.EmpId,
                            NatureOfEmployment = experience.NatureOfEmployment,
                            CurrentDesignation = experience.CurrentDesignation,
                            Department = experience.Department,
                            OfficialTitle = experience.OfficialTitle,
                            PayrollCompanyName = experience.PayrollCompanyName,
                            OrganizationAddress = experience.OrganizationAddress,
                            EmployeeCode = experience.EmployeeCode,
                            KeyResponsibility = experience.KeyResponsibility,
                            EmploymentTenure = experience.EmploymentTenure,
                            ReasonForLeaving = experience.ReasonForLeaving,
                            ReportingManagerName = experience.ReportingManagerName,
                            ReportingManagerDesignation = experience.ReportingManagerDesignation,
                            IsReportingManagerStillInCompany = experience.IsReportingManagerStillInCompany,
                            CompanyLandline = experience.CompanyLandline,
                            PersonalMobileNo = experience.PersonalMobileNo,
                            BestTimeToReach = experience.BestTimeToReach,
                            CreatedDate = DateTime.Now
                        };

                        // ✅ Add each PreviousEmployment to the repository
                        await previousEmploymentRepository.AddAsync(previousEmployment);
                    }

                    // ✅ Save all changes to the database
                    await _UnitOfWork.SaveAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                // ❌ Log the exception if necessary
                //_logger.LogError(ex, "Error while saving Previous Employments.");
                return false;
            }
        }

        // Get All BGV Data
        public async Task<BGVFormDTO> GetEmployeeDetailsAsync(int empId)
        {
            try
            {
                // ✅ Get repositories for BGVForm and PreviousEmployments
                var bgvFormRepository = _UnitOfWork.GetRepository<Bgvform>();
                var previousEmploymentRepository = _UnitOfWork.GetRepository<PreviousEmployment>();

                // ✅ Fetch BGVForm data
                var bgvForm = await bgvFormRepository.GetBGVByEmpIdAsync(empId);


                // ❌ Return null if no BGVForm found
                if (bgvForm == null)
                {
                    return null;
                }

                // ✅ Fetch PreviousEmployments data for the given EmpId
                var previousEmployments = await previousEmploymentRepository.GetPreviousEmploymentByEmpIdAsync(empId);


                // ✅ Map data to BGVFormDTO
                var employeeData = new BGVFormDTO
                {
                    BGVFormId = bgvForm[0].Id,
                    EmpId = bgvForm[0].EmpId,
                    FirstName = bgvForm[0].FirstName,
                    MiddleName = bgvForm[0].MiddleName,
                    LastName = bgvForm[0].LastName,
                    FatherFirstName = bgvForm[0].FatherFirstName,
                    FatherMiddleName = bgvForm[0].FatherMiddleName,
                    FatherLastName = bgvForm[0].FatherLastName,
                    DateOfBirth = bgvForm[0].DateOfBirth,
                    PlaceOfBirth = bgvForm[0].PlaceOfBirth,
                    Gender = bgvForm[0].Gender,
                    MaritalStatus = bgvForm[0].MaritalStatus,
                    Nationality = bgvForm[0].Nationality,
                    MobileNumber = bgvForm[0].MobileNumber,
                    AlternateNumber = bgvForm[0].AlternateNumber,
                    Email = bgvForm[0].Email,

                    // Education Details
                    CourseName = bgvForm[0].CourseName,
                    ProgramType = bgvForm[0].ProgramType,
                    CollegeName = bgvForm[0].CollegeName,
                    UniversityBoardName = bgvForm[0].UniversityBoardName,
                    PassingYear = bgvForm[0].PassingYear,
                    ProofType = bgvForm[0].ProofType,

                    // Address Details
                    CompleteAddress = bgvForm[0].CompleteAddress,
                    NearestLandmark = bgvForm[0].NearestLandmark,

                    // Reference Details
                    ReferenceName = bgvForm[0].ReferenceName,
                    ReferenceContact = bgvForm[0].ReferenceContact,

                    // Consent
                    ConsentGiven = bgvForm[0].ConsentGiven,

                    // ✅ Map PreviousEmployments
                    PreviousEmployments = previousEmployments.Select(pe => new PreviousEmploymentDTO
                    {
                        PreviousEmploymentId = pe.Id,
                        NatureOfEmployment = pe.NatureOfEmployment,
                        CurrentDesignation = pe.CurrentDesignation,
                        Department = pe.Department,
                        OfficialTitle = pe.OfficialTitle,
                        PayrollCompanyName = pe.PayrollCompanyName,
                        OrganizationAddress = pe.OrganizationAddress,
                        FromDate = pe.FromDate,
                        ToDate = pe.ToDate,
                        EmployeeCode = pe.EmployeeCode,
                        CTCPerAnnum = pe.CtcperAnnum,
                        KeyResponsibility = pe.KeyResponsibility,
                        EmploymentTenure = pe.EmploymentTenure,
                        ReasonForLeaving = pe.ReasonForLeaving,
                        ReportingManagerName = pe.ReportingManagerName,
                        ReportingManagerDesignation = pe.ReportingManagerDesignation,
                        IsReportingManagerStillInCompany = pe.IsReportingManagerStillInCompany,
                        CompanyLandline = pe.CompanyLandline,
                        PersonalMobileNo = pe.PersonalMobileNo,
                        BestTimeToReach = pe.BestTimeToReach
                    }).ToList()
                };

                // ✅ Return the mapped data
                return employeeData;
            }
            catch (Exception ex)
            {
                // ❌ Log the exception
                //_logger.LogError(ex, "Error while fetching BGVForm and Previous Employments.");
                return null;
            }
        }

        //Get All MasterSheet Data of Employee
        public EmployeeFormViewModel GetMasterSheetDataAsync(int EmpId)
        {
            DataTable employeeMasterData = new DataTable();
            List<MasterWorkExperience> masterWorkExperiences = new List<MasterWorkExperience>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeeMasterData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpId", EmpId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(employeeMasterData);
                    }
                }
            }

            if (employeeMasterData.Rows.Count > 0 && employeeMasterData.Columns.Contains("WorkExperience"))
            {
                string json = employeeMasterData.Rows[0]["WorkExperience"]?.ToString();

                // 🔹 Debug: Print JSON to check format
                Console.WriteLine("Raw JSON WorkExperience: " + json);

                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    try
                    {
                        // Deserialize JSON into a List of MasterWorkExperience
                        JArray jsonArray = JArray.Parse(json);
                        foreach (JObject item in jsonArray)
                        {
                            MasterWorkExperience experience = new MasterWorkExperience
                            {
                                OrganisationName = item["OrganisationName"]?.ToString(),
                                Designation = item["Designation"]?.ToString(),
                                ReasonForLeaving = item["ReasonForLeaving"]?.ToString(),
                                YearsOfExperience = item["YearsOfExperience"]?.ToObject<decimal>() ?? 0m
                            };

                            // 🔹 Debug: Print Date Values Before Parsing
                            Console.WriteLine($"Raw WorkStartDate: {item["WorkStartDate"]}");
                            Console.WriteLine($"Raw WorkEndDate: {item["WorkEndDate"]}");

                            // Convert WorkStartDate
                            if (DateTime.TryParse(item["WorkStartDate"]?.ToString(), out DateTime parsedFromDate))
                            {
                                experience.FromDate = DateOnly.FromDateTime(parsedFromDate);
                            }

                            // Convert WorkEndDate
                            if (DateTime.TryParse(item["WorkEndDate"]?.ToString(), out DateTime parsedToDate))
                            {
                                // Ensure WorkEndDate is after WorkStartDate

                                experience.ToDate = DateOnly.FromDateTime(parsedToDate);

                            }

                            masterWorkExperiences.Add(experience);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Error parsing WorkExperience JSON: " + ex.Message);
                    }
                }
            }

            // Initialize EmployeeFormViewModel
            EmployeeFormViewModel employeeFormViewModel = new EmployeeFormViewModel
            {
                MasterEmployee = employeeMasterData.ConvertToSingle<MasterEmployee>(),
                MasterContactDetails = employeeMasterData.ConvertToSingle<MasterContactDetails>(),
                MasterAddress = employeeMasterData.ConvertToSingle<MasterAddress>(),
                MasterEducation = employeeMasterData.ConvertToSingle<MasterEducation>(),
                MasterBankDetails = employeeMasterData.ConvertToSingle<MasterBankDetails>(),
                MasterEmergencyContactViewModel = employeeMasterData.ConvertToSingle<MasterEmergencyContactViewModel>(),
                MasterReportingDetails = employeeMasterData.ConvertToSingle<MasterReportingDetails>(),
                MasterFamilyDetails = employeeMasterData.ConvertToSingle<MasterFamilyDetails>(),
                MasterDependentDetails = employeeMasterData.ConvertToSingle<MasterDependentDetails>(),
                MasterWorkExperience = masterWorkExperiences
            };

            return employeeFormViewModel;
        }

        //Get All BGV Data for Admin



        public async Task<Result> GetBackGroundFormDetailsAsync()
        {
            var bgvRepository = _UnitOfWork.GetRepository<Bgvform>();  // Using generic repository
            var previousEmploymentRepository = _UnitOfWork.GetRepository<PreviousEmployment>();  // Using generic repository

            // Fetch all records from both tables
            var bgvForms = await bgvRepository.GetAllAsync();
            var previousEmployments = await previousEmploymentRepository.GetAllAsync();

            // Combine the results in a dictionary or an object
            var responseData = new
            {
                BgvForms = bgvForms,
                PreviousEmployments = previousEmployments
            };

            return new Result { Success = true, Data = responseData };
        }


        public async Task<Result> GetMasterSheetFormDetailsAsync()
        {
            var MasterRepository = _UnitOfWork.GetRepository<EmployeeMaster>();  // Using generic repository
            

            // Fetch all records from both tables
            var masteremployee = await MasterRepository.GetAllAsync();
      

            // Combine the results in a dictionary or an object
            var responseData = new
            {
                MasterForm = masteremployee
                
            };

            return new Result { Success = true, Data = responseData };
        }




    }
}
