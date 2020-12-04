using System;

namespace project.models.Addresses
{
    public class GetAddressModel : BaseAddressModel
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Company { get; set; }
    }
}
