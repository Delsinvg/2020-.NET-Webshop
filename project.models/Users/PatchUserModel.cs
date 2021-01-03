using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PatchUserModel
    {
        [Required(ErrorMessage = "het oude wachtwoord moet ingevuld worden")]
        [Display(Name = "Oud wachtwoord")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "het nieuwe wachtwoord moet ingevuld worden")]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }
    }
}
