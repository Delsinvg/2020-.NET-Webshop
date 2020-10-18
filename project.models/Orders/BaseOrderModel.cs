using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Orders
{
   public class BaseOrderModel
    {
        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
