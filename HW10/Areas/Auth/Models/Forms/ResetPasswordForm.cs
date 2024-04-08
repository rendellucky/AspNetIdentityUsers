using System.ComponentModel.DataAnnotations;

namespace HW10.Areas.Auth.Models.Forms
{
	public class ResetPasswordForm
	{
        [Display(Name = "New password")]
        [Required(ErrorMessage = "Input new password!")]
        public string Password { get; set; }

        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Input confirm new password!")]
        public string ConfirmPassword { get; set; }
    }
}
