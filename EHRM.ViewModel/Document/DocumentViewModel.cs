using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ViewModel.Document
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "File Path")]
        public string FilePath { get; set; } = null!;

        [Display(Name = "Uploaded At")]
        public DateTime? UploadedAt { get; set; }

        public IFormFile? File { get; set; } // For handling file uploads
    }
}
