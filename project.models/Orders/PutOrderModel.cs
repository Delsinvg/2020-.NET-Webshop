using System;
using System.Collections.Generic;

namespace project.models.Orders
{
    public class PutOrderModel : BaseOrderModel
    {
        public ICollection<Guid> Products { get; set; }
        public ICollection<int> Quantity { get; set; }
    }
}
