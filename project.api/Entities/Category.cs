using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project.api.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public Category ParentCategory { get; set; }
        public Guid? ParentId { get; set; }
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Category> SubCategories { get; set; }

    }
}
