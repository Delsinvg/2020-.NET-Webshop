using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Orders;

namespace project.api.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetOrderModel>> GetOrder(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid orderId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetOrder", "400");
                }

                GetOrderModel boek = await _orderRepository.GetOrder(orderId);

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
        public async Task<ActionResult<GetOrderModel>> PostBoek(PostOrderModel postOrderModel)
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
