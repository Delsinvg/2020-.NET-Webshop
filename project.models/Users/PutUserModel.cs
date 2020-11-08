using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Users
{
    public class PutUserModel : BaseUserModel
    {
        public ICollection<string> Roles { get; set; }
        public Guid AddressId { get; set; }
    }
}
