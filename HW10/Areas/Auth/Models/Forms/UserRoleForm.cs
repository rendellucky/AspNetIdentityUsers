using System.ComponentModel.DataAnnotations;

namespace HW10.Areas.Auth.Models.Forms
{
    public class UserRoleForm
    {
        public bool Active { get; set; }
        public string? Name { get; set; }
    }
}
