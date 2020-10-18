using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace project.models.Images
{
    public class BaseImageModel
    {
        [StringLength(50)]
        public string Path { get; set; }
    }
}
