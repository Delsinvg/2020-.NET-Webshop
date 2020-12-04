using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Orders
{
    public class BaseOrderModel
    {
        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }
    }
}
