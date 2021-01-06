using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.models.Orders
{
    public class GetOrderModel : BaseOrderModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Producten")]
        public ICollection<OrderProductModel> Products { get; set; }
        [Display(Name = "Totale prijs")]
        public Decimal totalPrice { get; set; }
    }
}
