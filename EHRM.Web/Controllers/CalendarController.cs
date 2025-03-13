using EHRM.DAL.Database;
using EHRM.ServiceLayer.Calendar;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Models;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EHRM.Web.Controllers

{
    public class CalendarController : Controller
    {
        private readonly ICalendarService _calendar;
        
        public CalendarController(ICalendarService calendar)
        {

            _calendar = calendar;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCombinedEvents(int empId)
        {
            if (empId <= 0)
            {
                return Json(new { error = "Invalid Employee ID" }); // Return error response
            }

            Console.WriteLine($"GetCombinedEvents called with empId: {empId}"); // Debugging

            // Get holidays for the given empId
            List<CalendarViewModel> holidays = _calendar.GetHolidayByEmpIdTeamId(empId);

            // Get punch details for the given empId
            List<CalendarViewModel> punchDetails = _calendar.GetPunchDetailsByEmpId(empId);

            // Initialize a list of events
            var events = new List<object>();




            // Loop through holidays to create dynamic events

            foreach (var holiday in holidays)
            {
                string formattedDate = holiday.HolidayDate.ToString("yyyy-MM-dd") + "T10:00:00"; // Format holiday date

                var holidayEvent = new
                {
                    title = holiday.HolidayName,  // Holiday title
                    start = formattedDate,        // Formatted start date with time
                    end = formattedDate,          // Same end date as start for single-day holiday
                    allDay = true                 // All-day event
                };

                events.Add(holidayEvent); // Add holiday event to the list
            }

            // Add punch details events
            foreach (var punch in punchDetails)
            {
                string formattedDate = punch.PunchDate.ToString("yyyy-MM-dd") + "T10:00:00"; // Format punch date

                var punchEvent = new
                {
                    title = "Punch Details",           // Add a title for the punch event
                    start = formattedDate,             // FullCalendar expects a "start" field
                    allDay = true,                     // Mark it as an all-day event
                    punchInTime = punch.PunchInTime,   // Include punch in time
                    punchOutTime = punch.PunchOutTime, // Include punch out time
                    totalHours = !string.IsNullOrEmpty(punch.PunchInTime) && !string.IsNullOrEmpty(punch.PunchOutTime)
                                 ? CalculateTotalHours(punch.PunchInTime, punch.PunchOutTime) // Calculate total hours
                                 : null  // If no punch times, leave total hours as null
                };

                events.Add(punchEvent);  // Add punch event to the list
            }

            return new JsonResult(events);  // Return the combined list of events
        }

        // Helper method to calculate total hours between PunchInTime and PunchOutTime
        private string CalculateTotalHours(string punchInTime, string punchOutTime)
        {
            try
            {
                DateTime punchIn = DateTime.Parse(punchInTime);
                DateTime punchOut = DateTime.Parse(punchOutTime);
                double totalHours = (punchOut - punchIn).TotalHours;

                return totalHours.ToString("F2");  // Format total hours to 2 decimal places
            }
            catch (Exception)
            {
                return null;  // Return null in case of any parsing error
            }
        }


        public IActionResult PunchStatus()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetPunchData(string punchDate = null)
        {
            // Fetch the result from the service layer
            var result = await _calendar.GetPunchAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var punchData = result.Data as IEnumerable<EmployeePunchDetail>;

                if (punchData != null)
                {
                    // Filter data by today's date by default
                    var today = DateOnly.FromDateTime(DateTime.Now);
                    punchData = punchData.Where(p => p.PunchDate == today);

                    // If a punchDate filter is provided, filter the data based on that date
                    if (!string.IsNullOrEmpty(punchDate))
                    {
                        // Convert the punchDate string to DateOnly for comparison
                        if (DateOnly.TryParse(punchDate, out var parsedDate))
                        {
                            punchData = punchData.Where(p => p.PunchDate == parsedDate);
                    }
                    else
                    {
                            return Json(new { Success = false, Message = "Invalid date format." });
                    }
                    }

                    // Return the filtered list as a JSON response
                    var punchList = punchData.Select(punch => new
                    {
                        EmployeeName = punch.EmployeeName,
                        PunchDate = punch.PunchDate?.ToString("yyyy-MM-dd"),  // Format the DateOnly to string
                        Punchintime = punch.Punchintime?.ToString("HH:mm:ss"), // Format the TimeOnly to string
                        Punchouttime = punch.Punchouttime?.ToString("HH:mm:ss"), // Format the TimeOnly to string
                        Totalhours = punch.TotalHours,
                    }).ToList();

                    return Json(new { Success = true, Data = punchList });
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format." });
                }
            }

            // In case of failure to fetch data
            return Json(new { Success = false, Message = "Failed to fetch punch data." });
        }


        [HttpGet]
        public async Task<JsonResult> GetPunch(int empId) // Add empId as parameter
        {
            // Fetch the result from the service layer
            var result = await _calendar.GetPunchDetailsAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var punchData = result.Data as IEnumerable<EmployeePunchDetail>;

                if (punchData != null)
                {
                    // Filter the data based on empId
                    var filteredPunchData = punchData.Where(punch => punch.Empid == empId); // Filter by empId

                    // If a punchDate filter is provided, you can still filter based on it if you need
                    var punchList = filteredPunchData.Select(punch => new
                    {
                        Id = punch.Id,
                        PunchDate = punch.PunchDate,
                        Punchintime = punch.Punchintime,
                        Punchouttime = punch.Punchouttime,
                        Totalhours = punch.TotalHours,
                    }).ToList();

                    // Return the filtered list as a JSON response
                    return Json(punchList);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No data found." });
            }
        }



    }
}



