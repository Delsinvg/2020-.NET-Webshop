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

        public async Task DeleteCategory(Guid id)
        {
            try
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    throw new KeyNotFoundException();
                }
                _context.Categories.Remove(category);

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
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteCategory", "400");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "DeleteCategory", "400");
                }
            }
        }

        public async Task<List<GetCategoryModel>> GetCategories()
        {

            List<GetCategoryModel> categories = await _context.Categories
                .Select(x => new GetCategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentCategory = x.ParentCategory.Name,
                    ParentId = x.ParentId
                })
                .AsNoTracking()
                .ToListAsync();



            if (categories.Count == 0)
            {
                throw new CollectionException("No Categories found", this.GetType().Name, "GetCategories", "404");
            }
            return categories;
        }

        public async Task<GetCategoryModel> GetCategory(Guid id)
        {

            GetCategoryModel category = await _context.Categories
            .Select(x => new GetCategoryModel
            {
                Id = x.Id,
                Name = x.Name,
                ParentCategory = x.ParentCategory.Name,
                ParentId = x.ParentId
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                throw new EntityException("Category not found.", this.GetType().Name, "GetUitgeverij", "404");
            }

            return category;
        }

        public async Task<GetCategoryModel> PostCategory(PostCategoryModel postCategoryModel)
        {
            EntityEntry<Category> result = await _context.Categories.AddAsync(new Category
            {
                Name = postCategoryModel.Name,
                ParentId = postCategoryModel.ParentId
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PostAddress", "400");
            }

            return await GetCategory(result.Entity.Id);
        }

        public async Task PutCategory(Guid id, PutCategoryModel putCategoryModel)
        {
            try
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                {
                    throw new KeyNotFoundException();
                }

                category.Name = putCategoryModel.Name;
                category.ParentId = putCategoryModel.ParentId;

                await _context.SaveChangesAsync();

            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PutAddress", "400");
            }
        }
    }
}
