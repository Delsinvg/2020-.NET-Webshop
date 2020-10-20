using project.models.OrderProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IOrderProductRepository
    {
        Task<List<GetOrderProductModel>> GetOrderProducts();
        Task<GetOrderProductModel> GetOrderProduct(string id);
        Task<GetProductOrderModel> GetProductOrder(string id);
        Task<GetOrderProductModel> PostProduct(PostProductOrderModel postProductModel);
        Task DeleteOrder(string id);
    }
}
