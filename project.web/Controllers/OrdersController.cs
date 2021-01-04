using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using project.api.Exceptions;
using project.models.Orders;
using project.web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly IStringLocalizer<OrdersController> _localizer;

        public OrdersController(ProjectApiService projectApiService, ITokenValidationService tokenValidationService, IStringLocalizer<OrdersController> localizer)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                Authorize("Moderator", "Index");

                await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetOrderModel> getOrderModels = await _projectApiService.GetModels<GetOrderModel>("Orders");

                return View(getOrderModels);
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

                GetOrderModel getOrderModel = await _projectApiService.GetModel<GetOrderModel>(id, "Orders");

                return View(getOrderModel);
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
        public async Task<IActionResult> Create(PostOrderModel postOrderModel)
        {
            try
            {
                Authorize("Moderator", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                if (ModelState.IsValid)
                {
                    GetOrderModel getOrderModel = await _projectApiService.PostModel<PostOrderModel, GetOrderModel>(postOrderModel, "Orders");

                    return RedirectToRoute(new { action = "Index", controller = "Orders" });
                }

                return View(postOrderModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(postOrderModel);
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

                GetOrderModel getOrderModel = await _projectApiService.GetModel<GetOrderModel>(id, "Orders");

                PutOrderModel putOrderModel = new PutOrderModel
                {
                    Orderdate = getOrderModel.Orderdate,
                };

                return View(putOrderModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Edit(string id, PutOrderModel putOrderModel)
        {
            try
            {
                Authorize("Moderator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (ModelState.IsValid)
                {
                    await _projectApiService.PutModel<PutOrderModel>(id, putOrderModel, "Orders");

                    return RedirectToRoute(new { action = "Index", controller = "Orders" });
                }

                return View(putOrderModel);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Type.Equals("DatabaseException"))
                {
                    TempData["ApiError"] = e.Message;

                    return View(putOrderModel);
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

                GetOrderModel getOrderModel = await _projectApiService.GetModel<GetOrderModel>(id, "Orders");

                return View(getOrderModel);
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Prevents XSRF/CSRF attacks
        public async Task<IActionResult> Delete(string id, GetOrderModel getOrderModel)
        {
            try
            {
                Authorize("Moderator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete POST");

                await _projectApiService.DeleteModel(id, "Orders");

                return RedirectToRoute(new { action = "Index", controller = "Orders" });
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
                    error = _localizer["Onvoldoende rechten om orders op te vragen"];
                    break;
                case "Details":
                    error = _localizer["Onvoldoende rechten om de details van een order op te vragen"];
                    break;
                case "Create":
                    error = _localizer["Onvoldoende rechten om order aan te maken"];
                    break;
                case "Edit":
                    error = _localizer["Onvoldoende rechten om order aan te passen"];
                    break;
                case "Delete":
                    error = _localizer["Onvoldoende rechten om order te verwijderen"];
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
