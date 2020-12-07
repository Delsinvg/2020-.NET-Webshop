using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Get a list of all orders.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/orders
        ///
        /// </remarks>
        /// <returns>List of GetOrderModel</returns>
        /// <response code="200">Returns the list of orders</response>
        /// <response code="404">No orders were found</response> 

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<List<GetOrderModel>>> GetOrders()
        {
            try
            {
                List<GetOrderModel> orders = await _orderRepository.GetOrders();

                return orders;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        /// <summary>
        /// Get details of an order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/orders/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <returns>An GetOrderModel</returns>
        /// <response code="200">Returns the order</response>
        /// <response code="404">The order could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<GetOrderModel>> GetOrder(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid orderId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetOrder", "400");
                }

                GetOrderModel order = await _orderRepository.GetOrder(orderId);

                return order;
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
        /// Creates an order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/orders
        ///     {
        ///        "orderdate": "Date of the order"
        ///        "userId": "Id of the user"
        ///        "products": "Id of the products"
        ///     }
        ///
        /// </remarks>
        /// <param name="postOrderModel"></param>
        /// <returns>A newly created order</returns>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="400">If something went wrong while saving into the database</response> 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<GetOrderModel>> PostOrder(PostOrderModel postOrderModel)
        {
            try
            {
                GetOrderModel order = await _orderRepository.PostOrder(postOrderModel);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
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
        /// Updates an order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/orders/{id}
        ///     {
        ///        "orderdate": "Date of the order"
        ///        "userId": "Id of the user"
        ///        "products": "Id of the products"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <param name="putOrderModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The order could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Putorder(string id, PutOrderModel putOrderModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid orderId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutOrder", "400");
                }

                await _orderRepository.PutOrder(orderId, putOrderModel);

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
        /// Deletes an order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/orders/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <response code="204">Returns no content</response>
        /// <response code="404">The order could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid orderId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteOrder", "400");
                }

                await _orderRepository.DeleteOrder(orderId);

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
