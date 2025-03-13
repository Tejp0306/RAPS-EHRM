using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Employee;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Calendar
{
    public class CalendarService : ICalendarService
    {
        private readonly string _connectionString;
        private readonly IUnitOfWork _unitOfWork;

        public CalendarService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _connectionString = configuration.GetConnectionString("EHRMConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is not configured properly.");
            }
            _unitOfWork = unitOfWork;
        }
        public List<CalendarViewModel> GetHolidayByEmpIdTeamId(int empid)
        {
            List<CalendarViewModel> holidays = new List<CalendarViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetHolidayDataByEmpId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empid", empid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            holidays.Add(new CalendarViewModel
                            {
                                HolidayName = reader["HolidayName"].ToString(),
                                HolidayDate = Convert.ToDateTime(reader["HolidayDate"].ToString()),
                                


                            });
                        }
                    }
                }
            }
            return holidays;
        }

        public List<CalendarViewModel> GetPunchDetailsByEmpId(int empid)
        {
            List<CalendarViewModel> holidays = new List<CalendarViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeePunchDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empid", empid);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            holidays.Add(new CalendarViewModel
                            {
                               
                                PunchDate = Convert.ToDateTime(reader["PunchDate"].ToString()),
                                PunchInTime = reader["PunchInTime"].ToString(),
                                PunchOutTime = reader["PunchOutTime"].ToString(),
                                TotalHours = reader["TotalHours"].ToString(),

                            });
                        }
                    }
                }
            }
            return holidays;
        }



        public async Task<Result> GetPunchAsync()
        {
            var PunchRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
            var lt = await PunchRepository.GetAllAsync();
            return new Result { Success = true, Data = lt };
        }

        public async Task<Result> GetPunchDetailsAsync()
        {
            var PunchDetailsRepository = _unitOfWork.GetRepository<EmployeePunchDetail>();
            var lt = await PunchDetailsRepository.GetAllAsync();
            return new Result { Success = true, Data = lt };
        }
    }
}
