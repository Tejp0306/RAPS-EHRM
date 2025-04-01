using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.PostJoining
{
    public class NDAFormViewModel
    {
        public int Id { get; set; }


        public string EmployeeName { get; set; }


        public string AgreementDate { get; set; }

        public string Signature { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
