using project.models.Addresses;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Categories
{
    public class GetCategoryModel : BaseCategoryModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentCategory { get; set; }
    }
}
