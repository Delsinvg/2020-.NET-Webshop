using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Roles;

namespace project.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetRoleModel>>> GetRoles()
        {
            List<GetRoleModel> roles = await _roleRepository.GetRoles();

            if (roles.Count.Equals(0))
            {
                return NotFound();
            }

            return roles;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetRoleModel>> GetRole(string id)
        {
            try
            {
                GetRoleModel role = await _roleRepository.GetRole(id);

                if (role == null)
                {
                    return NotFound();
                }

                return role;
            }
            catch (GuidException e)
            {
                return BadRequest(e.ProjectError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetRoleModel>> PostRole(PostRoleModel postRoleModel)
        {
            try
            {
                GetRoleModel role = await _roleRepository.PostRole(postRoleModel);

                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRole(string id, PutRoleModel putRoleModel)
        {
            try
            {
                await _roleRepository.PutRole(id, putRoleModel);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                await _roleRepository.DeleteRole(id);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
            }
        }
    }
}
