using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.api.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        public Guid? AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }

}
