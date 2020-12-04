using System;
using System.Collections.Generic;

namespace project.models.Orders
{
    public class GetOrderModel : BaseOrderModel
    {
        public Guid Id { get; set; }

        public string User { get; set; }
        public ICollection<string> Products { get; set; }
    }
}
