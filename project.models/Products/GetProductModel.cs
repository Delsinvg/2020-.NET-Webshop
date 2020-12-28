﻿using project.models.Images;
using System;
using System.Collections.Generic;

namespace project.models.Products
{
    public class GetProductModel : BaseProductModel
    {
        public Guid Id { get; set; }

        public string Category { get; set; }
        public string Company { get; set; }
        public ICollection<ImageModel> ImagesModel { get; set; }
    }
}
