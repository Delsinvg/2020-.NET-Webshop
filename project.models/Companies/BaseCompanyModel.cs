using System.ComponentModel.DataAnnotations;

namespace project.models.Companies
{
    public class BaseCompanyModel
    {
        [Required(ErrorMessage = "De naam moet ingevuld worden")]
        [Display(Name = "Bedrijfsnaam")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Het emailadres moet ingevuld worden")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Dit veld moet een mailadres zijn")]
        public string Email { get; set; }

        [Required(ErrorMessage = "De naam moet ingevuld worden")]
        [Display(Name = "Rekeningnummer")]
        [StringLength(20, MinimumLength = 8)]
        public string AccountNumber { get; set; }

        [Display(Name = "Telefoonnummer")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
