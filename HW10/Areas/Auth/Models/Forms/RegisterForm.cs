using System.ComponentModel.DataAnnotations;

namespace HW10.Areas.Auth.Models.Forms
{
	public class RegisterForm
	{
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Input login!")]
        [EmailAddress(ErrorMessage = "Login is email!")]
        public string Login { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Input password!")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Input confirm password!")]
        public string ConfirmPassword { get; set; }
    }
}
