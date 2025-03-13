using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Models
{
    public class CalendarViewModel
    {
        public string HolidayName { get; set; }

        public DateTime HolidayDate { get; set; }
        public DateTime PunchDate { get; set; }
        public string PunchInTime { get; set; }
        public string PunchOutTime { get; set; }
        public string TotalHours { get; set; }
    }
}
