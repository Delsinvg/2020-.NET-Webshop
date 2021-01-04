using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace project.models.Users
{
    public class PatchUserModel
    {
        [Required(ErrorMessage = "Het oude wachtwoord moet ingevuld worden")]
        [Display(Name = "Oud wachtwoord")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Het nieuwe wachtwoord moet ingevuld worden")]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [JsonIgnore]
        [Display(Name = "Herhaal nieuw wachtwoord")]
        public string ConfirmNewPassword { get; set; }
    }
}
