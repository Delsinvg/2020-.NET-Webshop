using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.OrderProducts
{
    public class PostProductOrderModel : BaseOrderProductModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
    }
}
