//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using project.api.Exceptions;
//using project.api.Repositories;
//using project.models.OrderProducts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;

//namespace project.api.Controllers
//{
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        [ApiController]
//        [Route("api/[controller]")]
//        [Produces("application/json")]
//        [Consumes("application/json")]
//        public class  OrderProductController : ControllerBase
//        {
//            private readonly IOrderProductRepository _orderProductRepository;

//            public OrderProductController(IOrderProductRepository orderProductRepository)
//            {
//                _orderProductRepository = orderProductRepository;
//            }

//            /// <summary>
//            /// Get a list of all orderproducts.
//            /// </summary>
//            /// <remarks>
//            /// Sample request:
//            ///
//            ///     GET api/orderproducts
//            ///
//            /// </remarks>
//            /// <returns>GetOrderProductsModel</returns>
//            /// <response code="200">Returns the list of all OrderProducts</response>
//            /// <response code="401">Unauthorized - Invalid JWT token</response> 
//            /// <response code="403">Forbidden - Required role assignment is missing</response> 
//            [HttpGet]
//            [ProducesResponseType(StatusCodes.Status200OK)]
//            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//            [ProducesResponseType(StatusCodes.Status403Forbidden)]
//            [Authorize(Roles = "Moderator")]
//            public async Task<ActionResult<List<GetOrderProductModel>>> GetOrderProducts()
//            {
//                return await _orderProductRepository.GetOrderProducts();
//            }

//        /// <summary>
//        /// Get details of an OrderProduct.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     GET /api/OrderProducts/{id}
//        ///
//        /// </remarks>
//        /// <param name="id"></param>   
//        /// <returns>An GetOrderProductModel</returns>
//        /// <response code="200">Returns the OrderProduct</response>
//        /// <response code="400">The id is not a valid Guid</response> 
//        /// <response code="401">Unauthorized - Invalid JWT token</response> 
//        /// <response code="403">Forbidden - Required role assignment is missing</response> 
//        /// <response code="404">The OrderProduct could not be found</response> 
//        [HttpGet("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Authorize(Roles = "Moderator")]
//        public async Task<ActionResult<GetOrderProductModel>> GetOrderProduct(string id)
//        {
//            try
//            {
//                if (!Guid.TryParse(id, out Guid orderProductId))
//                {
//                    throw new GuidException("Ongeldig id", this.GetType().Name, "GetOrderProduct", "400");
//                }

//                GetOrderProductModel OrderProduct = await _orderProductRepository.GetOrderProduct(orderProductId.ToString());

//                return OrderProduct;
//            }
//            catch (ProjectException e)
//            {
//                if (e.ProjectError.Status.Equals("404"))
//                {
//                    return NotFound(e.ProjectError);
//                }
//                else if (e.ProjectError.Status.Equals("403"))
//                {
//                    return new ObjectResult(e.ProjectError)
//                    {
//                        StatusCode = (int)HttpStatusCode.Forbidden
//                    };
//                }
//                else
//                {
//                    return BadRequest(e.ProjectError);
//                }
//            }
//        }

//        /// <summary>
//        /// Creates one or more OrderProducts.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST /api/OrderProducts
//        ///     {
//        ///        "ExemplaarId": "f0bf443b-f644-4248-afa7-08d86f9f8e21"
//        ///     }
//        ///
//        /// </remarks>
//        /// <param name="postOrderProductsModel"></param>
//        /// <response code="201">Returns the newly created OrderProduct</response>
//        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>    
//        /// <response code="401">Unauthorized - Invalid JWT token</response> 
//        /// <response code="403">Forbidden - Required role assignment is missing</response> 
//        /// <response code="404">The user could not be found</response>
//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Authorize(Roles = "Moderator")]
//        public async Task<IActionResult> PostOrderProducts(PostOrderProductsModel postOrderProductsModel)
//        {
//            try
//            {
//                await _orderProductRepository.PostOrderProducts(postOrderProductsModel);

//                return NoContent();
//            }
//            catch (ProjectException e)
//            {
//                if (e.ProjectError.Status.Equals("404"))
//                {
//                    return NotFound(e.ProjectError);
//                }
//                else if (e.ProjectError.Status.Equals("403"))
//                {
//                    return new ObjectResult(e.ProjectError)
//                    {
//                        StatusCode = (int)HttpStatusCode.Forbidden
//                    };
//                }
//                else
//                {
//                    return BadRequest(e.ProjectError);
//                }
//            }
//        }

//        /// <summary>
//        /// Updates an OrderProduct.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     PUT /api/OrderProducts/{id}
//        ///     {
//        ///        "AantalVerlengingen": 1,
//        ///        "Teruggebracht": true
//        ///     }
//        ///
//        /// </remarks>
//        /// <param name="id"></param>   
//        /// <param name="putOrderProductModel"></param>   
//        /// <response code="204">Returns no content</response>
//        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response> 
//        /// <response code="401">Unauthorized - Invalid JWT token</response> 
//        /// <response code="404">The OrderProduct could not be found</response> 
//        [HttpPut("{id}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Authorize(Roles = "Moderator")]
//        public async Task<IActionResult> PutOrderProduct(string id, PutOrderProductModel putOrderProductModel)
//        {
//            try
//            {
//                if (!Guid.TryParse(id, out Guid OrderProductId))
//                {
//                    throw new GuidException("Ongeldig id", this.GetType().Name, "PutOrderProduct", "400");
//                }

//                await _orderProductRepository.PutOrderProduct(OrderProductId, putOrderProductModel);

//                return NoContent();
//            }
//            catch (ProjectException e)
//            {
//                if (e.ProjectError.Status.Equals("404"))
//                {
//                    return NotFound(e.ProjectError);
//                }
//                else if (e.ProjectError.Status.Equals("403"))
//                {
//                    return new ObjectResult(e.ProjectError)
//                    {
//                        StatusCode = (int)HttpStatusCode.Forbidden
//                    };
//                }
//                else
//                {
//                    return BadRequest(e.ProjectError);
//                }
//            }
//        }

//        /// <summary>
//        /// Deletes an OrderProduct.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     DELETE /api/OrderProducts/{id}
//        ///
//        /// </remarks>
//        /// <param name="id"></param>   
//        /// <response code="204">Returns no content</response>
//        /// <response code="400">The id is not a valid Guid</response> 
//        /// <response code="401">Unauthorized - Invalid JWT token</response> 
//        /// <response code="403">Forbidden - Required role assignment is missing</response> 
//        /// <response code="404">The OrderProduct could not be found</response> 
//        [HttpDelete("{id}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//        [ProducesResponseType(StatusCodes.Status403Forbidden)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        [Authorize(Roles = "Moderator")]
//        public async Task<IActionResult> DeleteOrderProduct(string id)
//        {
//            try
//            {
//                if (!Guid.TryParse(id, out Guid orderProductId))
//                {
//                    throw new GuidException("Ongeldig id", this.GetType().Name, "DeleteOrderProduct", "400");
//                }

//                await _orderProductRepository.DeleteOrderProduct(orderProductId.ToString());

//                return NoContent();
//            }
//            catch (ProjectException e)
//            {
//                if (e.ProjectError.Status.Equals("404"))
//                {
//                    return NotFound(e.ProjectError);
//                }
//                else
//                {
//                    return BadRequest(e.ProjectError);
//                }
//            }
//        }
//    }
//}
