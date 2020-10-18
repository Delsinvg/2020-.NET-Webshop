using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }
}
