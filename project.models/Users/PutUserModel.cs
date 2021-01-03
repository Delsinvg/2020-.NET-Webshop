using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PutUserModel : BaseUserModel
    {
        [Display(Name = "Rollen")]
        public ICollection<string> Roles { get; set; }

        [Display(Name = "Adres id")]
        public Guid AddressId { get; set; }
    }
}
