using System.ComponentModel.DataAnnotations;

namespace EHRM.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
