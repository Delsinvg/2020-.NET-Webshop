using System;
using System.ComponentModel.DataAnnotations;

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
