using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Master
{
    public class HolidayViewModel
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string? DeletedBy { get; set; }

        public string? CreateDate { get; set; }

        public string? HolidayDate { get; set; }

        public string? TeamName { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        //public string SelectedTeam { get; set; }
      
    }
}
