using System.ComponentModel.DataAnnotations;

namespace project.models.Categories
{
    public class BaseCategoryModel
    {
        [Display(Name = "Categorienaam")]
        [StringLength(30, MinimumLength = 2)]
        [Required(ErrorMessage = "De naam moet ingevuld worden")]
        public string Name { get; set; }
    }
}
