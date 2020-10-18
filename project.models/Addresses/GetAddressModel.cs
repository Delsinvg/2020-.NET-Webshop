using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Addresses
{
   public class GetAddressModel : BaseAddressModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Company { get; set; }
    }
}
