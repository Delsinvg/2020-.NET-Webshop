using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Categories
{
    public class PostCategoryModel : BaseCategoryModel
    {
        public Guid ParentId { get; set; }
    }
}
