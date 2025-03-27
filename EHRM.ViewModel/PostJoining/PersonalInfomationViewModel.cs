using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.PostJoining
{
    public class PersonalInfomationViewModel
    {
        
        public int ID { get; set; }

       
        public string EmployeeName { get; set; }

       
        public string PersonalEmail { get; set; }

       
        public string PermanentAddress { get; set; }

       
        public string CurrentAddress { get; set; }

        
        public string? HomePhone { get; set; }

        
        public string MobilePhone { get; set; }

        // Emergency Contact 1
       
        public string EmergencyContact1Name { get; set; }

       
        public string EmergencyContact1Relationship { get; set; }

        public string EmergencyContact1Phone { get; set; }

        // Emergency Contact 2 (Optional)
       
        public string? EmergencyContact2Name { get; set; }

      
        public string? EmergencyContact2Relationship { get; set; }

       
        public string? EmergencyContact2Phone { get; set; }

     
        public string Signature { get; set; }

        public string FormDate { get; set; }
    }
}
