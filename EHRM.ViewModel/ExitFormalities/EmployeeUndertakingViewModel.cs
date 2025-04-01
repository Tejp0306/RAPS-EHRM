using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.ExitFormalities
{
    public class EmployeeUndertakingViewModel
    {

        public int ID { get; set; }

        public string EmployeeName { get; set; }

        public string Relation { get; set; } // Son/Daughter

        public string FatherName { get; set; }

        public string PermanentAddress { get; set; }

        public string OfficeAddress { get; set; }

        public string LastWorkingDate { get; set; }

        public string ResignationDate { get; set; }

        public string EmployeeSignature { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
