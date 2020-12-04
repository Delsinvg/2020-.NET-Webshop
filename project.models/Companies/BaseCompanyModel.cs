using System.ComponentModel.DataAnnotations;

namespace project.models.Companies
{
    public class BaseCompanyModel
    {
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(20, MinimumLength = 8)]
        public string AccountNumber { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
