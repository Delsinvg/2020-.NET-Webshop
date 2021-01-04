using System;
using System.ComponentModel.DataAnnotations;

namespace project.models.Categories
{
    public class GetCategoryModel : BaseCategoryModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        [Display(Name = "Bovenstaande categorie")]
        public string ParentCategory { get; set; }
    }
}
