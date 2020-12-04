using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class BaseUserModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}

