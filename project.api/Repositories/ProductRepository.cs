using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Images;
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

                if (product.Images != null && product.Images.Count > 0)
                {
                    _context.Images.RemoveRange(product.Images);
                }

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
                    ImagesModel = x.Images.Select(x => new ImageModel
                    {
                        Name = x.Name,
                        FileType = x.FileType,
                        Extension = x.Extension,
                        Description = x.Description,
                        Data = x.Data
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();


            if (products.Count == 0)
            {
                throw new CollectionException("No products found.", this.GetType().Name, "GetProducts", "404");
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
                    ImagesModel = x.Images.Select(x => new ImageModel
                    {
                        Name = x.Name,
                        FileType = x.FileType,
                        Extension = x.Extension,
                        Description = x.Description,
                        Data = x.Data
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);


            if (product == null)
            {
                throw new CollectionException("No products found.", this.GetType().Name, "GetProducts", "404");
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

            if (postProductModel.ImageModels != null && postProductModel.ImageModels.Count > 0)
            {
                result.Entity.Images = new List<Image>();

                foreach (ImageModel imageModel in postProductModel.ImageModels)
                {
                    Image image = new Image
                    {
                        Name = imageModel.Name,
                        FileType = imageModel.FileType,
                        Extension = imageModel.Extension,
                        Data = imageModel.Data
                    };

                    result.Entity.Images.Add(image);
                }
            }

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
                    throw new EntityException("Product niet gevonden", this.GetType().Name, "PutProduct", "404");
                }

                if (product.Images.Count > 0)
                {
                    if (putProductModel.AfbeeldingNames == null)
                    {
                        _context.Images.RemoveRange(product.Images);
                    }
                    else
                    {
                        string[] namen = putProductModel.AfbeeldingNames.Split(",");

                        foreach (Image image in product.Images)
                        {
                            if (!namen.Contains(image.Name))
                            {
                                _context.Images.Remove(image);
                            }
                        }
                    }
                }

                product.Name = putProductModel.Name;
                product.Description = putProductModel.Description;
                product.Stock = putProductModel.Stock;
                product.Price = putProductModel.Price;
                product.CategoryId = putProductModel.CategoryId;
                product.CompanyId = putProductModel.CompanyId;

                if (putProductModel.ImageModels != null && putProductModel.ImageModels.Count > 0)
                {
                    if (product.Images == null || product.Images.Count == 0)
                    {
                        product.Images = new List<Image>();
                    }

                    foreach (ImageModel imageModel in putProductModel.ImageModels)
                    {
                        Image image = new Image
                        {
                            Name = imageModel.Name,
                            FileType = imageModel.FileType,
                            Extension = imageModel.Extension,
                            Data = imageModel.Data
                        };

                        product.Images.Add(image);
                    }
                }

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
