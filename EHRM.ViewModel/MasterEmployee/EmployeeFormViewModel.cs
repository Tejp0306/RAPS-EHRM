using System.Net;

public class EmployeeFormViewModel
{
    public MasterEmployee MasterEmployee { get; set; }
    public MasterContactDetails MasterContactDetails { get; set; }
    public MasterAddress MasterAddress { get; set; }
    public MasterEducation MasterEducation { get; set; }
    public List<MasterWorkExperience> MasterWorkExperience { get; set; } = new List<MasterWorkExperience>();
    public MasterBankDetails MasterBankDetails { get; set; }
    public MasterEmergencyContactViewModel MasterEmergencyContactViewModel { get; set; }
    public MasterReportingDetails MasterReportingDetails { get; set; }
    public MasterFamilyDetails MasterFamilyDetails { get; set; }
    public MasterDependentDetails MasterDependentDetails { get; set; }



}
