using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PatchUserModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
