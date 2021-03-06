﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using project.api.Exceptions;
using project.models.Addresses;
using project.models.Users;
using project.shared.Settings;
using project.web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly FileUploadSettings _fileUploadSettings;
        private readonly IStringLocalizer<UsersController> _localizer;

        public UsersController(
            ProjectApiService projectApiService, ITokenValidationService tokenValidationService,
        IOptions<FileUploadSettings> fileUploadSettings,
        IStringLocalizer<UsersController> localizer)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
            _fileUploadSettings = fileUploadSettings.Value;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Authorize("Moderator", "Index");

                await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetUserModel> getUsersModel = await _projectApiService.GetModels<GetUserModel>("Users");

                return View(getUsersModel);
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
                Authorize("Customer", "Details");

                await _tokenValidationService.Validate(this.GetType().Name, "Details");

                GetUserModel getUserModel = await _projectApiService.GetModel<GetUserModel>(id, "Users");


                return View(getUserModel);
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
                Authorize("Moderator", "Create");

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
        public async Task<IActionResult> Create(PostUserModel postUserModel)
        {
            try
            {
                Authorize("Moderator", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");


                if (ModelState.IsValid)
                {
                    GetUserModel getUserModel = await _projectApiService.PostModel<PostUserModel, GetUserModel>(postUserModel, "Users");

                    return RedirectToRoute(new { action = "Index", controller = "Users" });
                }

                return View(postUserModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("IdentityException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postUserModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                Authorize("Administrator", "Edit");

                ViewBag.Addresses = (await _projectApiService.GetModels<GetAddressModel>("Addresses")).OrderBy(x => x.City).ToList();

                await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

                GetUserModel getUserModel = await _projectApiService.GetModel<GetUserModel>(id, "Users");


                PutUserModel putUserModel = new PutUserModel
                {
                    FirstName = getUserModel.FirstName,
                    LastName = getUserModel.LastName,
                    Email = getUserModel.Email,
                    Roles = getUserModel.Roles
                };

                return View(putUserModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutUserModel putUserModel)
        {
            try
            {
                Authorize("Administrator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutUserModel>(id, putUserModel, "Users");

                    return RedirectToRoute(new { action = "Index", controller = "Users" });
                }

                return View(putUserModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("IdentityException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putUserModel);
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

                GetUserModel getUserModel = await _projectApiService.GetModel<GetUserModel>(id, "Users");

                return View(getUserModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetUserModel getUserModel)
        {
            try
            {
                Authorize("Administrator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Users");

                return RedirectToRoute(new { action = "Index", controller = "Users" });
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> ChangePassword(string id, PatchUserModel patchUserModel)
        {
            try
            {
                Authorize("Customer", "PatchUser");

                if (patchUserModel.NewPassword != patchUserModel.ConfirmNewPassword)
                {
                    ModelState.AddModelError("ConfirmNewPassword", "Wachtwoorden komen niet overeen");
                }

                if (ModelState.IsValid)
                {
                    await _projectApiService.PatchModel<PatchUserModel>(id, patchUserModel, "Users");

                    return RedirectToRoute(new { action = "Index", controller = "Home" });
                }

                return View(patchUserModel);
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
                    error = _localizer["Onvoldoende rechten om users op te vragen"];
                    break;
                case "Details":
                    error = _localizer["Onvoldoende rechten om de details van een user op te vragen"];
                    break;
                case "Create":
                    error = _localizer["Onvoldoende rechten om user aan te maken"];
                    break;
                case "Edit":
                    error = _localizer["Onvoldoende rechten om user aan te passen"];
                    break;
                case "Delete":
                    error = _localizer["Onvoldoende rechten om user te verwijderen"];
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
                return RedirectToRoute(new { action = "Index", controller = "Authentication" });
            }
            else // In case of all other errors redirect to home page
            {
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }
        }
    }
}
