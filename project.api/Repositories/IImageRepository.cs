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
        Task<GetImageModel> GetImage(Guid id);
        Task<GetImageModel> PostImage(PostImageModel postImageModel);
        Task PutImage(Guid id, PutImageModel putImageModel);
        Task DeleteImage(Guid id);
    }
}
