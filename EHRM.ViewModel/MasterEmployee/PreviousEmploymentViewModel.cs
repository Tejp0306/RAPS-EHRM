using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHRM.ViewModel.MasterEmployee
{

    public class PreviousEmploymentViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public string NatureOfEmployment { get; set; }
        public string CurrentDesignation { get; set; }
        public string Department { get; set; }
        public string OfficialTitle { get; set; }
        public string PayrollCompanyName { get; set; }
        public string OrganizationAddress { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string EmployeeCode { get; set; }
        public decimal CTCPerAnnum { get; set; }
        public string KeyResponsibility { get; set; }
        public string EmploymentTenure { get; set; }
        public string ReasonForLeaving { get; set; }
        public string ReportingManagerName { get; set; }
        public string ReportingManagerDesignation { get; set; }
        public string IsReportingManagerStillInCompany { get; set; }
        public string CompanyLandline { get; set; }
        public string PersonalMobileNo { get; set; }
        public string BestTimeToReach { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class EmploymentViewModel
    {
        
        public List<PreviousEmploymentViewModel> PreviousEmployments { get; set; }
    }
}