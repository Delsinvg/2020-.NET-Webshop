using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Users
{
    public class EmailModel
    {
        [EmailAddress(ErrorMessage = "Dit veld moet een mailadres zijn")]
        [Required(ErrorMessage = "Het emailadres moet ingevuld worden")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
