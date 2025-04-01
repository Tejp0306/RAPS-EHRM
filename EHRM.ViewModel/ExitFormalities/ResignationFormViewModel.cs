using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.ExitFormalities
{
    public class ResignationFormViewModel
    {

        public int Id { get; set; }

        public string EmployeeSignature { get; set; }

        public string EmployeeName { get; set; }

        public string Position { get; set; }

        public string FinalDay { get; set; }

        public string TotalMonths { get; set; }

        public string ResignationDate { get; set; }
    }
}
