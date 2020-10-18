using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
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

        public async Task DeleteImage(string id)
        {
            try
            {
                Image image = await _context.Images.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (image == null)
                {
                    throw new KeyNotFoundException();
                }


                _context.Images.Remove(image);

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteImage");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "DeleteImage");
                }
            }
            
        }

        public async Task<GetImageModel> GetImage(string id)
        {
            try
            {
                GetImageModel image = await _context.Images
                    .Select(x => new GetImageModel
                    {
                        Id = x.Id,
                        Path = x.Path,
                        Product = x.Product.Name
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return image;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetImage");
            }
        }

        public async Task<List<GetImageModel>> GetImages()
        {
            try
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

                return images;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetImage");
            }
        }

        public async Task<GetImageModel> PostImage(PostImageModel postImageModel)
        {
            EntityEntry<Image> result = await _context.Images.AddAsync(new Image
            {
                Path = postImageModel.Path,
                ProductId = postImageModel.ProductId
            });

            await _context.SaveChangesAsync();

            return new GetImageModel
            {
                Id = result.Entity.Id,
                Path = result.Entity.Path,
                Product = result.Entity.Product.Name
            };
        }

        public async Task PutImage(string id, PutImageModel putImageModel)
        {
            try
            {
                Image image = await _context.Images.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                if (image == null)
                {
                    throw new KeyNotFoundException();
                }

                image.Path = putImageModel.Path;
                image.ProductId = putImageModel.ProductId;

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }

                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutImage");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "PutImage");
                }
            }
        }
    }
}
