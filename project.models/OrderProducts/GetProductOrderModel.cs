using System;

namespace project.models.OrderProducts
{
    public class GetProductOrderModel : BaseOrderProductModel
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public string Order { get; set; }
    }
}
