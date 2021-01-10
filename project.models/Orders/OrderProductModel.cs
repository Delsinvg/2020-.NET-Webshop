using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Orders
{
    public class OrderProductModel
    {
        [Display(Name = "Product id")]
        public Guid ProductId { get; set; }
        [Display(Name = "Naam")]
        public String Name { get; set; }
        [Display(Name = "Hoeveelheid")]
        public int Quantity { get; set; }
        [Display(Name = "Prijs")]
        public Decimal Price { get; set; }
    }
}
