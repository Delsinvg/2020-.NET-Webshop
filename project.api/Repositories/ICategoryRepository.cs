using project.models.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<GetCategoryModel>> GetCategories();
        Task<GetCategoryModel> GetCategory(Guid id);
        Task<GetCategoryModel> PostCategory(PostCategoryModel postCategoryModel);
        Task PutCategory(Guid id, PutCategoryModel putCategoryModel);
        Task DeleteCategory(Guid id);
    }
}
