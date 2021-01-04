using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class GetUserModel : BaseUserModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
