using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Document;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Review;
using EHRM.ViewModel.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        #region TimeSheet
        public async Task<Result> CreateTimeSheetAsync(TimeSheetViewModel model, List<string> FilePath)
        {
            try
            {
                // Ensure DailyEntries are provided
                if (model.DailyEntries == null || !model.DailyEntries.Any())
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Please fill in the daily timesheet entries."
                    };
                }

                TimeSheet timesheet;

                // Check if the timesheet exists by Id, otherwise create a new one
                if (model.Id > 0)
                {
                    // Update existing timesheet
                    timesheet = await _context.TimeSheets
                        .Include(t => t.DailyEntries)
                        .FirstOrDefaultAsync(t => t.Id == model.Id);

                    if (timesheet == null)
                    {
                        return new Result
                        {
                            Success = false,
                            Message = "Timesheet not found."
                        };
                    }

                    // Update the existing timesheet fields
                    timesheet.Id = model.Id;
                    timesheet.PresentMonth = model.PresentMonth;
                    timesheet.EmpName = model.EmpName;
                    timesheet.ClientName = model.ClientName;
                    timesheet.Position = model.Position;
                    timesheet.ProjectName = model.ProjectName;
                    timesheet.EmployeeSignature = model.EmployeeSignature;
                    timesheet.ManagerSignature = model.ManagerSignature;
                    timesheet.SignatureDate = model.SignatureDate;
                    timesheet.SubmissionDate = model.SubmissionDate;
                    timesheet.Note = model.Note;
                    timesheet.TotalHours = model.TotalHours;
                    timesheet.UpdatedAt = DateTime.Now;

                    // Update daily entries
                    foreach (var entry in model.DailyEntries)
                    {
                        var dailyEntry = timesheet.DailyEntries
                            .FirstOrDefault(de => de.DayDate?.ToString("dd-MM-yyyy") == entry.DayDate);
                        if (dailyEntry != null)
                        {
                            dailyEntry.HoursWorked = decimal.TryParse(entry.HoursWorked, out var hours) ? hours : 0;
                            dailyEntry.AssignmentDesc = entry.AssignmentDesc;
                            dailyEntry.Remarks = entry.Remarks;
                            dailyEntry.UpdatedAt = DateTime.Now;
                        }
                    }
                }
                else
                {
                    string paths = string.Empty;
                    foreach (var entry in model.FilePath)
                    {
                        paths +=entry+",";


                    }
                    Console.WriteLine(paths);

                    // Create a new timesheet
                    timesheet = new TimeSheet
                    {
                        Id = model.Id,
                        PresentMonth = model.PresentMonth,
                        FilePaths = paths,
                        EmpId = model.EmpId,
                        EmpName = model.EmpName,
                        ClientName = model.ClientName,
                        Position = model.Position,
                        ProjectName = model.ProjectName,
                        EmployeeSignature = model.EmployeeSignature,
                        ManagerSignature = model.ManagerSignature,
                        SignatureDate = model.SignatureDate,
                        SubmissionDate = model.SubmissionDate,
                        Note = model.Note,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        TotalHours = model.TotalHours
                    };

                    // Loop through the daily entries and create records for them
                    foreach (var dailyEntry in model.DailyEntries)
                    {
                        DateOnly? _daydate = DateOnly.FromDateTime(DateTime.ParseExact(dailyEntry.DayDate, "d-M-yyyy", CultureInfo.InvariantCulture));
                        var newDailyEntry = new DailyEntry
                        {
                            DayDate = _daydate,  // This will be null if parsing failed
                            DayOfWeek = dailyEntry.DayOfWeek,
                            EmpId = model.EmpId,
                            HoursWorked = string.IsNullOrEmpty(dailyEntry.HoursWorked) ? 0 : Convert.ToDecimal(dailyEntry.HoursWorked),
                            AssignmentDesc = dailyEntry.AssignmentDesc,
                            Remarks = dailyEntry.Remarks,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        // Add the daily entry to the Timesheet
                        timesheet.DailyEntries.Add(newDailyEntry);
                    }

                    // Add the new timesheet and daily entries to the database
                    await _UnitOfWork.GetRepository<TimeSheet>().AddAsync(timesheet);
                }

                // Save changes to the database
                await _UnitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Timesheet saved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error saving record: {ex.Message}"
                };
            }
        }

        public async Task<TimeSheetViewModel> GetTimeSheetByIdAsync(int EmpId)
        {
            try
            {
                var timesheet = await _context.TimeSheets
                    .Include(t => t.DailyEntries).OrderByDescending(t => t.Id)
                    .FirstOrDefaultAsync(t => t.EmpId == EmpId);

                if (timesheet == null)
                {

                    return null;
                }

                // Map the timesheet to the view model
                var timesheetViewModel = new TimeSheetViewModel
                {
                    Id = timesheet.Id,
                    PresentMonth = timesheet.PresentMonth,
                    EmpName = timesheet.EmpName,
                    ClientName = timesheet.ClientName,
                    Position = timesheet.Position,
                    ProjectName = timesheet.ProjectName,
                    EmployeeSignature = timesheet.EmployeeSignature,
                    ManagerSignature = timesheet.ManagerSignature,
                    SignatureDate = timesheet.SignatureDate,
                    SubmissionDate = timesheet.SubmissionDate,
                    Note = timesheet.Note,
                    TotalHours = timesheet.TotalHours,
                    DailyEntries = timesheet.DailyEntries.Select(de => new DailyEntryModel
                    {
                        DayDate = de.DayDate?.ToString("dd-MM-yyyy"),
                        DayOfWeek = de.DayOfWeek,
                        HoursWorked = de.HoursWorked?.ToString() ?? "0",
                        AssignmentDesc = de.AssignmentDesc,
                        Remarks = de.Remarks
                    }).ToList()
                };

                return timesheetViewModel;
    }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching the timesheet.", ex);
            }
        }

        public async Task<Result> GetTimeSheetByMonthAsync(string month)
        {
            var timeSheetRepository = _UnitOfWork.GetRepository<TimeSheet>();  // Using generic repository

            // Fetch all timesheets and filter by month (assuming `Date` is a DateTime column)
            var timesheets = await timeSheetRepository.GetAllAsync();

            // Filter the timesheets by the month
            var filteredTimesheets = timesheets.Where(t => t.PresentMonth == month).ToList();

            return new Result { Success = true, Data = filteredTimesheets };
        }

        public async Task<TimeSheetViewModel> GetTimeSheetsByIdAsync(int id)
        {
            try
            {
                // Fetch the timesheet by ID and include DailyEntries in the result
                var timesheet = await _context.TimeSheets
                    .Include(t => t.DailyEntries)  // Include related DailyEntries
                    .FirstOrDefaultAsync(t => t.Id == id);  // Filter by the timesheet ID

                // If no timesheet is found, return null
                if (timesheet == null)
                {
                    return null;
                }

                // Map the fetched timesheet to a ViewModel
                var timesheetViewModel = new TimeSheetViewModel
                {
                    Id = timesheet.Id,
                    PresentMonth = timesheet.PresentMonth,
                    EmpName = timesheet.EmpName,
                    ClientName = timesheet.ClientName,
                    Position = timesheet.Position,
                    ProjectName = timesheet.ProjectName,
                    EmployeeSignature = timesheet.EmployeeSignature,
                    ManagerSignature = timesheet.ManagerSignature,
                    SignatureDate = timesheet.SignatureDate,
                    SubmissionDate = timesheet.SubmissionDate,
                    Note = timesheet.Note,
                    TotalHours = timesheet.TotalHours,
                    // Map the DailyEntries into DailyEntryModel objects
                    DailyEntries = timesheet.DailyEntries.Select(de => new DailyEntryModel
                    {
                        DayDate = de.DayDate?.ToString("dd-MM-yyyy"),
                        DayOfWeek = de.DayOfWeek,
                        HoursWorked = de.HoursWorked?.ToString() ?? "0",  // Default to "0" if HoursWorked is null
                        AssignmentDesc = de.AssignmentDesc,
                        Remarks = de.Remarks
                    }).ToList()
                };

                return timesheetViewModel;
            }
            catch (Exception ex)
            {
                // Log the exception (or handle it as needed)
                throw new Exception("An error occurred while fetching the timesheet.", ex);
            }
        }

        public async Task<Result> GetFilesAsync(int id)
        {
            try
            {
                var DocumentRepository = _UnitOfWork.GetRepository<TimeSheet>();  // Using generic repository
                var doc = await DocumentRepository.GetByIdAsync(id);  // Fetch role by ID

                if (doc == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Attachments not found."
                    };
                }

                var showdoc = new TimeSheetViewModel
                {
                    Id = doc.Id,
                    FilePath = doc.FilePaths.Split(',').ToList()


                };

                return new Result
                {
                    Success = true,
                    Data = showdoc
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching TimeSheet Attachments: {ex.Message}"
                };
            }
        }
    }





        #endregion
    }


