using Microsoft.EntityFrameworkCore;
using project.api.Entities;
using project.api.Exceptions;
using project.models.OrderProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class OrderProductRepository : IOrderProductRepository
    {
        private readonly ProjectContext _context;

        public OrderProductRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task DeleteOrderProduct(string id)
        {
            try
            {
                OrderProduct orderProduct = await _context.OrderProducts

                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (orderProduct == null)
                {
                    throw new EntityException("OrderProduct not found.", this.GetType().Name, "DeleteOrderproduct", "404");
                }

                _context.OrderProducts.Remove(orderProduct);

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "DeleteOrderProduct", "400");
            }
        }

        public async Task<GetOrderProductModel> GetOrderProduct(string id)
        {
            try
            {
                GetOrderProductModel orderProduct = await _context.OrderProducts
                    .Select(x => new GetOrderProductModel
                    {
                        Id = x.Id,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        Product = x.Product.Name,
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return orderProduct;
            }
            catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetOrderProduct", "404");
            }
        }

        public async Task<List<GetOrderProductModel>> GetOrderProducts()
        {
            try
            {
                List<GetOrderProductModel> orderProducts = await _context.OrderProducts
                    .Select(x => new GetOrderProductModel
                    {
                        Id = x.Id,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        Product = x.Product.Name,
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return orderProducts;
            }
            catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetImage", "404");
            }
        }

        public Task<GetProductOrderModel> GetProductOrder(string id)
        {
            throw new NotImplementedException();
        }

        public Task PostProductOrder(PostProductOrderModel postProductModel)
        {
            throw new NotImplementedException();
        }
    }
}
