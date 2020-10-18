using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProjectContext _context;

        public CategoryRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task DeleteCategory(string id)
        {
            try
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (category == null)
                {
                    throw new KeyNotFoundException();
                }
                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteCategory");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "DeleteCategory");
                }
            }
        }

        public async Task<List<GetCategoryModel>> GetCategories()
        {
            try
            {
                List<GetCategoryModel> categories = await _context.Categories
                    .Select(x => new GetCategoryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentCategory = x.ParentCategory.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return categories;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetCategories");
            }
        }

        public async Task<GetCategoryModel> GetCategory(string id)
        {
            try
            {
                GetCategoryModel category = await _context.Categories
                .Select(x => new GetCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentCategory = x.ParentCategory.Name
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return category;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetCategory");
            }
        }

        public async Task<GetCategoryModel> PostCategory(PostCategoryModel postCategoryModel)
        {
            EntityEntry<Category> result = await _context.Categories.AddAsync(new Category
            {
                Name = postCategoryModel.Name,
                ParentId = postCategoryModel.ParentId
            });

            await _context.SaveChangesAsync();

            return new GetCategoryModel
            {
                Id = result.Entity.Id,
                Name = result.Entity.Name,
                ParentCategory = result.Entity.ParentCategory.Name
            };
        }

        public async Task PutCategory(string id, PutCategoryModel putCategoryModel)
        {
            try
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                if (category == null)
                {
                    throw new KeyNotFoundException();
                }

                category.Name = putCategoryModel.Name;
                category.ParentId = putCategoryModel.ParentId;

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }

                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutCategory");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "PutCategory");
                }
            }
        }
    }
}
