using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Addresses
{
   public class BaseAddressModel
    {
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
