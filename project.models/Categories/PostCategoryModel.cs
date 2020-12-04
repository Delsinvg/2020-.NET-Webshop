using System;

namespace project.models.Categories
{
    public class PostCategoryModel : BaseCategoryModel
    {
        public Guid ParentId { get; set; }
    }
}
