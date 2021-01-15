using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ProjectContext _context;

        public OrderRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task DeleteOrder(Guid id)
        {
            try
            {
                Order order = await _context.Orders
                    .Include(x => x.OrderProducts)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (order == null)
                {
                    throw new EntityException("Order not found.", this.GetType().Name, "DeleteOrder", "404");
                }

                if (order.OrderProducts.Count > 0)
                {
                    _context.OrderProducts.RemoveRange(order.OrderProducts);
                }

                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "DeleteOrder", "400");
            }
        }

        public async Task<GetOrderModel> GetOrder(Guid id)
        {

            GetOrderModel order = await _context.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .Select(x => new GetOrderModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Orderdate = x.Orderdate,
                    totalPrice = x.OrderProducts.Sum(x => x.Price),
                    Products = x.OrderProducts.Select(x => new OrderProductModel { Name = x.Product.Name, ProductId = x.ProductId, Quantity = x.Quantity, Price = x.Price }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                throw new EntityException("Order not found.", this.GetType().Name, "GetOrder", "404");
            }

            return order;
        }

        public async Task<List<GetOrderModel>> GetOrders()
        {

            List<GetOrderModel> orders = await _context.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .Include(x => x.User)
                .Select(x => new GetOrderModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Orderdate = x.Orderdate,
                    totalPrice = x.OrderProducts.Sum(x => x.Price * x.Quantity),
                    Products = x.OrderProducts.Select(x => new OrderProductModel { Name = x.Product.Name, ProductId = x.ProductId, Quantity = x.Quantity, Price = x.Price }).ToList()

                })
                .AsNoTracking()
                .ToListAsync();

            if (orders.Count == 0)
            {
                throw new CollectionException("No orders found.", this.GetType().Name, "GetOrders", "404");
            }

            return orders;

        }

        public async Task<GetOrderModel> PostOrder(PostOrderModel postOrderModel)
        {
            try
            {
                await CheckUser(postOrderModel.UserId, "PostOrder");

                EntityEntry<Order> result = await _context.Orders.AddAsync(new Order
                {
                    Orderdate = postOrderModel.Orderdate,
                    UserId = postOrderModel.UserId,
                });

                if (postOrderModel.Products != null)
                {
                    await AddOrderProducts(result.Entity.Id, postOrderModel.Products.ToList());
                }

                await _context.SaveChangesAsync();

                return await GetOrder(result.Entity.Id);
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PostOrder", "400");
            }
        }

        private async Task AddOrderProducts(Guid orderId, List<OrderProductModel> products)
        {

            foreach (var item in products)
            {
                await _context.OrderProducts.AddAsync(new OrderProduct
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });

            }
            await _context.SaveChangesAsync();
        }

        public async Task PutOrder(Guid id, PutOrderModel putOrderModel)
        {
            try
            {
                Order order = await _context.Orders
                    .Include(x => x.OrderProducts)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (order == null)
                {
                    throw new EntityException("Order not found.", this.GetType().Name, "PutOrder", "404");
                }

                await CheckUser(putOrderModel.UserId, "PutOrder");

                order.Orderdate = putOrderModel.Orderdate;
                order.UserId = putOrderModel.UserId;

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PutOrder", "400");
            }
        }

        private async Task CheckUser(Guid? userId, string sourceMethod)
        {
            if (userId != null)
            {
                User user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    throw new EntityException("User not found.", this.GetType().Name, sourceMethod, "404");
                }
            }
        }
    }
}
