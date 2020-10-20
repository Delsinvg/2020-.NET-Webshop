using project.models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IRoleRepository
    {
        Task<List<GetRoleModel>> GetRoles();
        Task<GetRoleModel> GetRole(string id);
        Task<GetRoleModel> PostRole(PostRoleModel postRoleModel);
        Task PutRole(string id, PutRoleModel putRoleModel);
        Task DeleteRole(string id);
    }
}

