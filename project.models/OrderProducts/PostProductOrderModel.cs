using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.OrderProducts
{
    class PostProductOrderModel : BaseOrderProductModel
    {
        public Guid OrderId { get; set; }
    }
}
