using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Master
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public int roleId { get; set; }
        public string? RoleName { get; set; }

        public string? RoleDescription { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string? DeletedBy { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

}
