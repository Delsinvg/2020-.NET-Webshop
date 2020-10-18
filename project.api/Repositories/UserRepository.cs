using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using project.api.Entities;
using project.models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserRepository(ProjectContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            IdentityResult result = await _userManager.DeleteAsync(user);
        }

        public async Task<GetUserModel> GetUser(string id)
        {
            GetUserModel user = await _context.Users
                .Include(x => x.UserRoles)
                .Select(x => new GetUserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return user;
        }

        public async Task<List<GetUserModel>> GetUsers()
        {
            List<GetUserModel> users = await _context.Users
                .Include(x => x.UserRoles)
                .Select(x => new GetUserModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task PatchUser(string id, PatchUserModel patchUserModel)
        {
            User user = await _userManager.FindByIdAsync(id);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, patchUserModel.CurrentPassword, patchUserModel.NewPassword);
        }

        public async Task<GetUserModel> PostUser(PostUserModel postUserModel)
        {
            User user = new User
            {
                UserName = postUserModel.Email,
                FirstName = postUserModel.FirstName,
                LastName = postUserModel.LastName,
                Email = postUserModel.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, postUserModel.Password);

            await _userManager.AddToRolesAsync(user, postUserModel.Roles);

            return await GetUser(user.Id.ToString());
        }

        public async Task PutUser(string id, PutUserModel putUserModel)
        {
            User user = await _userManager.FindByIdAsync(id);

            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

            user.FirstName = putUserModel.FirstName;
            user.LastName = putUserModel.LastName;
            user.Email = putUserModel.Email;

            await _userManager.AddToRolesAsync(user, putUserModel.Roles);

            IdentityResult result = await _userManager.UpdateAsync(user);
        }
    }
}
