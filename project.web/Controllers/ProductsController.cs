﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using project.api.Exceptions;
using project.models.Categories;
using project.models.Companies;
using project.models.Images;
using project.models.Products;
using project.web.Helpers;
using project.web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly IStringLocalizer<ProductsController> _localizer;


        public ProductsController(ProjectApiService projectApiService, ITokenValidationService tokenValidationService, IStringLocalizer<ProductsController> localizer)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                Authorize("Customer", "Index");

                await _tokenValidationService.Validate(this.GetType().Name, "Index");

                List<GetProductModel> getProductsModel = await _projectApiService.GetModels<GetProductModel>("Products");

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    List<GetProductModel> filteredList = new List<GetProductModel>();
                    foreach (var item in getProductsModel)
                    {
                        if (item.Name.ToLower().Contains(searchTerm.ToLower()))
                        {
                            filteredList.Add(item);
                        }
                    }
                    getProductsModel = filteredList;
                }

                for (int i = 0; i < getProductsModel.Count(); i++)
                {
                    if (getProductsModel[i].ImagesModel.Count != 0)
                    {
                        string key = getProductsModel[i].Id.ToString();
                        ViewData[key] = ImageHelper.GetImage(getProductsModel[i].ImagesModel.ToArray()[0].Data, getProductsModel[i].ImagesModel.ToArray()[0].FileType, "product");
                    }

                }

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
                Authorize("Customer", "Index");

                GetProductModel getProductModel = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                if (getProductModel.ImagesModel != null && getProductModel.ImagesModel.Count > 0)
                {
                    for (int i = 0; i < getProductModel.ImagesModel.Count; i++)
                    {
                        string key = "afbeelding" + i;
                        ViewData[key] = ImageHelper.GetImage(getProductModel.ImagesModel.ToArray()[i].Data, getProductModel.ImagesModel.ToArray()[i].FileType, "product");
                    }
                }

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
                Authorize("Moderator", "Create");

                ViewBag.Categories = (await _projectApiService.GetModels<GetCompanyModel>("Categories")).OrderBy(x => x.Name).ToList();
                ViewBag.Companies = (await _projectApiService.GetModels<GetCompanyModel>("Companies")).OrderBy(x => x.Name).ToList();

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
        public async Task<IActionResult> Create(PostProductModel postProductModel)
        {
            try
            {
                Authorize("Moderator", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                ViewBag.Categories = (await _projectApiService.GetModels<GetCompanyModel>("Categories")).OrderBy(x => x.Name).ToList();
                ViewBag.Companies = (await _projectApiService.GetModels<GetCompanyModel>("Companies")).OrderBy(x => x.Name).ToList();

                if (postProductModel.Images != null)
                {
                    postProductModel.ImageModels = new List<ImageModel>();

                    foreach (IFormFile image in postProductModel.Images)
                    {
                        if (await ImageHelper.ValidateImage(image, this.ModelState, "Images"))
                        {
                            postProductModel.ImageModels.Add(await ImageHelper.SetAfbeelding(image, "Afbeelding van de cover"));
                        }
                        else
                        {
                            break;
                        }
                    }
                }

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
                Authorize("Moderator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit GET");

                ViewBag.Categories = (await _projectApiService.GetModels<GetCategoryModel>("Categories")).OrderBy(x => x.Name).ToList();
                ViewBag.Companies = (await _projectApiService.GetModels<GetCompanyModel>("Companies")).OrderBy(x => x.Name).ToList();

                GetProductModel getProductModel = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                List<string> names = new List<string>();

                if (getProductModel.ImagesModel != null && getProductModel.ImagesModel.Count > 0)
                {
                    for (int i = 0; i < getProductModel.ImagesModel.Count; i++)
                    {
                        names.Add(getProductModel.ImagesModel.ToArray()[i].Name);
                    }
                }

                PutProductModel putProductModel = new PutProductModel
                {
                    Name = getProductModel.Name,
                    Description = getProductModel.Description,
                    Price = getProductModel.Price,
                    Stock = getProductModel.Stock,
                    ImageModels = getProductModel.ImagesModel,
                    ImageNames = string.Join(",", names)
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
                Authorize("Moderator", "Edit");

                await _tokenValidationService.Validate(this.GetType().Name, "Edit POST");

                if (putProductModel.Images != null)
                {
                    putProductModel.ImageModels = new List<ImageModel>();

                    foreach (IFormFile afbeelding in putProductModel.Images)
                    {
                        if (await ImageHelper.ValidateImage(afbeelding, this.ModelState, "Afbeeldingen"))
                        {
                            putProductModel.ImageModels.Add(await ImageHelper.SetAfbeelding(afbeelding, "Afbeelding van de cover"));
                        }
                        else
                        {
                            break;
                        }
                    }
                }


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
                Authorize("Moderator", "Delete");

                await _tokenValidationService.Validate(this.GetType().Name, "Delete GET");

                GetProductModel getProductModel = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                if (getProductModel.ImagesModel != null && getProductModel.ImagesModel.Count > 0)
                {
                    for (int i = 0; i < getProductModel.ImagesModel.Count; i++)
                    {
                        string key = "afbeelding" + i;
                        ViewData[key] = ImageHelper.GetImage(getProductModel.ImagesModel.ToArray()[i].Data, getProductModel.ImagesModel.ToArray()[i].FileType, "product");
                    }
                }

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
                Authorize("Moderator", "Delete");

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
                    error = _localizer["Onvoldoende rechten om producten op te vragen"];
                    break;
                case "Details":
                    error = _localizer["Onvoldoende rechten om de details van een product op te vragen"];
                    break;
                case "Create":
                    error = _localizer["Onvoldoende rechten om product aan te maken"];
                    break;
                case "Edit":
                    error = _localizer["Onvoldoende rechten om product aan te passen"];
                    break;
                case "Delete":
                    error = _localizer["Onvoldoende rechten om product te verwijderen"];
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
