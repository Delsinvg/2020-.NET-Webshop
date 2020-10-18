using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Categories
{
    public class PutCategoryModel : BaseCategoryModel
    {
        public Guid ParentId { get; set; }
    }
}
