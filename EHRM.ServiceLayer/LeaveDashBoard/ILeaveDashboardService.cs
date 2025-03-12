using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Leave;

namespace EHRM.ServiceLayer.LeaveDashBoard
{
    public interface ILeaveDashboardService
    {

        Task<LeaveSummaryViewModel> CalculateLeavePolicy(DateTime startDate);

    }

}
