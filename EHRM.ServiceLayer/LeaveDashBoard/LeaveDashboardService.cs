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
            // Fetch leave policies from repository
            var policyRepository = _unitOfWork.GetRepository<LeavePolicy>();
            List<LeavePolicy> policies = (await policyRepository.GetAllAsync()).ToList();

            // Calculate years of service in decimal
            double yearsOfService = (DateTime.Now - startDate).TotalDays / 365.25;

            // Initialize leave summary
            var leave = new LeaveSummaryViewModel();

            // Check if employee is in probation period
            var probationPolicy = policies.FirstOrDefault(p => p.IsProbation == true
                                                               && yearsOfService >= (double)p.YearsOfServiceMin
                                                               && yearsOfService < (double)p.YearsOfServiceMax);
            if (probationPolicy != null)
            {
                // Apply probation leave policy
                leave.CasualLeave = probationPolicy.CasualLeave;
                leave.SickLeave = probationPolicy.SickLeave;
                leave.EarnedLeave = probationPolicy.EarnedLeave;
                leave.EarnedLeaveAccrualRate = probationPolicy.EarnedLeaveAccrualRate;
                leave.CarryForwardEL = probationPolicy.IsEarnedLeaveCarriedForward;
                leave.CarryForwardLimit = probationPolicy.CarryForwardLimit;
                leave.TotalLeave = leave.CasualLeave + leave.SickLeave + leave.EarnedLeave;
                leave.Tenure = (decimal)yearsOfService;
                return leave;
            }

            // Standard leave policies based on years of service
            var applicablePolicy = policies.FirstOrDefault(p => yearsOfService >= (double)p.YearsOfServiceMin
                                                                 && yearsOfService < (double)p.YearsOfServiceMax
                                                                 && (p.IsProbation == false || p.IsProbation == null));
            if (applicablePolicy != null)
            {
                leave.CasualLeave = applicablePolicy.CasualLeave;
                leave.SickLeave = applicablePolicy.SickLeave;
                leave.EarnedLeave = applicablePolicy.EarnedLeave;
                leave.EarnedLeaveAccrualRate = applicablePolicy.EarnedLeaveAccrualRate;
                leave.CarryForwardEL = applicablePolicy.IsEarnedLeaveCarriedForward;
                leave.CarryForwardLimit = applicablePolicy.CarryForwardLimit;
                leave.TotalLeave = leave.CasualLeave + leave.SickLeave + leave.EarnedLeave;
                leave.Tenure = (int)yearsOfService;
            }

            return leave;
        }



    }
}
