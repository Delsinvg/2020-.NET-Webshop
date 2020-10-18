using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Orders
{
    public class PutOrderModel : BaseOrderModel
    {
        public Guid UserId { get; set; }
        public ICollection<Guid> Products { get; set; }
    }
}
