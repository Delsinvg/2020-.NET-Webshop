using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Companies
{
    public class GetCompanyModel : BaseCompanyModel
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public string Address { get; set; }
    }
}
