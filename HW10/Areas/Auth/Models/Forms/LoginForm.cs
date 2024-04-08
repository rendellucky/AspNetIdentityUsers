using System.ComponentModel.DataAnnotations;

namespace HW10.Areas.Auth.Models.Forms
{
    public class LoginForm
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "LOGIN!")]
        [EmailAddress(ErrorMessage = "Login is email!")]
        public string Login { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "PASSWORD!")]
        public string Password { get; set; }
    }
}
