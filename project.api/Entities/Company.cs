using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(20, MinimumLength = 8)]
        public string AccountNumber { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        public Guid? AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
