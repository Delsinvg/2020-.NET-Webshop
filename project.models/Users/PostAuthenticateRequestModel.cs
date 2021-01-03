using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PostAuthenticateRequestModel
    {
        [Required(ErrorMessage = "De gebruikersnaam moet ingevuld worden")]
        [EmailAddress(ErrorMessage = "Dit veld moet een mailadres zijn")]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Het wachtwoord moet ingevuld worden")]
        [Display(Name = "Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
