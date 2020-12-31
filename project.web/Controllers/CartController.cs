using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.models.Products;
using project.web.Helpers;
using project.web.Models;
using project.web.Services;
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
            //Authorize("Customer", "Index");

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

        public IActionResult Remove(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = ProductAlreadyInCard(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
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
    }
}

