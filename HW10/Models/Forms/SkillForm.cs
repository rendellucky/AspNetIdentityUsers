using System.ComponentModel.DataAnnotations;

namespace HW10.Models.Forms
{
    public class SkillForm
    {
        public SkillForm()
        {

        }
        private Skill? _skill;

        public string? ImageSrc => _skill?.Image?.Src;
        public SkillForm(Skill skill)
        {
            _skill = skill;
            Name = skill.Name;
        }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is necessarily")]
        [MaxLength(20, ErrorMessage = "Up to 20 characters")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public IFormFile? Image { get; set; }

    }
}
