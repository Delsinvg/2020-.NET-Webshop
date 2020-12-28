using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Products;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;

        public ProductsController(ProjectApiService projectApiService, ITokenValidationService tokenValidationService)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                Authorize("Beheerder", "Index");

                await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetProductModel> getProductsModel = await _projectApiService.GetModels<GetProductModel>("Products");

                return View(getProductsModel);
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
                Authorize("Beheerder", "Details");

                await _tokenValidationService.Validate(this.GetType().Name, "Details");

                GetProductModel getProductModel = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                return View(getProductModel);
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
        public async Task<IActionResult> Create(PostProductModel postProductModel)
        {
            try
            {
                Authorize("Beheerder", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                if (ModelState.IsValid)
                {
                    GetProductModel getProductModel = await _projectApiService.PostModel<PostProductModel, GetProductModel>(postProductModel, "Products");

                    return RedirectToRoute(new { action = "Index", controller = "Products" });
                }

                return View(postProductModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postProductModel);
                }

                return HandleError(e);
            }
        }
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                Authorize("Beheerder", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

                GetProductModel getProductModel = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                PutProductModel putProductModel = new PutProductModel
                {
                    Name = getProductModel.Name,
                    Description = getProductModel.Description,
                    Price = getProductModel.Price,
                    Stock = getProductModel.Stock,
                };

                return View(putProductModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutProductModel putProductModel)
        {
            try
            {
                Authorize("Beheerder", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutProductModel>(id, putProductModel, "Products");

                    return RedirectToRoute(new { action = "Index", controller = "Products" });
                }

                return View(putProductModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putProductModel);
                }

                return HandleError(e);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Authorize("Beheerder", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete GET");

                GetProductModel getProductModel = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                return View(getProductModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetProductModel getProductModel)
        {
            try
            {
                Authorize("Beheerder", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Products");

                return RedirectToRoute(new { action = "Index", controller = "Products" });
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
                    error = "Onvoldoende rechten om producten op te vragen";
                    break;
                case "Details":
                    error = "Onvoldoende rechten om de details van een product op te vragen";
                    break;
                case "Create":
                    error = "Onvoldoende rechten om product aan te maken";
                    break;
                case "Edit":
                    error = "Onvoldoende rechten om product aan te passen";
                    break;
                case "Delete":
                    error = "Onvoldoende rechten om product te verwijderen";
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
