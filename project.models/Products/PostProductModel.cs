using System;

namespace project.models.Products
{
    public class PostProductModel : BaseProductModel
    {
        public Guid CategoryId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
