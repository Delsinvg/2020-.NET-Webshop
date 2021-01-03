using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Categories
{
    public class PostCategoryModel : BaseCategoryModel
    {
        [Required(ErrorMessage = "De parent moet ingevuld worden")]
        public Guid ParentId { get; set; }
    }
}
