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

        public async Task DeleteOrder(string id)
        {
            try
            {
                Order order = await _context.Orders
                    .Include(x => x.OrderProducts)
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (order == null)
                {
                    throw new KeyNotFoundException();
                }

                _context.OrderProducts.RemoveRange(order.OrderProducts);

                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();
            } catch(Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteOrder");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "DeleteOrder");
                }
            }
        }

        public async Task<GetOrderModel> GetOrder(string id)
        {
            try
            {
                GetOrderModel order = await _context.Orders
                    .Include(x => x.OrderProducts)
                    .Include(x => x.User)
                    .Select(x => new GetOrderModel
                    {
                        Id = x.Id,
                        Orderdate = x.Orderdate,
                        PhoneNumber = x.PhoneNumber,
                        User = $"{x.User.FirstName} {x.User.LastName}",
                        Products = x.OrderProducts.Select(x => x.Product.Name).ToList(),
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return order;
            } catch(Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetOrder");
            }
        }

        public async Task<List<GetOrderModel>> GetOrders()
        {
            try
            {
                List<GetOrderModel> orders = await _context.Orders
                    .Include(x => x.OrderProducts)
                    .Include(x => x.User)
                    .Select(x => new GetOrderModel
                    {
                        Id = x.Id,
                        Orderdate = x.Orderdate,
                        PhoneNumber = x.PhoneNumber,
                        User = $"{x.User.FirstName} {x.User.LastName}",
                        Products = x.OrderProducts.Select(x => x.Product.Name).ToList(),
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return orders;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetOrders");
            }
        }

        public async Task<GetOrderModel> PostOrder(PostOrderModel postOrderModel)
        {
            EntityEntry<Order> result = await _context.Orders.AddAsync(new Order
            {
                Orderdate = postOrderModel.Orderdate,
                PhoneNumber = postOrderModel.PhoneNumber,
                UserId = postOrderModel.UserId,

            });

            await _context.SaveChangesAsync();

            await AddOrderProducts(result.Entity.Id, postOrderModel.Products.ToList());

            return await GetOrder(result.Entity.Id.ToString());
        }

        private async Task AddOrderProducts(Guid orderId, List<Guid> productIds)
        {
            foreach (Guid productId in productIds)
            {
                await _context.OrderProducts.AddAsync(new OrderProduct
                {
                    OrderId = orderId,
                    ProductId = productId,
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task PutOrder(string id, PutOrderModel putOrderModel)
        {
            try
            {
                Order order = await _context.Orders
                    .Include(x => x.OrderProducts)
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (order == null)
                {
                    throw new KeyNotFoundException();
                }

                order.Orderdate = putOrderModel.Orderdate;
                order.PhoneNumber = putOrderModel.PhoneNumber;
                order.UserId = putOrderModel.UserId;

                _context.OrderProducts.RemoveRange(order.OrderProducts);


                await AddOrderProducts(Guid.Parse(id), putOrderModel.Products.ToList());
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutOrder");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "PutOrder");
                }
            }
        }
    }
}
