using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Users
{
    public class ResetPasswordModel
    {
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
