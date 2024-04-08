using Microsoft.AspNetCore.Identity;

namespace HW10.Models
{
	public class UserIdentity:IdentityUser<int>
	{
		public virtual Image? Image { get; set; }
		public virtual MapMarker? MapMarker { get; set; }
	}
}
