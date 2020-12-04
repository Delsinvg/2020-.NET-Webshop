using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Companies;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        //private readonly ITokenValidationService _tokenValidationService;

        public CompaniesController(ProjectApiService projectApiService)
        {
            _projectApiService = projectApiService;
            //_tokenValidationService = tokenValidationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //Authorize("Moderator", "Index");

                //await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetCompanyModel> getCompaniesModel = await _projectApiService.GetModels<GetCompanyModel>("Companies");

                return View(getCompaniesModel);
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
                //Authorize("Moderator", "Details");

                //await _tokenValidationService.Validate(this.GetType().Name, "Details");

                GetCompanyModel getCompanyModel = await _projectApiService.GetModel<GetCompanyModel>(id, "Companies");

                return View(getCompanyModel);
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
                //Authorize("Moderator", "Create");

                //await _tokenValidationService.Validate(this.GetType().Name, "Create GET");

                return View();
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Create(PostCompanyModel postCompanyModel)
        {
            try
            {
                //Authorize("Moderator", "Create");

                //await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                if (ModelState.IsValid)
                {
                    GetCompanyModel getCompanyModel = await _projectApiService.PostModel<PostCompanyModel, GetCompanyModel>(postCompanyModel, "Companies");

                    return RedirectToRoute(new { action = "Index", controller = "Companies" });
                }

                return View(postCompanyModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postCompanyModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                //Authorize("Moderator", "Edit");

                //await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

                GetCompanyModel getCompanyModel = await _projectApiService.GetModel<GetCompanyModel>(id, "Companies");

                PutCompanyModel putCompanyModel = new PutCompanyModel
                {
                    Name = getCompanyModel.Name,
                    AccountNumber = getCompanyModel.AccountNumber,
                    PhoneNumber = getCompanyModel.PhoneNumber,
                    Email = getCompanyModel.Email
                };

                return View(putCompanyModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutCompanyModel putCompanyModel)
        {
            try
            {
                //Authorize("Moderator", "Edit");

                //await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutCompanyModel>(id, putCompanyModel, "Companies");

                    return RedirectToRoute(new { action = "Index", controller = "Companies" });
                }

                return View(putCompanyModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putCompanyModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                //Authorize("Moderator", "Delete");

                //await _tokenValidationService.Validate(this.GetType().Name, "Delete GET");

                GetCompanyModel getCompanyModel = await _projectApiService.GetModel<GetCompanyModel>(id, "Companies");

                return View(getCompanyModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetCompanyModel getCompanyModel)
        {
            try
            {
                //Authorize("Moderator", "Delete");

                //await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Companies");

                return RedirectToRoute(new { action = "Index", controller = "Companies" });
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
                    error = "No permission to get all companies";
                    break;
                case "Details":
                    error = "No permission to get companies";
                    break;
                case "Create":
                    error = "No permission to create company";
                    break;
                case "Edit":
                    error = "No permission to edit company";
                    break;
                case "Delete":
                    error = "No permission to delete company";
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
