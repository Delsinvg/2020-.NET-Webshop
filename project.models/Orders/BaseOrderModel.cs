using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Orders
{
    public class BaseOrderModel
    {
        [Display(Name = "Gebruikers id")]
        [Required]
        public Guid UserId { get; set; }

        [Display(Name = "Order datum")]
        [DataType(DataType.Date)]
        public DateTime Orderdate { get; set; }


    }
    
}
