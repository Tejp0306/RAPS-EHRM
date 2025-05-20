using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace EHRM.ViewModel.Master
{
    public class AddNoticeBoardViewModel
    {
        public int Id { get; set; }

        //public string? HeadingName { get; set; }

        //public string? Description { get; set; }

        //public IFormFile File { get; set; }
        public string? HeadingName { get; set; }
        public string? Description { get; set; }
        public string? ExpiryDate { get; set; }

        // Make sure that the File property is not required
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
        public string? FilePath { get; set; }
    }

}