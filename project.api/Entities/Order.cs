using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.api.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
