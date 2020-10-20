using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.OrderProducts
{
    public class BaseOrderProductModel
    {
        [Range(0, 1000)]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
