using System.ComponentModel.DataAnnotations;

namespace project.models.Categories
{
    public class BaseCategoryModel
    {
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
