using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.PunchDetails;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Dashboard
{
    public class Dashboardservice : IdashboardService
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public Dashboardservice(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _connectionString = ConnectionStringConfiguration._DefaultConnectionString;
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is not configured properly.");
            }
            _unitOfWork = unitOfWork;
        }

        //Get Employee Dynamic Data for the Manager Partial View
        public List<EmployeeViewModel> GetEmployeesByManager(int managerId)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeeCountByManager", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ManagerId", managerId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new EmployeeViewModel
                            {
                                EmpId = Convert.ToInt32(reader["EmpId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Count = Convert.ToInt32(reader["EmployeeCount"])
                            });
                        }
                    }
                }
            }
            return employees;
        }

        //Get Employee Dynamic Data for the Admin Partial View

        public List<EmployeeViewModel> GetAllEmployeeDataForAdmin()
        {
            List<EmployeeViewModel> employeesforadmin = new List<EmployeeViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllEmployeeDetailsForAdmin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeesforadmin.Add(new EmployeeViewModel
                            {
                                EmpId = Convert.ToInt32(reader["EmpId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Title = reader["Title"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                //DateOfBirth = reader["DateOfBirth"].ToString(),
                                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]),
                                NewEmployeesLast15Days = Convert.ToInt32(reader["NewEmployeesLast15Days"]),
                                UpcomingBirthdaysNextMonth = Convert.ToInt32(reader["UpcomingBirthdaysNextMonth"]),
                                UpcomingWorkAnniversariesNextMonth = Convert.ToInt32(reader["UpcomingWorkAnniversariesNextMonth"]),
                                PunchDetails = new PunchDetailsViewModel
                                {
                                    Empid = Convert.ToInt32(reader["EmpId"]),
                                    //EmployeeName= reader["LastName"].ToString() + reader["LastName"].ToString()

                                }
                            });
                        }
                    }
                }
            }
            return employeesforadmin;

        }

        public List<EmployeeViewModel> GetDataForUserDashboard(int userId)
        {
            List<EmployeeViewModel> useremployees = new List<EmployeeViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DataForUserPartialView", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empid", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            useremployees.Add(new EmployeeViewModel
                            {
                                EmpId = Convert.ToInt32(reader["EmpId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                //Count = Convert.ToInt32(reader["EmployeeCount"]),
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(),
                                Country = reader["Country"].ToString(),
                                ZipCode = reader["ZipCode"].ToString(),
                                CellPhone = reader["CellPhone"].ToString(),
                                Degree = reader["CourseName"].ToString(),
                                institution = reader["InstitutionName"].ToString(),
                                year = reader["PassedDate"].ToString(),
                                qual = reader["Details"].ToString(),

                            });
                        }
                    }
                }
            }
            return useremployees;
        }


        #region Punch Details
        //public async Task<Result> UpdatePunchOutAsync(int EmpId)
        //{
        //    try
        //    {
        //        var PunchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();  // Using generic repository
        //        var existingPunch = PunchRepository.GetAllAsync().Result;  // Fetch existing role by ID

        //        existingPunch = existingPunch.Where(x => x.Empid == EmpId).OrderByDescending(a => a.Id).ToList();
        //        var EntityUpdate = existingPunch.FirstOrDefault();

        //        if (EntityUpdate == null)
        //        {
        //            return new Result
        //            {
        //                Success = false,
        //                Message = "Punch Detail not found."
        //            };
        //        }
        //        else
        //        {
        //            // Get the current time and round to the nearest second
        //            var currentTime = DateTime.Now;
        //            var roundedTime = new TimeOnly(currentTime.Hour, currentTime.Minute, currentTime.Second);

        //            // Update the PunchOutTime
        //            EntityUpdate.Punchouttime = roundedTime;

        //            await PunchRepository.UpdateAsync(EntityUpdate);
        //            await _unitOfWork.SaveAsync();

        //            return new Result
        //            {
        //                Success = true,
        //                Message = "Punch Detail updated successfully."
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result
        //        {
        //            Success = false,
        //            Message = $"Error updating Punch Detail: {ex.Message}"
        //        };
        //    }
        //}



        //public async Task<Result> SavePunchInAsync(int EmpId, string userName)
        //{
        //    try
        //    {
        //        // Get the current time and round to the nearest second
        //        var currentTime = DateTime.Now;
        //        var roundedTime = new TimeOnly(currentTime.Hour, currentTime.Minute, currentTime.Second);

        //        // Create a new EmployeePunchDetail instance
        //        var punchIn = new EmployeePunchDetail
        //        {
        //            Empid = EmpId,
        //            EmployeeName = userName,
        //            Month = DateTime.Now.ToString("MMMM"),  // Month from DateTime

        //            // Convert DateTime to DateOnly for PunchDate
        //            PunchDate = DateOnly.FromDateTime(DateTime.Now),

        //            // Use the rounded time (excluding fractional seconds)
        //            Punchintime = roundedTime
        //        };

        //        // Get the repository for EmployeePunchDetail and add the new punchIn record
        //        var punchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
        //        await punchRepository.AddAsync(punchIn);
        //        await _unitOfWork.SaveAsync();

        //        // Return a success result
        //        return new Result
        //        {
        //            Success = true,
        //            Message = "Punch In data saved successfully."
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle and return error result in case of failure
        //        return new Result
        //        {
        //            Success = false,
        //            Message = $"Error saving Punch In data: {ex.Message}"
        //        };
        //    }
        //}

        public async Task<Result> SavePunchInAsync(int EmpId, string userName)
        {
            try
            {
                var punchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
                var today = DateOnly.FromDateTime(DateTime.Now);
                var currentTime = DateTime.Now;

                var punchIn = new EmployeePunchDetail
                {
                    Empid = EmpId,
                    EmployeeName = userName,
                    Month = currentTime.ToString("MMMM"),
                    PunchDate = DateOnly.FromDateTime(currentTime),
                    Punchintime = DateTime.Now.ToString(),
                    Punchouttime = null
                };

                await punchRepository.AddAsync(punchIn);
                await _unitOfWork.SaveAsync();

                return new Result { Success = true, Message = "Punch in recorded successfully." };
            }
            catch (Exception ex)
            {
                return new Result { Success = false, Message = $"Error: {ex.Message}" };
            }
        }


        public async Task<Result> UpdatePunchOutAsync(int EmpId)
        {
            try
            {
                var punchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
                var today = DateOnly.FromDateTime(DateTime.Now);

                var existingPunch = (await punchRepository.GetAllAsync())
                    .Where(x => x.Empid == EmpId && x.Punchouttime == null)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (existingPunch == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "No active punch-in found. Please punch in first."
                    };
                }


                existingPunch.Punchouttime = DateTime.Now.ToString();

                await punchRepository.UpdateAsync(existingPunch);
                    await _unitOfWork.SaveAsync();

                return new Result { Success = true, Message = "Punch out recorded successfully." };
            }
            catch (Exception ex)
                    {
                return new Result { Success = false, Message = $"Error: {ex.Message}" };
                }
            }



        //public async Task<string> GetPunchStatusAsync(int empId)
        //{
        //    var punchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
        //    var today = DateOnly.FromDateTime(DateTime.Now);

        //    var todayPunch = (await punchRepository.GetAllAsync())
        //        .Where(x => x.Empid == empId)
        //        .OrderByDescending(x => x.Id)
        //        .FirstOrDefault();

        //    if (todayPunch == null)
        //    {
        //        return "CanPunchIn"; // No punch yet today
        //    }

        //    var punchInDateTime = new DateTime(
        //            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //            todayPunch.Punchintime.Value.Hour,
        //            todayPunch.Punchintime.Value.Minute,
        //            todayPunch.Punchintime.Value.Second
        //        );

        //    var totalHours = DateTime.Now - punchInDateTime;

        //    if (totalHours.TotalMinutes <= 1 && todayPunch.Punchouttime == null)
        //    {

        //        return "CanPunchOut"; // Currently punched in
        //    }

        //    else
        //    {
        //        //punchoutTime == N/A
        //        return "CanPunchIn";
        //    }

        //    // Punch out already done - check if 8 hours passed
        //    if (todayPunch.Punchouttime != null)
        //    {
        //        var punchOutDateTime = new DateTime(
        //            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
        //            todayPunch.Punchouttime.Value.Hour,
        //            todayPunch.Punchouttime.Value.Minute,
        //            todayPunch.Punchouttime.Value.Second
        //        );

        //        var elapsed = DateTime.Now - punchOutDateTime;

        //        if (elapsed.TotalMinutes >= 1)
        //        {
        //            return "CanPunchIn"; // Cooldown expired
        //        }
        //        else
        //        {
        //            return "Cooldown"; // Still within cooldown
        //        }
        //    }

        //    return "CanPunchIn"; // Default fallback
        //}


        public async Task<string> GetPunchStatusAsync(int empId)
            {
            var punchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();

            var todayPunch = (await punchRepository.GetAllAsync())
                .Where(x => x.Empid == empId)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (todayPunch == null)
                {
                return "CanPunchIn"; // No punch yet today
        }
            if (!string.IsNullOrEmpty(todayPunch.Punchintime) && string.IsNullOrEmpty(todayPunch.Punchouttime))
            {
                DateTime punchInTime = DateTime.Parse(todayPunch.Punchintime);
                DateTime currentTime = DateTime.Now;

                var minutesSincePunchIn = (currentTime - punchInTime).TotalMinutes;

                if (minutesSincePunchIn < 600)
        {
                    return "CanPunchOut";
                }
                else
            {
                    return "CanPunchIn"; // Allow punch in again after long hours
                }
            }
            else if (!string.IsNullOrEmpty(todayPunch.Punchintime) && !string.IsNullOrEmpty(todayPunch.Punchouttime))
                {
                DateTime punchOutTime = DateTime.Parse(todayPunch.Punchouttime);
                DateTime currentTime = DateTime.Now;

                var minutesSincePunchOut = (currentTime - punchOutTime).TotalMinutes;

                if (minutesSincePunchOut >= 2)
                {
                    return "CanPunchIn"; // Enable punch in after 8 hours cooldown
            }
                else
                {
                    return "Cooldown"; // Still in 8-hour cooldown
                }
            }


                return "CanPunchIn";
            
            //return "CanPunchIn"; // Default fallback
        }







        #endregion

    }
}

