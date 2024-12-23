using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Proc
{
    public class SubMenuProcDetails
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int MainMenuId { get; set; }
        public int RoleId { get; set; }
        public int EmployeeId { get; set; }
        public string SubMenuName { get; set; }
        public string MainMenuName { get; set; }
        public string RoleName { get; set; }
        public string EmployeeName { get; set; }
    }
}
