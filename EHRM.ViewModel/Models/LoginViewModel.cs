using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EHRM.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string ExistingLogoPath { get; set; }

        public IFormFile FaviconFile { get; set; }
        public string ExistingFaviconPath { get; set; }
        public string Bio { get; set; }
        public string OrganizationName { get; set; }


    }
}
