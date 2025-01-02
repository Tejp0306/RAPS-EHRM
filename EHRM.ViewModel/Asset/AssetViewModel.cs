using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Asset
{
    public class AssetViewModel
    {
        public int Id { get; set; } // Primary Key
        public int TeamId { get; set; }
        public string? TeamName { get; set; }
        public int EmpId { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int Value { get; set; }
        public string? Status { get; set; }
        public string? Summary { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int DeletedBy { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }


    }
}
