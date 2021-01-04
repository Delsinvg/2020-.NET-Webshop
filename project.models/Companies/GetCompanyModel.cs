using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Companies
{
    public class GetCompanyModel : BaseCompanyModel
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        [Display(Name = "Adres")]
        public string Address { get; set; }
    }
}
