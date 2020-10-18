using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Products
{
    public class PutProductModel : BaseProductModel
    {
        public Guid CategoryId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
