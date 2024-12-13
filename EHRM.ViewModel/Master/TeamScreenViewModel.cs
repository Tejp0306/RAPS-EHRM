using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Master
{
    public class TeamScreenViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }

}