using EHRM.DAL.Database;
using EHRM.ServiceLayer.Calendar;
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
        public JsonResult GetEvents(int empId)
        {
            if (empId <= 0)
            {
              
                return Json(new { error = "Invalid Employee ID" }); // Return error response
            }

            Console.WriteLine($"GetEvents called with empId: {empId}"); // Debugging

            // Get holidays for the given empId
            List<CalendarViewModel> holidays = _calendar.GetHolidayByEmpIdTeamId(empId);

            // Initialize a list of events
            var events = new List<object>();

            // Add hardcoded events for testing
            events.Add(new { title = "Meeting", start = "2025-02-08T10:00:00", end = "2025-02-08T12:00:00" });
            events.Add(new { title = "Workshop", start = "2025-02-09", allDay = true });
            events.Add(new { title = "Lunch Break", start = "2025-02-10T13:00:00", end = "2025-02-10T14:00:00" });

            // Loop through holidays to create dynamic events
            foreach (var holiday in holidays)
            {
                // Format the holiday date to the desired format: "yyyy-MM-ddT10:00:00"
                string formattedDate = holiday.HolidayDate.ToString("yyyy-MM-dd") + "T10:00:00";

                // Assuming your model has these properties: HolidayName, HolidayDate, and AllDay
                var holidayEvent = new
                {
                    title = holiday.HolidayName,  // Holiday title
                    start = formattedDate,        // Formatted start date with time
                    end = formattedDate,          // Same end date as start for single-day holiday
                    allDay = true                 // If it's an all-day event, set it to true
                };

                events.Add(holidayEvent);  // Add the holiday event to the list
            }

            // Return the events as JSON
            return new JsonResult(events);
        }



    }
}



