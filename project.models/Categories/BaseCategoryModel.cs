using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Categories
{
    public class BaseCategoryModel
    {
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
