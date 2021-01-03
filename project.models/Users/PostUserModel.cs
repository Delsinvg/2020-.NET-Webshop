using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PostUserModel : BaseUserModel
    {
        [Required(ErrorMessage = "Het wachtwoord moet ingevuld worden")]
        [Display(Name = "Wachtwoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Rollen")]
        public ICollection<string> Roles { get; set; }


        [Required(ErrorMessage = "De landcode moet ingevuld worden")]
        [Display(Name = "Landcode")]
        [StringLength(4, MinimumLength = 2)]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Het land moet ingevuld worden")]
        [Display(Name = "Land")]
        [StringLength(30, MinimumLength = 2)]
        public string Country { get; set; }

        [Required(ErrorMessage = "De stad moet ingevuld worden")]
        [Display(Name = "Stad")]
        [StringLength(30, MinimumLength = 2)]
        public string City { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "De postcode moet ingevuld worden")]
        [RegularExpression(@"^\d+$")]
        [Range(1, 10000, ErrorMessage = "De postcode is minimum 1 en maximum 10000")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "De straat moet ingevuld worden")]
        [Display(Name = "Straat")]
        [StringLength(30, MinimumLength = 2)]
        public string Street { get; set; }
    }
}
