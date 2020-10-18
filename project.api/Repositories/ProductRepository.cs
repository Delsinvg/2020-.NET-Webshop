using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private readonly ProjectContext _context;

        public ProductRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task DeleteProduct(string id)
        {
            try
            {
                Product product = await _context.Products
                    .Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (product == null)
                {
                    throw new KeyNotFoundException();
                }

                _context.Images.RemoveRange(product.Images);

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteProduct");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "DeleteProduct");
                }
            }
        }

        public async Task<List<GetProductModel>> GetProducts()
        {
            try
            {

                List<GetProductModel> products = await _context.Products
                   .Include(x => x.Images)
                    .Include(x => x.Category)
                    .Include(x => x.Company)
                    .Select(x => new GetProductModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price,
                        Description = x.Description,
                        Stock = x.Stock,
                        Company = x.Company.Name,
                        Category = x.Category.Name,
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return products;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetProducts");
            }
        }

        public async Task<GetProductModel> GetProduct(string id)
        {
            try
            {
                GetProductModel product = await _context.Products
                    .Include(x => x.Images)
                    .Include(x => x.Category)
                    .Include(x => x.Company)
                    .Select(x => new GetProductModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price,
                        Description = x.Description,
                        Stock = x.Stock,
                        Company = x.Company.Name,
                        Category = x.Category.Name,
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return product;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetProduct");
            }
        }

        public async Task<GetProductModel> PostProduct(PostProductModel postProductModel)
        {
            EntityEntry<Product> result = await _context.Products.AddAsync(new Product
            {
                Name = postProductModel.Name,
                Description = postProductModel.Description,
                Stock = postProductModel.Stock,
                Price = postProductModel.Price,
                CategoryId = postProductModel.CategoryId,
                CompanyId = postProductModel.CompanyId,
                
            });

            await _context.SaveChangesAsync();

            return new GetProductModel
            {
                Id = result.Entity.Id,
                Name = result.Entity.Name,
                Description = result.Entity.Description,
                Stock = result.Entity.Stock,
                Price = result.Entity.Price,
                Category = result.Entity.Category.Name,
                Company = result.Entity.Company.Name
            };
        }

        public async Task PutProduct(string id, PutProductModel putProductModel)
        {
            try
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (product == null)
                {
                    throw new KeyNotFoundException();
                }
                product.Name = putProductModel.Name;
                product.Description = putProductModel.Description;
                product.Stock = putProductModel.Stock;
                product.Price = putProductModel.Price;
                product.CategoryId = putProductModel.CategoryId;
                product.CompanyId = putProductModel.CompanyId;

                await _context.SaveChangesAsync();
            } catch(Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }

                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutProduct");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "PutProduct");
                }
            }
        }
    }
}
