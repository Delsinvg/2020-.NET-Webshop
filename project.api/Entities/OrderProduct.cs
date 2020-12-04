using System;
using System.ComponentModel.DataAnnotations;

namespace project.api.Entities
{
    public class OrderProduct
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
