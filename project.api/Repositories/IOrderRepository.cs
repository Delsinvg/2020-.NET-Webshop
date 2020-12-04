using project.models.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IOrderRepository
    {
        Task<List<GetOrderModel>> GetOrders();
        Task<GetOrderModel> GetOrder(Guid id);
        Task<GetOrderModel> PostOrder(PostOrderModel postOrderModel);
        Task PutOrder(Guid id, PutOrderModel putOrderModel);
        Task DeleteOrder(Guid id);
    }
}
