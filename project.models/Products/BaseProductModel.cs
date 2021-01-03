using System.ComponentModel.DataAnnotations;

namespace project.models.Products
{
    public class BaseProductModel
    {
        [Required(ErrorMessage = "de productnaam moet ingevuld worden")]
        [Display(Name = "Productnaam")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "de stock moet ingevuld worden")]
        [Display(Name = "Stock")]
        [Range(0, 10000)]
        public int Stock { get; set; }

        [Required(ErrorMessage = "de omschrijving moet ingevuld worden")]
        [Display(Name = "Omschrijving")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "de prijs moet ingevuld worden")]
        [Display(Name = "Prijs")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
