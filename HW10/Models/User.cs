using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace HW10.Models
{
    public class User
    {
        public User()
        {
            Images = new List<Image>();
            UserSkills = new List<UserSkill>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime Birthday { get; set; }

		public virtual Image? Image { get; set; }
		public virtual ICollection<Image> Images { get; set; }
		public virtual Group Group { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
	}
}
