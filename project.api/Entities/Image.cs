using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Path { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
