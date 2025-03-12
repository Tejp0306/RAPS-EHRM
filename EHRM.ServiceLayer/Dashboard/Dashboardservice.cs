using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.PunchDeatils;
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

        public Dashboardservice(IConfiguration configuration,IUnitOfWork unitOfWork)
        {
            _connectionString = configuration.GetConnectionString("EHRMConnection");
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
                                DateOfBirth = reader["DateOfBirth"].ToString(),
                                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"]),
                                PunchDetail= new PunchDetailsViewModel
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
        public async Task<Result> UpdatePunchOutAsync(int EmpId)
        {
            try
            {
                var PunchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();  // Using generic repository
                var existingPunch = PunchRepository.GetAllAsync().Result;  // Fetch existing role by ID

                existingPunch = existingPunch.Where(x => x.Empid == EmpId).OrderByDescending(a => a.Id).ToList();
                var EntityUpdate = existingPunch.FirstOrDefault();

                if (EntityUpdate == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Punch Detail not found."
                    };
                }
                else
                {
                    // Format the current time as HH:mm:ss (24-hour format)
                    EntityUpdate.Punchouttime = DateTime.Now.ToString("HH:mm:ss");

                    await PunchRepository.UpdateAsync(EntityUpdate);
                    await _unitOfWork.SaveAsync();

                    return new Result
                    {
                        Success = true,
                        Message = "Punch Detail updated successfully."
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Punch Detail: {ex.Message}"
                };
            }
        }


        public async Task<Result> SavePunchInAsync(int EmpId , string userName)
        {
            try
            {
                var punchIn = new EmployeePunchDetail
                {
                    Empid = EmpId,
                    EmployeeName = userName,
                    Month = DateTime.Now.ToString("MMMM"),// model.Month,
                    PunchDate = DateTime.Now.ToString("yyyy-MM-dd"), // model.PunchDate,  // Convert DateTime to DateOnly
                    Punchintime = DateTime.Now.ToString("HH:mm:ss"), //model.PunchInTime,

                };

                var punchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
                await punchRepository.AddAsync(punchIn);
                await _unitOfWork.SaveAsync();


                return new Result
                {
                    Success = true,
                    Message = "Punch In data saved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error saving Punch In data: {ex.Message}"
                };
            }
        }

        #endregion

    }
}

