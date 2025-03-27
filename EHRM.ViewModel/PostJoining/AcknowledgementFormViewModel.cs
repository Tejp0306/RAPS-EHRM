using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.PostJoining
{
    public class AcknowledgementFormViewModel
    {

        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string? EmployeeSignature { get; set; } // Stores the file path of the uploaded signature

        public string SignatureDate { get; set; } // Stored as varchar(255) in the database

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

