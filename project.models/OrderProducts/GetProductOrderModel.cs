using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.OrderProducts
{
    class GetProductOrderModel : BaseOrderProductModel
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public string Order { get; set; }
    }
}
