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
                    throw new CollectionException("Remove all Orders first.", this.GetType().Name, "DeleteOrder", "400");
                }

                _context.OrderProducts.RemoveRange(order.OrderProducts);

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
                    Products = x.OrderProducts.Select(x => x.ProductId).ToList(),
                    Quantity = x.OrderProducts.Select(x => x.Quantity).ToList(),
                    Orderdate = x.Orderdate
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
                    Products = x.OrderProducts.Select(x => x.ProductId).ToList(),
                    Quantity = x.OrderProducts.Select(x => x.Quantity).ToList(),
                    Orderdate = x.Orderdate
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

                await AddOrderProducts(result.Entity.Id, postOrderModel.Products.ToList(), postOrderModel.Quantity.ToList());

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

        private async Task AddOrderProducts(Guid orderId, List<Guid> productIds, List<int> quantity)
        {
            for (int i = 0; i < quantity.Count ; i++)
            {
                await _context.OrderProducts.AddAsync(new OrderProduct
                {
                    OrderId = orderId,
                    ProductId = productIds[i],
                    Quantity = quantity[i]
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

                _context.OrderProducts.RemoveRange(order.OrderProducts);


                await AddOrderProducts(id, putOrderModel.Products.ToList(), putOrderModel.Quantity.ToList());
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
                    throw new EntityException("Genre not found.", this.GetType().Name, sourceMethod, "404");
                }
            }
        }
    }
}
