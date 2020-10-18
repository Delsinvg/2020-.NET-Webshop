using project.models.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<GetCategoryModel>> GetCategories();
        Task<GetCategoryModel> GetCategory(string id);
        Task<GetCategoryModel> PostCategory(PostCategoryModel postCategoryModel);
        Task PutCategory(string id, PutCategoryModel putCategoryModel);
        Task DeleteCategory(string id);
    }
}
