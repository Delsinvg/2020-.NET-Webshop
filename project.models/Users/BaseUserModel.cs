using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class BaseUserModel
    {
        [Required(ErrorMessage = "de voornaam moet ingevuld worden")]
        [Display(Name = "Voornaam")]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "de achternaam moet ingevuld worden")]
        [Display(Name = "Achternaam")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Dit veld moet een mailadres zijn")]
        [Required(ErrorMessage = "Het emailadres moet ingevuld worden")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

