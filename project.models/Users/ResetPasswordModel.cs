﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Users
{
    public class ResetPasswordModel
    {
        [Display(Name = "Gebruikers id")]
        public Guid UserId { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Wachtwoord")]
        [Required(ErrorMessage = "Het wachtwoord moet ingevuld worden")]
        public string Password { get; set; }
    }
}
