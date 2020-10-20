using project.models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IProductsRepository
    {
        Task<List<GetProductModel>> GetProducts();
        Task<GetProductModel> GetProduct(Guid id);
        Task<GetProductModel> PostProduct(PostProductModel postProductModel);
        Task PutProduct(Guid id, PutProductModel putProductModel);
        Task DeleteProduct(Guid id);
    }
}
