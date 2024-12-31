using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.SubMenu
{
    public class SubMenuViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Controller { get; set; }

        public string? Action { get; set; }

        public int? MainMenuId { get; set; }

        public int? RoleId { get; set; }

        public int? EmpId { get; set; }

        public bool? IsActive { get; set; }

  
    }
}
