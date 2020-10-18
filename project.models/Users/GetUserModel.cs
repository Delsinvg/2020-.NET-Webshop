using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Users
{
    public class GetUserModel : BaseUserModel
    {
        public Guid Id { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
