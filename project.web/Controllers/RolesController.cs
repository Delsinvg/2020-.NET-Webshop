﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using project.api.Exceptions;
using project.models.Roles;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class RolesController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly IStringLocalizer<RolesController> _localizer;

        public RolesController(
            ProjectApiService projectApiService,
            ITokenValidationService tokenValidationService,
            IStringLocalizer<RolesController> localizer)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Authorize("Administrator", "Index");

                await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetRoleModel> getRolesModel = await _projectApiService.GetModels<GetRoleModel>("Roles");

                return View(getRolesModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                Authorize("Administrator", "Details");

                await _tokenValidationService.Validate(this.GetType().Name, "Details");

                GetRoleModel getRoleModel = await _projectApiService.GetModel<GetRoleModel>(id, "Roles");

                return View(getRoleModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                Authorize("Administrator", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create GET");

                return View();
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Create(PostRoleModel postRoleModel)
        {
            try
            {
                Authorize("Administrator", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                if (ModelState.IsValid)
                {
                    GetRoleModel getRoleModel = await _projectApiService.PostModel<PostRoleModel, GetRoleModel>(postRoleModel, "Roles");

                    return RedirectToRoute(new { action = "Index", controller = "Roles" });
                }

                return View(postRoleModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("IdentityException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postRoleModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                Authorize("Administrator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

                GetRoleModel getRoleModel = await _projectApiService.GetModel<GetRoleModel>(id, "Roles");

                PutRoleModel putRoleModel = new PutRoleModel
                {
                    Name = getRoleModel.Name,
                    Description = getRoleModel.Description
                };

                return View(putRoleModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutRoleModel putRoleModel)
        {
            try
            {
                Authorize("Administrator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutRoleModel>(id, putRoleModel, "Roles");

                    return RedirectToRoute(new { action = "Index", controller = "Roles" });
                }

                return View(putRoleModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("IdentityException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putRoleModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Authorize("Administrator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete GET");

                GetRoleModel getRoleModel = await _projectApiService.GetModel<GetRoleModel>(id, "Roles");

                return View(getRoleModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetRoleModel getRoleModel)
        {
            try
            {
                Authorize("Administrator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Roles");

                return RedirectToRoute(new { action = "Index", controller = "Roles" });
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        private void Authorize(string role, string method)
        {
            string error = string.Empty;

            switch (method)
            {
                case "Index":
                    error = _localizer["Onvoldoende rechten om rollen op te vragen"];
                    break;
                case "Details":
                    error = _localizer["Onvoldoende rechten om de details van een rollen op te vragen"];
                    break;
                case "Create":
                    error = _localizer["Onvoldoende rechten om rollen aan te maken"];
                    break;
                case "Edit":
                    error = _localizer["Onvoldoende rechten om rollen aan te passen"];
                    break;
                case "Delete":
                    error = _localizer["Onvoldoende rechten om rollen te verwijderen"];
                    break;
            }

            if (!HttpContext.User.IsInRole(role))
            {
                throw new ForbiddenException(error, this.GetType().Name, $"{method}", "403");
            }
        }

        private IActionResult HandleError(ProjectException e)
        {
            // Place error in TempData to show in View
            TempData["ApiError"] = e.Message;

            // In case of 401 Unauthorized redirect to login
            if (e.ProjectError.Status.Equals("401"))
            {
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }
            else // In case of all other errors redirect to home page
            {
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }
        }
    }
}
