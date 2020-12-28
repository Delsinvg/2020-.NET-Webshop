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

        public async Task DeleteProduct(Guid id)
        {
            try
            {
                Product product = await _context.Products
                    .Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                {
                    throw new EntityException("Product not found.", this.GetType().Name, "DeleteProduct", "404");
                }

                if (product.OrderProducts.Count > 0)
                {
                    throw new CollectionException("Remove all Products first.", this.GetType().Name, "DeleteProducts", "400");
                }

                _context.Images.RemoveRange(product.Images);

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "DeleteProduct", "400");
            }
        }

        public async Task<List<GetProductModel>> GetProducts()
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
                    A
                })
                .AsNoTracking()
                .ToListAsync();


            if (products.Count == 0)
            {
                throw new CollectionException("No auteurs found.", this.GetType().Name, "GetProducts", "404");
            }

            return products;
        }

        public async Task<GetProductModel> GetProduct(Guid id)
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
                .FirstOrDefaultAsync(x => x.Id == id);


            if (product == null)
            {
                throw new CollectionException("No auteurs found.", this.GetType().Name, "GetProducts", "404");
            }
            return product;
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PostProduct", "400");
            }

            return await GetProduct(result.Entity.Id);

        }

        public async Task PutProduct(Guid id, PutProductModel putProductModel)
        {
            try
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

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
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PutProduct", "400");
            }
        }
    }
}
