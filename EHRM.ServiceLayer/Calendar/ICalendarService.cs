using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Calendar
{
    public interface ICalendarService
    {
       List<CalendarViewModel> GetHolidayByEmpIdTeamId(int empId);
    }
}
