using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Products;

namespace project.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetProductModel>> GetProduct(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid productId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetBoek", "400");
                }

                GetProductModel boek = await _productsRepository.GetProduct(productId);

                return boek;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetProductModel>> PostBoek(PostProductModel postBoekModel)
        {
            try
            {
                GetProductModel product = await _productsRepository.PostProduct(postBoekModel);

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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
