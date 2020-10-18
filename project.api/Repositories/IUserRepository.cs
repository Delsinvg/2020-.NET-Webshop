using project.models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IUserRepository
    {
        Task<List<GetUserModel>> GetUsers();
        Task<GetUserModel> GetUser(string id);
        Task<GetUserModel> PostUser(PostUserModel postUserModel);
        Task PutUser(string id, PutUserModel putUserModel);
        Task PatchUser(string id, PatchUserModel patchUserModel);
        Task DeleteUser(string id);
    }
}
