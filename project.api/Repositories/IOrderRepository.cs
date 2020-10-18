using project.models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IOrderRepository
    {
        Task<List<GetOrderModel>> GetOrders();
        Task<GetOrderModel> GetOrder(string id);
        Task<GetOrderModel> PostOrder(PostOrderModel postOrderModel);
        Task PutOrder(string id, PutOrderModel putOrderModel);
        Task DeleteOrder(string id);
    }
}
