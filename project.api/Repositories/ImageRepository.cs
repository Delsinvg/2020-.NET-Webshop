using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Addresses;
using project.models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ProjectContext _context;

        public ImageRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task DeleteImage(Guid id)
        {
            try
            {
                Image image = await _context.Images.FirstOrDefaultAsync(x => x.Id == id);

                if (image == null)
                {
                    throw new EntityException("Image not found.", this.GetType().Name, "DeleteImage", "404");
                }


                _context.Images.Remove(image);

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "DeleteAddress", "400");
            }

        }

        public async Task<GetImageModel> GetImage(Guid id)
        {
            
                GetImageModel image = await _context.Images
                    .Select(x => new GetImageModel
                    {
                        Id = x.Id,
                        Path = x.Path,
                        Product = x.Product.Name
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
            {
                throw new EntityException("Image not found.", this.GetType().Name, "GetImage", "404");
            }

            return image;
            
        }

        public async Task<List<GetImageModel>> GetImages()
        {
            
                List<GetImageModel> images = await _context.Images
                    .Select(x => new GetImageModel
                    {
                        Id = x.Id,
                        Path = x.Path,
                        Product = x.Product.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();

            if (images.Count == 0)
            {
                throw new CollectionException("No images found.", this.GetType().Name, "GetImages", "404");
            }
            return images;
            
        }

        public async Task<GetImageModel> PostImage(PostImageModel postImageModel)
        {
            EntityEntry<Image> result = await _context.Images.AddAsync(new Image
            {
                Path = postImageModel.Path,
                ProductId = postImageModel.ProductId
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PostImage", "400");
            }

            return await GetImage(result.Entity.Id);
        }

        public async Task PutImage(Guid id, PutImageModel putImageModel)
        {
            try
            {
                Image image = await _context.Images.FirstOrDefaultAsync(x => x.Id == id);
                if (image == null)
                {
                    throw new EntityException("Image not found.", this.GetType().Name, "PutImage", "404");
                }

                image.Path = putImageModel.Path;
                image.ProductId = putImageModel.ProductId;

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PutImage", "400");
            }
        }
    }
}
