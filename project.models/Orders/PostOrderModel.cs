using System.Collections.Generic;

namespace project.models.Orders
{
    public class PostOrderModel : BaseOrderModel
    {
        public ICollection<OrderProductModel> Products { get; set; }

    }
}
