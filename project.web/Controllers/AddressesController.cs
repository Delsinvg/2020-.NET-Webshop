using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Addresses;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class AddressesController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;

        public AddressesController(ProjectApiService projectApiService, ITokenValidationService tokenValidationService)
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

                List<GetAddressModel> getAddressModels = await _projectApiService.GetModels<GetAddressModel>("Addresses");

                return View(getAddressModels);
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

                GetAddressModel getAddressModel = await _projectApiService.GetModel<GetAddressModel>(id, "Addresses");

                return View(getAddressModel);
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
        public async Task<IActionResult> Create(PostAddressModel postAddressModel)
        {
            try
            {
                //Authorize("Beheerder", "Create");

                //await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                if (ModelState.IsValid)
                {
                    GetAddressModel getAddressModel = await _projectApiService.PostModel<PostAddressModel, GetAddressModel>(postAddressModel, "Addresses");

                    return RedirectToRoute(new { action = "Index", controller = "Addresses" });
                }

                return View(postAddressModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postAddressModel);
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

                GetAddressModel getAddressModel = await _projectApiService.GetModel<GetAddressModel>(id, "Addresses");

                PutAddressModel putAddressModel = new PutAddressModel
                {
                    City = getAddressModel.City,
                    Country = getAddressModel.Country,
                    CountryCode = getAddressModel.CountryCode,
                    PostalCode = getAddressModel.PostalCode,
                    Street = getAddressModel.Street
                };

                return View(putAddressModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutAddressModel putAddressModel)
        {
            try
            {
                //Authorize("Beheerder", "Edit");

                //await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutAddressModel>(id, putAddressModel, "Addresses");

                    return RedirectToRoute(new { action = "Index", controller = "Addresses" });
                }

                return View(putAddressModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putAddressModel);
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

                GetAddressModel getAddressModel = await _projectApiService.GetModel<GetAddressModel>(id, "Addresses");

                return View(getAddressModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetAddressModel getAddressModel)
        {
            try
            {
                //Authorize("Beheerder", "Delete");

                //await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Addresses");

                return RedirectToRoute(new { action = "Index", controller = "Addresses" });
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
                    error = "No permission to get all addresses";
                    break;
                case "Details":
                    error = "No permission to get address details";
                    break;
                case "Create":
                    error = "No permission to create address";
                    break;
                case "Edit":
                    error = "No permission to edit address";
                    break;
                case "Delete":
                    error = "No permission to delete address";
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
