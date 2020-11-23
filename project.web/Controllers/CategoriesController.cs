using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Categories;
using project.web.Services;

namespace project.web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;

        public CategoriesController(ProjectApiService projectApiService, ITokenValidationService tokenValidationService)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Authorize("Moderator", "Index");

                await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetCategoryModel> getCategoriesModel = await _projectApiService.GetModels<GetCategoryModel>("Categories");

                return View(getCategoriesModel);
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
                Authorize("Moderator", "Details");

                await _tokenValidationService.Validate(this.GetType().Name, "Details");

                GetCategoryModel getCategoryModel = await _projectApiService.GetModel<GetCategoryModel>(id, "Categories");

                return View(getCategoryModel);
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
        public async Task<IActionResult> Create(PostCategoryModel postCategoryModel)
        {
            try
            {
                Authorize("Moderator", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                if (ModelState.IsValid)
                {
                    GetCategoryModel getCategoryModel = await _projectApiService.PostModel<PostCategoryModel, GetCategoryModel>(postCategoryModel, "Categories");

                    return RedirectToRoute(new { action = "Index", controller = "Categories" });
                }

                return View(postCategoryModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postCategoryModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                Authorize("Moderator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

                GetCategoryModel getCategoryModel = await _projectApiService.GetModel<GetCategoryModel>(id, "Categories");

                PutCategoryModel putCategoryModel = new PutCategoryModel
                {
                    Name = getCategoryModel.Name,
                    ParentId = (Guid)getCategoryModel.ParentId
                };

                return View(putCategoryModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutCategoryModel putCategoryModel)
        {
            try
            {
                Authorize("Moderator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutCategoryModel>(id, putCategoryModel, "Categories");

                    return RedirectToRoute(new { action = "Index", controller = "Categories" });
                }

                return View(putCategoryModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putCategoryModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Authorize("Moderator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete GET");

                GetCategoryModel getCategoryModel = await _projectApiService.GetModel<GetCategoryModel>(id, "Categories");

                return View(getCategoryModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetCategoryModel getCategoryModel)
        {
            try
            {
                Authorize("Moderator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Categories");

                return RedirectToRoute(new { action = "Index", controller = "Categories" });
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
                    error = "No permission to get all Categories";
                    break;
                case "Details":
                    error = "No permission to get Category details";
                    break;
                case "Create":
                    error = "No permission to create Category";
                    break;
                case "Edit":
                    error = "No permission to edit Category";
                    break;
                case "Delete":
                    error = "No permission to delete Category";
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
