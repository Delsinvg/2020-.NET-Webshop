using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Orders;
using project.models.Products;
using project.web.Helpers;
using project.web.Models;
using project.web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Controllers
{
    public class CartController : Controller
    {
        private readonly ProjectApiService _projectApiService;
        private readonly ITokenValidationService _tokenValidationService;

        public CartController(ProjectApiService projectApiService, ITokenValidationService tokenValidationService)
        {
            _projectApiService = projectApiService;
            _tokenValidationService = tokenValidationService;
        }

        public IActionResult Index()
        {
            try
            {

                Authorize("Customer", "Index");

                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                if (cart != null)
                {
                    ViewBag.cart = cart;
                    ViewBag.itemsCount = cart.Count;
                    ViewBag.total = cart.Sum(x => x.Product.Price * x.Quantity);
                }
                else
                {
                    ViewBag.cart = cart;
                }

                return View();
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        private int ProductAlreadyInCard(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                string productId = cart[i].Product.Id.ToString();
                string trimmedId = productId.Trim('{');
                if (trimmedId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public async Task<IActionResult> Buy(string id)
        {
            try
            {

                Authorize("Customer", "Index");

                GetProductModel product = await _projectApiService.GetModel<GetProductModel>(id, "Products");

                if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
                {
                    List<Item> cart = new List<Item>();
                    cart.Add(new Item { Quantity = 1, Product = product });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                    int index = ProductAlreadyInCard(id);
                    if (index != -1)
                    {
                        cart[index].Quantity++;
                    }
                    else
                    {
                        cart.Add(new Item { Quantity = 1, Product = product });
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                return RedirectToAction("Index");

            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }

        public IActionResult Remove(string id)
        {
            try
            {
                Authorize("Customer", "Index");

                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = ProductAlreadyInCard(id);
                cart.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                return RedirectToAction("Index");
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
                    error = "Onvoldoende rechten om shoppingcard op te halen";
                    break;
                case "Details":
                    error = "Onvoldoende rechten om details van shoppingcard op te halen";
                    break;
                case "Create":
                    error = "Onvoldoende rechten om shoppingcard op te maken";
                    break;
                case "Edit":
                    error = "Onvoldoende rechten om shoppingcard aan te passen";
                    break;
                case "Delete":
                    error = "Onvoldoende rechten om shoppingcard te verwijderen";
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
                return RedirectToRoute(new { action = "Index", controller = "Authentication" });
            }
        }

        public async Task<IActionResult> CheckOut(string userId)
        {
            try
            {
                Authorize("Customer", "Create");

                await _tokenValidationService.Validate(this.GetType().Name, "Create POST");

                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");


                PostOrderModel postOrderModel = new PostOrderModel
                {
                    UserId = new Guid(userId),
                    Products = cart.Select(x => new OrderProductModel { ProductId = x.Product.Id, Quantity = x.Quantity, Price = x.Product.Price * x.Quantity }).ToList(),
                    Orderdate = DateTime.Now
                };

                if (ModelState.IsValid)
                {
                    GetOrderModel getOrderModel = await _projectApiService.PostModel<PostOrderModel, GetOrderModel>(postOrderModel, "Orders");
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);

                    return RedirectToRoute(new { action = "Index", controller = "Products" });
                }
                return RedirectToRoute(new { action = "Details", controller = "Users" });
            }
            catch (ProjectException e)
            {
                return HandleError(e);
            }
        }
    }
}


