using System;

namespace project.models.Products
{
    public class GetProductModel : BaseProductModel
    {
        public Guid Id { get; set; }

        public string Category { get; set; }
        public string Company { get; set; }
    }
}
