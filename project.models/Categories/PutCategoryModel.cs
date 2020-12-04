using System;

namespace project.models.Categories
{
    public class PutCategoryModel : BaseCategoryModel
    {
        public Guid ParentId { get; set; }
    }
}
