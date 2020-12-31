using Microsoft.AspNetCore.Mvc;
using project.models.Roles;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Views.Users.Components.Roles
{
    public class RolesViewComponent : ViewComponent
    {
        private readonly ProjectApiService _projectApiService;

        public RolesViewComponent(ProjectApiService projectApiService)
        {
            _projectApiService = projectApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<string> checkedRoles)
        {
            List<GetRoleModel> getRolesModel = await GetRoles();

            if (checkedRoles.Count > 0)
            {
                foreach (GetRoleModel getRoleModel in getRolesModel)
                {
                    if (checkedRoles.Contains(getRoleModel.Name))
                    {
                        getRoleModel.Checked = true;
                    }
                }
            }

            return View(getRolesModel);
        }

        private async Task<List<GetRoleModel>> GetRoles()
        {
            return await _projectApiService.GetModels<GetRoleModel>("Roles");
        }
    }
}
