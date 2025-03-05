using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Leave;

namespace EHRM.ServiceLayer.LeaveDashBoard
{
    public class LeaveDashboardService : ILeaveDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<LeaveSummaryViewModel> CalculateLeavePolicy(DateTime startDate)
        {
            // Fetch leave policies from repository (if needed)
            var policyRepository = _unitOfWork.GetRepository<LeavePolicy>();


            List<LeavePolicy> policies = (await policyRepository.GetAllAsync()).ToList();

            // Calculate years of service
            double yearsOfService = (DateTime.Now - startDate).TotalDays / 365.25;

            // Initialize leave summary
            var leave = new LeaveSummaryViewModel();

            if (yearsOfService < 2)
            {
                // For 0 to less than 2 Years
                leave.CasualLeave = policies[0].CasualLeave;
                leave.SickLeave = policies[0].SickLeave;
                leave.EarnedLeave = policies[0].EarnedLeave;
                leave.EarnedLeaveAccrualRate = policies[0].EarnedLeaveAccrualRate; // 1.00 day per month
                leave.CarryForwardEL = policies[0].IsEarnedLeaveCarriedForward;
                leave.CarryForwardLimit = policies[0].CarryForwardLimit;
                leave.TotalLeave = leave.CasualLeave + leave.SickLeave + leave.EarnedLeave;
                leave.Tenure = (int)yearsOfService;
                
            }
            else if (yearsOfService >= 2 && yearsOfService < 5)
            {
                // For 2 to less than 5 Years
                leave.CasualLeave = policies[1].CasualLeave;
                leave.SickLeave = policies[1].SickLeave;
                leave.EarnedLeave = policies[1].EarnedLeave;
                leave.EarnedLeaveAccrualRate = policies[1].EarnedLeaveAccrualRate; // 1.00 day per month
                leave.CarryForwardEL = policies[1].IsEarnedLeaveCarriedForward;
                leave.CarryForwardLimit = policies[1].CarryForwardLimit;
                leave.TotalLeave = leave.CasualLeave + leave.SickLeave + leave.EarnedLeave;
            }
            else
            {
                // For 5+ Years
                leave.CasualLeave = policies[2].CasualLeave;
                leave.SickLeave = policies[2].SickLeave;
                leave.EarnedLeave = policies[2].EarnedLeave;
                leave.EarnedLeaveAccrualRate = policies[2].EarnedLeaveAccrualRate; // 1.00 day per month
                leave.CarryForwardEL = policies[2].IsEarnedLeaveCarriedForward;
                leave.CarryForwardLimit = policies[2].CarryForwardLimit;
                leave.TotalLeave = leave.CasualLeave + leave.SickLeave + leave.EarnedLeave;
            }

            return leave;
        }


    }
}
