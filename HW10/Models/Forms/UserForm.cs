using System.ComponentModel.DataAnnotations;

namespace HW10.Models.Forms
{
    public class UserForm
    {
        public UserForm()
        {

        }
        private User? _user;

        public string? ImageSrc => _user?.Image?.Src;
        public List<string>? ImagesSrc => _user?.Images.Select(x => x.Src).ToList();
        public UserForm(User user)
        {
            _user = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Description = user.Description;
            Birthday = user.Birthday;
            IsActive = user.IsActive;
            GroupId = user.Group.Id;
            //Skills = user.UserSkills.Where(x => x.User.Id == user.Id).ToList();
        }
        [Display(Name = "First name")]
        [Required(ErrorMessage = "First name is necessarily")]
        [MaxLength(20, ErrorMessage = "Up to 20 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Last name is necessarily")]
        [MaxLength(20, ErrorMessage = "Up to 20 characters")]
        public string LastName { get; set; }

        [Display(Name = "Description")]
        [MaxLength(200, ErrorMessage = "Up to 200 characters")]
        [Required(ErrorMessage = "Description is necessarily")]
        public string Description { get; set; }

        [Display(Name = "Birth day")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Activity")]
        public bool IsActive { get; set; }

        [Display(Name = "Group")]
        public int GroupId { get; set; }

        //[Display(Name = "Skills")]
        //public List<UserSkill> Skills { get; set; }

        [Display(Name = "Image")]
        public IFormFile? Image { get; set; }

        [Display(Name = "Additional Images")]
        public List<IFormFile>? Images { get; set; }
    }
}
