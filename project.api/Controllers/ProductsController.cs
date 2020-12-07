using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        /// <summary>
        /// Get a list of all products.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/products
        ///
        /// </remarks>
        /// <returns>List of GetProductModel</returns>
        /// <response code="200">Returns the list of products</response>
        /// <response code="404">No products were found</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetProductModel>>> GetProducts()
        {
            try
            {
                List<GetProductModel> products = await _productsRepository.GetProducts();

                return products;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        /// <summary>
        /// Get details of an product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/products/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <returns>An GetOrderModel</returns>
        /// <response code="200">Returns the product</response>
        /// <response code="404">The order could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<GetProductModel>> GetProduct(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid productId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetProduct", "400");
                }

                GetProductModel product = await _productsRepository.GetProduct(productId);

                return product;
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Creates an product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/products
        ///     {
        ///        "name": "Name of the product"
        ///        "stock": "Number of products"
        ///        "description": "Description of the product"
        ///        "price": "price of the product"
        ///        "categoryId": "Id of the category"
        ///        "companyId": "Id of the company"
        ///     }
        ///
        /// </remarks>
        /// <param name="postProductModel"></param>
        /// <returns>A newly created product</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If something went wrong while saving into the database</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<GetProductModel>> PostProduct(PostProductModel postProductModel)
        {
            try
            {
                GetProductModel product = await _productsRepository.PostProduct(postProductModel);

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Updates an product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/products/{id}
        ///     {
        ///        "name": "Name of the product"
        ///        "stock": "Number of products"
        ///        "description": "Description of the product"
        ///        "price": "price of the product"
        ///        "categoryId": "Id of the category"
        ///        "companyId": "Id of the company"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <param name="putProductModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The product could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> PutProduct(string id, PutProductModel putProductModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid productId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutProduct", "400");
                }

                await _productsRepository.PutProduct(productId, putProductModel);

                return NoContent();
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Deletes an product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/products/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <response code="204">Returns no content</response>
        /// <response code="404">The product could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid productId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteProduct", "400");
                }

                await _productsRepository.DeleteProduct(productId);

                return NoContent();
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }
    }
}
