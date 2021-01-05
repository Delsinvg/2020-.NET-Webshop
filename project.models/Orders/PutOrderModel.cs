using System.Collections.Generic;

namespace project.models.Orders
{
    public class PutOrderModel : BaseOrderModel
    {
        public ICollection<OrderProductModel> Products { get; set; }
    }
}
