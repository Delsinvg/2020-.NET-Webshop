using System;

namespace project.models.OrderProducts
{
    public class PostProductOrderModel : BaseOrderProductModel
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
    }
}
