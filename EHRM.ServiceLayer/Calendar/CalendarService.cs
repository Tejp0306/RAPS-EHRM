using EHRM.ServiceLayer.Employee;
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

        public CalendarService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EHRMConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("Database connection string is not configured properly.");
            }
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
                                HolidayDate = Convert.ToDateTime(reader["HolidayDate"].ToString())

                            });
                        }
                    }
                }
            }
            return holidays;
        }
    }
}
