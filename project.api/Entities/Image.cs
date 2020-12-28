using System;
using System.ComponentModel.DataAnnotations;

namespace project.api.Entities
{
    public class Image
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FileType { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 4)]
        public string Extension { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public Guid? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
