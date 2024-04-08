namespace HW10.Models
{
	public class UserSkill
	{
		public int Id { get; set; }
		public int Level { get; set; }	
		public virtual User? User { get; set; }
		public virtual Skill? Skill { get; set; }

	}
}
