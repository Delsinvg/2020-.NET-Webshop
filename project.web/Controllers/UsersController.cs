using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Users;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        //private readonly ITokenValidationService _tokenValidationService;
        //private readonly FileUploadSettings _fileUploadSettings;

        public UsersController(
            ProjectApiService projectApiService)
        //,ITokenValidationService tokenValidationService,
        //IOptions<FileUploadSettings> fileUploadSettings)
        {
            _projectApiService = projectApiService;
            //_tokenValidationService = tokenValidationService;
            //_fileUploadSettings = fileUploadSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //Authorize("Beheerder", "Index");

                //await _tokenValidationService.Validate(this.GetType().Name, "Index");

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
                //Authorize("Beheerder", "Details");

                //await _tokenValidationService.Validate(this.GetType().Name, "Details");

                GetUserModel getUserModel = await _projectApiService.GetModel<GetUserModel>(id, "Users");

                //ViewData["ImageData"] = GetImage(getUserModel.AfbeeldingModel.Data, getUserModel.AfbeeldingModel.Bestandstype);

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
                //Authorize("Beheerder", "Create");

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
        public async Task<IActionResult> Create(PostUserModel postUserModel)
        {
            try
            {
                //Authorize("Beheerder", "Create");

                //await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                //if (postUserModel.Afbeelding != null && ValidateImage(postUserModel.Afbeelding))
                //{
                //    postUserModel.AfbeeldingModel = await SetAfbeelding(postUserModel.Afbeelding);
                //}

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
                //Authorize("Beheerder", "Edit");

                //await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

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
                //Authorize("Beheerder", "Edit");

                //await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                //if (putUserModel.Afbeelding != null && ValidateImage(putUserModel.Afbeelding))
                //{
                //    putUserModel.AfbeeldingModel = await SetAfbeelding(putUserModel.Afbeelding);
                //}

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
                //Authorize("Beheerder", "Delete");

                //await _tokenValidationService.Validate(this.GetType().Name, "Delete GET");

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
                //Authorize("Beheerder", "Delete");

                //await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Users");

                return RedirectToRoute(new { action = "Index", controller = "Users" });
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
                    error = "No permission to get all users";
                    break;
                case "Details":
                    error = "No permission to get user details";
                    break;
                case "Create":
                    error = "No permission to create user";
                    break;
                case "Edit":
                    error = "No permission to edit user";
                    break;
                case "Delete":
                    error = "No permission to delete user";
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
