using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PostUserModel : BaseUserModel
    {
        [Required]
        public string Password { get; set; }
        public ICollection<string> Roles { get; set; }
        public Guid AddressId { get; set; }
    }
}
