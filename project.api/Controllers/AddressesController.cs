﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Addresses;

namespace project.api.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
