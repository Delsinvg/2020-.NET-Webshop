using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Users
{
    public class PostAuthenticateRequestModel
    {
        [Required]
        [RegularExpression(@"^\w+[\.]\w+(@svsl\.be)$", ErrorMessage = "Invalid @test.be email address.")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
