using System;

namespace project.models.Categories
{
    public class GetCategoryModel : BaseCategoryModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentCategory { get; set; }
    }
}
