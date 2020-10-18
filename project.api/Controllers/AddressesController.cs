using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Repositories;
using project.models.Addresses;

namespace project.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressesController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetAddressModel>>> GetAddresses()
        {
            return await _addressRepository.GetAddresses();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAddressModel>> GetAddress(string id)
        {
            return await _addressRepository.GetAddress(id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GetAddressModel>> PostAddress(PostAddressModel postAddressModel)
        {
            GetAddressModel address = await _addressRepository.PostAddress(postAddressModel);

            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutAddress(string id, PutAddressModel putAddressModel)
        {
            await _addressRepository.PutAddress(id, putAddressModel);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAddress(string id)
        {
            await _addressRepository.DeleteAddress(id);

            return NoContent();
        }
    }
}
