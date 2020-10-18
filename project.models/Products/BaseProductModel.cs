using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Products
{
    public class BaseProductModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        [Range(0, 10000)]
        public int Stock { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
