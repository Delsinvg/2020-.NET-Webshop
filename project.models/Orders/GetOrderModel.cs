using System;
using System.Collections.Generic;

namespace project.models.Orders
{
    public class GetOrderModel : BaseOrderModel
    {
        public Guid Id { get; set; }
        public ICollection<OrderProductModel> Products { get; set; }
    }
}
