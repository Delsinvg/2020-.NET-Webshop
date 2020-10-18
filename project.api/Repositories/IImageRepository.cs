using project.models.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IImageRepository
    {
        Task<List<GetImageModel>> GetImages();
        Task<GetImageModel> GetImage(string id);
        Task<GetImageModel> PostImage(PostImageModel postImageModel);
        Task PutImage(string id, PutImageModel putImageModel);
        Task DeleteImage(string id);
    }
}
