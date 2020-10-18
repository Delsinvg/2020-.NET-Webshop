using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        [Range(0, 10000)]
        public int Stock { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
