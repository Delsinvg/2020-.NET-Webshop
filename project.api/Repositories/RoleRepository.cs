using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using project.api.Entities;
using project.models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ProjectContext _context;
        private readonly RoleManager<Role> _roleManager;

        public RoleRepository(ProjectContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task DeleteRole(string id)
        {
            Role role = await _roleManager.FindByIdAsync(id);

            IdentityResult result = await _roleManager.DeleteAsync(role);
        }

        public async Task<GetRoleModel> GetRole(string id)
        {
            GetRoleModel role = await _context.Roles
                .Select(x => new GetRoleModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return role;
        }

        public async Task<List<GetRoleModel>> GetRoles()
        {
            List<GetRoleModel> roles = await _context.Roles
                .Select(x => new GetRoleModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .AsNoTracking()
                .ToListAsync();

            return roles;
        }

        public async Task<GetRoleModel> PostRole(PostRoleModel postRoleModel)
        {
            Role role = new Role
            {
                Name = postRoleModel.Name,
                Description = postRoleModel.Description
            };

            IdentityResult result = await _roleManager.CreateAsync(role);

            return new GetRoleModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public async Task PutRole(string id, PutRoleModel putRoleModel)
        {
            Role role = await _roleManager.FindByIdAsync(id);

            role.Name = putRoleModel.Name;
            role.Description = putRoleModel.Description;

            IdentityResult result = await _roleManager.UpdateAsync(role);
        }
    }
}
