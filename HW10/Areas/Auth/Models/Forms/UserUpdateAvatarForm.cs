using System.ComponentModel.DataAnnotations;

namespace HW10.Areas.Auth.Models.Forms
{
    public class UserUpdateAvatarForm
    {
		[Display(Name = "Image")]
		public IFormFile? Image { get; set; }
	}
}
