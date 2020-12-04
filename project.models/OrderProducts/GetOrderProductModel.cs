using System;

namespace project.models.OrderProducts
{
    public class GetOrderProductModel : BaseOrderProductModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Product { get; set; }
    }
}
