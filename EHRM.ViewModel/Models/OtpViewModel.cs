using System.ComponentModel.DataAnnotations;

namespace EHRM.Web.Models
{
    public class OtpViewModel
    {
        // The OTP entered by the user
        [Required(ErrorMessage = "OTP is required.")]
        [StringLength(6, ErrorMessage = "OTP must be 6 digits long.", MinimumLength = 6)]
        [Display(Name = "Enter OTP")]
        public string Otp { get; set; }

        // The email of the user (optional, depending on your flow)
        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email format.")]
        //[Display(Name = "Email")]
        //public string Email { get; set; }
    }
}
