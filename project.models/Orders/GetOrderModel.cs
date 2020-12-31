using System;
using System.Collections.Generic;

namespace project.models.Orders
{
    public class GetOrderModel : BaseOrderModel
    {
        public Guid Id { get; set; }
        public ICollection<Guid> Products { get; set; }
        public ICollection<int> Quantity { get; set; }
        public ICollection<Decimal> Price { get; set; }
    }
}
