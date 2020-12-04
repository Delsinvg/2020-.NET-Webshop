using System;
using System.Collections.Generic;

namespace project.models.Users
{
    public class GetUserModel : BaseUserModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
