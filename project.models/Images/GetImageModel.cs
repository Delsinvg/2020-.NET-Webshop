using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Images
{
    public class GetImageModel : BaseImageModel
    {
        public Guid Id { get; set; }

        public string Product { get; set; }
    }
}
