using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.models.Users
{
    public class PostUserModel : BaseUserModel
    {
        [Required]
        public string Password { get; set; }
        public ICollection<string> Roles { get; set; }
        [StringLength(4, MinimumLength = 2)]


        public string CountryCode { get; set; }
        [StringLength(30, MinimumLength = 2)]
        public string Country { get; set; }
        [StringLength(30, MinimumLength = 2)]
        public string City { get; set; }
        [RegularExpression(@"^\d+$")]
        [Range(0, 10000)]
        public int PostalCode { get; set; }
        [StringLength(30, MinimumLength = 2)]
        public string Street { get; set; }
    }
}
