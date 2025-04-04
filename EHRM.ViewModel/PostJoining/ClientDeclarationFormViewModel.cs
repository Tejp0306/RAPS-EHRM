using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.PostJoining
{
    public class ClientPropertyDeclarationViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }

        public string EmployeeName { get; set; }

        public string ClientName { get; set; }

        public string ReceivedDate { get; set; }

        public string ItemsReceived { get; set; }

        public string EmployeeNameConfirm { get; set; }

        public string Signature { get; set; }

        public string ConfirmationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
