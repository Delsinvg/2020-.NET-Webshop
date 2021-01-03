using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Addresses
{
    public class GetAddressModel : BaseAddressModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Gebruiker")]
        public string User { get; set; }

        [Display(Name = "Bedrijf")]
        public string Company { get; set; }
    }
}
