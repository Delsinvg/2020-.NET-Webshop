using System;

namespace project.models.Orders
{
    public class OrderProductModel
    {
        public Guid ProductId { get; set; }
        public String Name { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
    }
}
