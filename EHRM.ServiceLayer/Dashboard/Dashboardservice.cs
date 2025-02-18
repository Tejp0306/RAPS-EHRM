using EHRM.DAL.Database;
using EHRM.ServiceLayer.Employee;
using EHRM.ViewModel.Employee;
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

        public Dashboardservice(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EHRMConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is not configured properly.");
            }
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
                                EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
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


    }
}

