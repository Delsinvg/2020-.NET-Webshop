using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PostAuthenticateRequestModel
    {
        [Required]

        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
