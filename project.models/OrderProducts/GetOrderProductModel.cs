using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.OrderProducts
{
    class GetOrderProductModel : BaseOrderProductModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Product { get; set; }
    }
}
