using EHRM.DAL.Database;
using EHRM.ServiceLayer.Calendar;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Models;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
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
                    punchInTime = !string.IsNullOrEmpty(punch.PunchInTime)
                                  ? DateTime.Parse(punch.PunchInTime).ToString("HH:mm:ss")
                                  : null,

                    punchOutTime = !string.IsNullOrEmpty(punch.PunchOutTime)
                                    ? DateTime.Parse(punch.PunchOutTime).ToString("HH:mm:ss")
                                    : null,

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
                    // Get today's date string in "yyyy-MM-dd" format
                    var todayString = DateTime.Now.ToString("yyyy-MM-dd");

                    // Filter data by today's date by default
                    punchData = punchData.Where(p => p.PunchDate == todayString);

                    // If a punchDate filter is provided, filter the data based on that date
                    if (!string.IsNullOrEmpty(punchDate))
                    {
                        // Validate that the punchDate string is a valid date
                        if (DateOnly.TryParse(punchDate, out var parsedDate))
                        {
                            var parsedDateString = parsedDate.ToString("yyyy-MM-dd");
                            punchData = punchData.Where(p => p.PunchDate == parsedDateString);
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
                        PunchDate = punch.PunchDate,  // Format the DateOnly to string
                        Punchintime = !string.IsNullOrEmpty(punch.Punchintime)
                                  ? DateTime.Parse(punch.Punchintime).ToString("HH:mm:ss")
                                  : null,

                        Punchouttime = !string.IsNullOrEmpty(punch.Punchouttime)
                                    ? DateTime.Parse(punch.Punchouttime).ToString("HH:mm:ss")
                                    : null,

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


        //[HttpGet]
        //public async Task<JsonResult> GetPunch(int empId, int? month) // month is optional
        //{
        //    // Fetch the punch details from the service layer
        //    var result = await _calendar.GetPunchDetailsAsync();

        //    if (result.Success && result.Data != null)
        //    {
        //        var punchData = result.Data as IEnumerable<EmployeePunchDetail>;
        //        if (punchData != null)
        //        {
        //            // Filter the data based on empId
        //            var filteredPunchData = punchData.Where(p => p.Empid == empId);

        //            // If a month is selected, filter the data by the month parsed from the PunchDate string
        //            if (month.HasValue)
        //            {
        //                filteredPunchData = filteredPunchData.Where(p =>
        //                    DateTime.TryParse(p.PunchDate, out DateTime punchDate) && punchDate.Month == month.Value);
        //            }

        //            // Map the filtered data to the desired format
        //            var punchList = filteredPunchData.Select(punch => new
        //            {
        //                Id = punch.Id,
        //                Month = punch.Month,
        //                PunchDate = punch.PunchDate,
        //                Punchintime = !string.IsNullOrEmpty(punch.Punchintime)
        //                              ? DateTime.Parse(punch.Punchintime).ToString("HH:mm:ss")
        //                              : null,
        //                Punchouttime = !string.IsNullOrEmpty(punch.Punchouttime)
        //                               ? DateTime.Parse(punch.Punchouttime).ToString("HH:mm:ss")
        //                               : null,
        //                Totalhours = punch.TotalHours,
        //            }).ToList();

        //            return Json(punchList);
        //        }

        //        return Json(new { Success = false, Message = "Invalid data format." });
        //    }

        //    return Json(new { Success = false, Message = result.Message ?? "No data found." });
        //}

        //[HttpGet]
        //public async Task<JsonResult> GetPunch(int empId, int month, int year)
        //{
        //    var result = await _calendar.GetPunchDetailsAsync();

        //    if (result.Success && result.Data != null)
        //    {
        //        var punchData = result.Data as IEnumerable<EmployeePunchDetail>;

        //        var filtered = punchData
        //            .Where(p => p.Empid == empId && DateTime.TryParse(p.PunchDate, out var date)
        //                        && date.Month == month && date.Year == year)
        //            .Select(p => new
        //            {
        //                p.Id,
        //                p.Month,
        //                PunchDate = DateTime.Parse(p.PunchDate).ToString("yyyy-MM-dd"),
        //                Punchintime = !string.IsNullOrEmpty(p.Punchintime) ? DateTime.Parse(p.Punchintime).ToString("HH:mm:ss") : "",
        //                Punchouttime = !string.IsNullOrEmpty(p.Punchouttime) ? DateTime.Parse(p.Punchouttime).ToString("HH:mm:ss") : "",
        //                Totalhours = p.TotalHours
        //            });

        //        return Json(filtered);
        //    }

        //    return Json(new { Success = false, Message = "No data found." });
        //}

        [HttpGet]
        public async Task<JsonResult> GetPunch(int empId, int month, int year)
        {
            var result = await _calendar.GetPunchDetailsAsync();

            if (result.Success && result.Data != null)
            {
                var punchData = result.Data as IEnumerable<EmployeePunchDetail>;

                // Parse and filter punch data by employee, month, and year
                var filteredPunches = punchData
                    .Where(p => p.Empid == empId && DateTime.TryParse(p.PunchDate, out var date)
                                && date.Month == month && date.Year == year)
                    .Select(p => new
                {
                        p.Id,
                        p.Month,
                        PunchDate = DateTime.TryParse(p.PunchDate, out var punchDate) ? punchDate.ToString("yyyy-MM-dd") : "",
                        PunchDateObj = DateTime.TryParse(p.PunchDate, out var punchDate2) ? punchDate2 : (DateTime?)null,
                        Punchintime = DateTime.TryParse(p.Punchintime, out var inTime) ? inTime.ToString("HH:mm:ss") : "",
                        Punchouttime = DateTime.TryParse(p.Punchouttime, out var outTime) ? outTime.ToString("HH:mm:ss") : "",
                        Totalhours = p.TotalHours
                    })
                    .Where(p => p.PunchDateObj != null)
                    .ToList();

                // Get leave data from service
                var leaveData = _calendar.GetLeaveDetailsByEmployee(empId);

                var formattedLeaves = leaveData.Select(ld => new
                    {
                    ld.EmpId,
                    ld.LeaveApplyId,
                    ld.LeaveType,
                    ld.LeaveStatus,
                    ld.LeaveId,
                    LeaveFrom = DateTime.TryParse(ld.LeaveFrom, out var from) ? from : (DateTime?)null,
                    LeaveTo = DateTime.TryParse(ld.LeaveTo, out var to) ? to : (DateTime?)null
                }).Where(ld => ld.LeaveFrom != null && ld.LeaveTo != null && ld.LeaveStatus == "Approved").ToList();

                // Generate all dates in the selected month
                var daysInMonth = DateTime.DaysInMonth(year, month);
                var allDatesInMonth = Enumerable.Range(1, daysInMonth)
                    .Select(day => new DateTime(year, month, day))
                    .ToList();

                var filtered = allDatesInMonth.Select(date =>
                {
                    var punch = filteredPunches.FirstOrDefault(p => p.PunchDateObj == date);

                    var leaveType = formattedLeaves
                        .Where(leave => date >= leave.LeaveFrom && date <= leave.LeaveTo)
                        .Select(leave => leave.LeaveType)
                        .FirstOrDefault();

                    return new
                    {
                        Month = date.ToString("MMMM"),
                        PunchDate = date.ToString("yyyy-MM-dd"),
                        Punchintime = punch != null && string.IsNullOrEmpty(leaveType) ? punch.Punchintime : "",
                        Punchouttime = punch != null && string.IsNullOrEmpty(leaveType) ? punch.Punchouttime : "",
                        Totalhours = punch != null && string.IsNullOrEmpty(leaveType) ? punch.Totalhours : 0,
                        LeaveType = leaveType ?? ""
                    };
                    }).ToList();

                return Json(filtered);
                }

            return Json(new { Success = false, Message = "No data found." });
                }












    }
}



