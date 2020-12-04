using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Addresses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressesController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        /// <summary>
        /// Get a list of all addresses.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/addresses
        ///
        /// </remarks>
        /// <returns>List of GetAddressModel</returns>
        /// <response code="200">Returns the list of addresses</response>
        /// <response code="404">No address were found</response> 

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Moderator")]
        public async Task<ActionResult<List<GetAddressModel>>> GetAddresses()
        {
            try
            {
                List<GetAddressModel> addresses = await _addressRepository.GetAddresses();

                return addresses;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        /// <summary>
        /// Get details of an address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/address/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <returns>An GetAddressModel</returns>
        /// <response code="200">Returns the address</response>
        /// <response code="404">The address could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response>

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Moderator")]
        public async Task<ActionResult<GetAddressModel>> GetAddress(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid addressId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetAddress", "400");
                }

                GetAddressModel address = await _addressRepository.GetAddress(addressId);

                return address;
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
        /// Creates an address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/address
        ///     {
        ///        "countryCode": "Code of the Country"
        ///        "country": "Name of the country"
        ///        "city": "Name of the city"
        ///        "postalCode": "postalcode of the city"
        ///        "street": "street name and number"
        ///     }
        ///
        /// </remarks>
        /// <param name="postAddressModel"></param>
        /// <returns>A newly created address</returns>
        /// <response code="201">Returns the newly created address</response>
        /// <response code="400">If something went wrong while saving into the database</response> 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<GetAddressModel>> PostAddress(PostAddressModel postAddressModel)
        {
            try
            {
                GetAddressModel address = await _addressRepository.PostAddress(postAddressModel);

                return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
            }
            catch (DatabaseException e)
            {
                return BadRequest(e.ProjectError);
            }
        }

        /// <summary>
        /// Updates an address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/address/{id}
        ///     {
        ///        "countryCode": "Code of the Country"
        ///        "country": "Name of the country"
        ///        "city": "Name of the city"
        ///        "postalCode": "postalcode of the city"
        ///        "street": "street name and number"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <param name="putAddressModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The address could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutAddress(string id, PutAddressModel putAddressModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid addressId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutAddress", "400");
                }

                await _addressRepository.PutAddress(addressId, putAddressModel);

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
        /// Deletes an address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/address/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <response code="204">Returns no content</response>
        /// <response code="404">The address could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid addressId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteAuteur", "400");
                }

                await _addressRepository.DeleteAddress(addressId);

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
