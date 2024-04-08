using System.ComponentModel.DataAnnotations;

namespace HW10.Models.Forms
{
	public class UserSkillForm
	{
		public UserSkillForm()
		{

		}
		private readonly UserSkill? _userSkill;
		public string? ImageSrc => _userSkill?.Skill?.Image?.Src;
		public UserSkillForm(UserSkill userSkill)
		{
			_userSkill = userSkill;
			Level = userSkill.Level;
			SkillId = userSkill.Skill.Id;
		}
		public int? Id => _userSkill?.Id;

		[Display(Name = "Level")]
		public int Level { get; set; }

		[Display(Name = "Skill")]
		public int SkillId { get; set; }
	}
}
