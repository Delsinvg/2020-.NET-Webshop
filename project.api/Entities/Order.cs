using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
