using System.ComponentModel.DataAnnotations;

namespace HW10.Areas.Auth.Models.Forms
{
    public class UserUpdateForm
    {
        public List<UserRoleForm> Roles { get; set; }
		[Display(Name = "MarkerId")]
		public int MapMarkerId { get; set; }
	}
}
