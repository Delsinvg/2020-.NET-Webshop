using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Orders
{
    public class BaseOrderModel
    {
        [Required]
        public Guid UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }
    }
}
